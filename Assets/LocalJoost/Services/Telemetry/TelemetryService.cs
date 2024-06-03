using System;
using RealityCollective.ServiceFramework.Services;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using UnityEngine;

namespace LocalJoost.Services.Telemetry
{
    [System.Runtime.InteropServices.Guid("b4fb9540-d404-4cb9-9358-ea95bf1541cb")]
    public class TelemetryService : BaseServiceWithConstructor, ITelemetryService
    {
        private string user;
        private TelemetryClient telemetryClient;
        private TelemetryServiceProfile serviceProfile;
        private List<string> logKeywordsToTrack = new();

        public TelemetryService(string name, uint priority, TelemetryServiceProfile profile)
            : base(name, priority)
        {
            serviceProfile = profile;
        }
        
        public void Initialize(string connectionString, string userId = null)
        {
            if(telemetryClient != null)
            {
                return;
            }
            user = userId;
            var config = new TelemetryConfiguration();
            config.ConnectionString = connectionString;
            telemetryClient = new TelemetryClient(config);
            AddTelemetryContext(telemetryClient.Context);
            TrackUnityLogKeywords();
        }
        
        public void TrackEvent(string eventName, Dictionary<string, string> properties = null)
        {
            if( telemetryClient == null )
            {
                return;
            }
            telemetryClient.TrackEvent(eventName, properties);
            telemetryClient.Flush();
            if(serviceProfile != null && serviceProfile.LogToConsole && !eventName.Contains("Keyword tracked"))
            {
                var propertiesString = properties != null
                    ? string.Join(", ", properties.Select(p => $"{p.Key}: {p.Value}"))
                    : string.Empty;
                Debug.Log($"Telemetry event: {eventName} {propertiesString}");
            }
        }
        
        public void TrackEvent(string eventName, params string[] properties)
        {
            if (telemetryClient == null)
            {
                return;
            }

            if (properties.Length % 2 != 0 && properties.Length > 0)
            {
                properties = properties.Take(properties.Length - 1).ToArray();
            }

            var telemetryProperties = new Dictionary<string, string>();
            for (var index = 0; index < properties.Length - 1; index++)
            {
                var propName = properties[index];
                if (!telemetryProperties.ContainsKey(propName))
                {
                    telemetryProperties.Add(propName, properties[index + 1]);
                }
            }

            TrackEvent(eventName, telemetryProperties);
        }
        
        public void TrackException(Exception exception)
        {
            if (telemetryClient == null)
            {
                return;
            }
            var exceptionTelemetry = new ExceptionTelemetry(exception);
            telemetryClient.TrackException(exceptionTelemetry);
            telemetryClient.Flush();
            if(serviceProfile != null && serviceProfile.LogToConsole )
            {
                Debug.Log($"Telemetry exception: {exception}");
            }
        }

        private void TrackUnityLogKeywords()
        {
            if (serviceProfile == null)
            {
                return;
            }
            if (serviceProfile.LogKeywordsToTrack.Any())
            {
                Application.logMessageReceivedThreaded += OnLogMessageReceived;
            }

            foreach (var keyword in serviceProfile.LogKeywordsToTrack)
            {
                if (!logKeywordsToTrack.Contains(keyword))
                {
                    logKeywordsToTrack.Add(keyword);
                }
            }
        }

        private void OnLogMessageReceived(string condition, string stacktrace, LogType type)
        {
            var keywordFound = logKeywordsToTrack.FirstOrDefault(condition.Contains);
            if (keywordFound != null)
            {
                TrackEvent($"Keyword tracked: {keywordFound}", 
                    "message", condition, "stacktrace", stacktrace);
            }
        }

        private void AddTelemetryContext(TelemetryContext context)
        {
            context.Component.Version = GetAppVersion();
            context.Device.Id = SystemInfo.deviceUniqueIdentifier;
            context.Device.OperatingSystem = SystemInfo.operatingSystem;
            context.Device.Type = SystemInfo.deviceModel;
            if (user != null)
            {
                context.User.Id = user;
            }
        }

        private string GetAppVersion()
        {
#if WINDOWS_UWP
            var version = Windows.ApplicationModel.Package.Current.Id.Version;
            return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
#else
            return Application.version;
#endif        
        }
    }
}
