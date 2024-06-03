using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MRTKExtensions.Utilities
{
    public class DeviceTypeEnableController : BaseController
    {
        [SerializeField]
        private List<string> supportedDeviceTypes;

        [SerializeField]
        private bool enableOnSupportedDeviceTypes = true;


        protected override void Awake()
        {
            base.Awake();
            var result = supportedDeviceTypes.Any(supportedDeviceType =>
                SystemInfo.deviceModel.Contains(supportedDeviceType));
            if (!enableOnSupportedDeviceTypes)
            {
                result = !result;
            }

            visuals.SetActive(result);
        }
    }
}