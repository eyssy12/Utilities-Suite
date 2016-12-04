﻿namespace Zagorapps.Utilities.Suite.WCF.Library.Services
{
    using System;
    using System.ServiceModel;
    using Core.Library.Events;
    using Providers;
    using Suite.Library.Communications;

    [ServiceContract]
    [ServiceKnownType("GetTypes", typeof(KnownTypeProvider<UtilitiesDataMessage>))]
    public interface IUtilitiesSuiteService
    {
        event EventHandler<EventArgs<IUtilitiesDataMessage>> MessageReceived;

        [OperationContract(IsOneWay = true)]
        void Handle(IUtilitiesDataMessage message);
    }
}