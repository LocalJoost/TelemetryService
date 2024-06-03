using System.Collections.Generic;
using RealityCollective.ServiceFramework.Definitions;
using RealityCollective.ServiceFramework.Interfaces;
using UnityEngine;

namespace LocalJoost.Services.Telemetry
{
    [CreateAssetMenu(menuName = "TelemetryServiceProfile", fileName = "TelemetryServiceProfile",
        order = (int)CreateProfileMenuItemIndices.ServiceConfig)]
    public class TelemetryServiceProfile : BaseServiceProfile<IServiceModule>
    {
        [SerializeField]
        public List<string> LogKeywordsToTrack = new(){"NullReferenceException", "MissingReferenceException"};

        [SerializeField]
        public bool LogToConsole = false;
    }
}
