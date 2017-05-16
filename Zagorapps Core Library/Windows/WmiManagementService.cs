namespace Zagorapps.Core.Library.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management;
    using Events.Windows;
    using Extensions;
    using Models;
    using Zagorapps.Core.Library.Events;

    public class WmiManagementService : IWmiManagementService
    {
        protected const string QuerySelectAllTemplate = "Select * from {0}",
            ClassNameMonitorBrightnessMethods = "WmiMonitorBrightnessMethods",
            ClassNameMonitorBrightness = "WmiMonitorBrightness",
            ClassNameMonitorBrightnessEvent = "WmiMonitorBrightnessEvent",
            ClassMethodSetBrightness = "WmiSetBrightness",
            ClassPropertyInstanceName = "InstanceName",
            ClassPropertyActive = "Active",
            ClassPropertyBrightness = "Brightness",
            ClassPropertyCurrentBrightness = "CurrentBrightness";

        private ManagementEventWatcher watcher;
        private ManagementScope scope;

        private Lazy<bool> isWmiSupported;

        private bool isStarted = false;

        public WmiManagementService()
        {
            string computerName = "localhost";
            string path = string.Format("\\\\{0}\\root\\WMI", computerName);

            if (computerName.Equals(computerName, StringComparison.OrdinalIgnoreCase))
            {
                this.scope = new ManagementScope(path, null);
            }
            else
            {
                ConnectionOptions Conn = new ConnectionOptions
                {
                    Username = string.Empty,
                    Password = string.Empty,
                    Authority = "ntlmdomain:DOMAIN"
                };
                this.scope = new ManagementScope(path, Conn);
            }

            this.isWmiSupported = new Lazy<bool>(() => this.GetBrightnesses().ToArray().Any());
        }

        public event EventHandler<WmiEventArgs> EventReceived;

        public event EventHandler<EventArgs<Exception>> FailureRaised;

        public bool IsWmiSupported
        {
            get { return this.isWmiSupported.Value; }
        }

        public bool Start()
        {
            try
            {
                this.scope.Connect();

                this.watcher = new ManagementEventWatcher(this.scope, new EventQuery(string.Format(WmiManagementService.QuerySelectAllTemplate, WmiManagementService.ClassNameMonitorBrightnessEvent)));
                this.watcher.EventArrived += this.Watcher_EventArrived;
                this.watcher.Start();

                return this.IsWmiSupported;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception {0} Trace {1}", e.Message, e.StackTrace);

                return false;
            }
        }

        public bool Stop()
        {
            if (this.isStarted)
            {
                this.watcher.Stop();
                this.watcher.EventArrived -= this.Watcher_EventArrived;

                return true;
            }

            return false;
        }

        public IEnumerable<WmiDeviceInfo> GetBrightnesses()
        {
            ObjectQuery query = new ObjectQuery(string.Format(WmiManagementService.QuerySelectAllTemplate, WmiManagementService.ClassNameMonitorBrightness));

            return this.InvokeWmiOperation(
                query,
                @object =>
                {
                    return new WmiDeviceInfo
                    {
                        Identity = (string)@object[WmiManagementService.ClassPropertyInstanceName],
                        Brightness = (byte)@object[WmiManagementService.ClassPropertyCurrentBrightness]
                    };
                })
                .ToArray();
        }

        public void SetBrightness(int value)
        {
            SelectQuery query = new SelectQuery(WmiManagementService.ClassNameMonitorBrightnessMethods);

            this.InvokeWmiOperation(
                query,
                @object =>
                {
                    @object.InvokeMethod(WmiManagementService.ClassMethodSetBrightness, new Object[] { UInt32.MaxValue, value.ClipToRange(0, 100) });
                });
        }

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.watcher != null)
                {
                    this.watcher.Dispose();
                }
            }
        }

        protected void InvokeWmiOperation(ObjectQuery query, Action<ManagementObject> operation)
        {
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(this.scope, query))
                {
                    using (ManagementObjectCollection objectCollection = searcher.Get())
                    {
                        foreach (ManagementObject @object in objectCollection)
                        {
                            operation(@object);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Invoker.Raise(ref this.FailureRaised, this, ex);
            }
        }

        protected IEnumerable<T> InvokeWmiOperation<T>(ObjectQuery query, Func<ManagementObject, T> operation)
        {
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(this.scope, query))
                {
                    using (ManagementObjectCollection objectCollection = searcher.Get())
                    {
                        foreach (ManagementObject @object in objectCollection)
                        {
                            return new[] { operation(@object) };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Invoker.Raise(ref this.FailureRaised, this, ex);

                return Enumerable.Empty<T>();
            }

            return Enumerable.Empty<T>();
        }

        private void Watcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            WmiEventArgs args = new WmiEventArgs(
                this.GetPropertyValue<bool>(e, WmiManagementService.ClassPropertyActive),
                this.GetPropertyValue<byte>(e, WmiManagementService.ClassPropertyBrightness),
                this.GetPropertyValue<string>(e, WmiManagementService.ClassPropertyInstanceName));

            this.EventReceived(sender, args);
        }

        private T GetPropertyValue<T>(EventArrivedEventArgs args, string propertyName)
        {
            try
            {
                return (T)args.NewEvent.Properties[propertyName].Value;
            }
            catch
            {
                return default(T);
            }
        }
    }
}