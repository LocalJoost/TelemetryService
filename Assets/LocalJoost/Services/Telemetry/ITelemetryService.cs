using System;
using System.Collections.Generic;
using RealityCollective.ServiceFramework.Interfaces;

namespace LocalJoost.Services.Telemetry
{
    public interface ITelemetryService : IService
    {
        void Initialize(string connectionString, string userId = null);
        public void TrackEvent(string eventName, Dictionary<string, string> properties = null);
        public void TrackEvent(string eventName, params string[] properties);
        public void TrackException(Exception exception);
    }
}