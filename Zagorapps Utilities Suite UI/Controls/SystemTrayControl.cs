﻿namespace Zagorapps.Utilities.Suite.UI.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using Hardcodet.Wpf.TaskbarNotification;
    using Zagorapps.Core.Library.Events;
    using DrawingIcon = System.Drawing.Icon;

    public class SystemTrayControl : ISystemTrayControl
    {
        protected readonly TaskbarIcon TrayIcon;

        public SystemTrayControl(ContextMenu menu, DrawingIcon icon, string toolTipText)
        {
            if (menu == null)
            {
                throw new ArgumentNullException(nameof(menu), "No context menu has been provided");
            }

            if (icon == null)
            { 
                throw new ArgumentNullException(nameof(icon), "No tray icon has been provided");
            }

            this.TrayIcon = new TaskbarIcon();
            this.TrayIcon.MenuActivation = PopupActivationMode.LeftOrRightClick;
            this.TrayIcon.Visibility = Visibility.Hidden;
            this.TrayIcon.Icon = icon;
            this.TrayIcon.ToolTipText = toolTipText;
            this.TrayIcon.TrayMouseDoubleClick += this.TrayIcon_TrayMouseDoubleClick;
            this.TrayIcon.ContextMenu = menu;

            MenuItem open = this.FindMenuItem(menu, App.MenuItemOpenApplication);
            open.Header = "Open " + App.Name;
            open.Click += this.Open_Click;

            MenuItem close = this.FindMenuItem(menu, App.MenuItemCloseApplication);
            close.Header = "Terminate";
            close.Click += this.Close_Click;
        }

        public event EventHandler<EventArgs<TrayState>> StateChanged;

        public void SetVisibility(Visibility visiblity)
        {
            this.TrayIcon.Visibility = visiblity;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            this.RequestApplicationDisplay();
        }

        private void TrayIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            this.RequestApplicationDisplay();
        }

        private void RequestApplicationDisplay()
        {
            Invoker.Raise(ref this.StateChanged, this, TrayState.ShowApplication);
        }

        private MenuItem FindMenuItem(ContextMenu menu, string name)
        {
            ItemCollection collection = menu.Items;

            MenuItem item = null;

            bool found = false;
            while (collection.MoveCurrentToNext() && !found)
            {
                item = collection.CurrentItem as MenuItem;

                if (item.Name == name)
                {
                    found = true;
                    collection.MoveCurrentToFirst();
                }
            }

            return item;
        }

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.TrayIcon.Dispose();
            }
        }
    }
}