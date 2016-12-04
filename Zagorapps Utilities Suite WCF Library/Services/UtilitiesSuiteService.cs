namespace Zagorapps.Utilities.Suite.WCF.Library.Services
{
    using System;
    using System.ServiceModel;
    using Core.Library.Events;
    using Suite.Library.Communications;

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class UtilitiesSuiteService : IUtilitiesSuiteService
    {
        public event EventHandler<EventArgs<IUtilitiesDataMessage>> MessageReceived;

        public void Handle(IUtilitiesDataMessage message)
        {
            Invoker.Raise(ref this.MessageReceived, this, message);
        }
    }
}