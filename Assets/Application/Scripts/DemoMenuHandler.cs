using System;
using System.Collections;
using System.Collections.Generic;
using LocalJoost.Services.Telemetry;
using RealityCollective.ServiceFramework.Services;
using UnityEngine;

public class DemoMenuHandler : MonoBehaviour
{
    private ITelemetryService telemetryService;

    private async void Start()
    {
        await ServiceManager.WaitUntilInitializedAsync();
        telemetryService = ServiceManager.Instance.GetService<ITelemetryService>();
        telemetryService.Initialize("your_connection_string_here");
    }

    public void HelloWorld()
    {
        telemetryService.TrackEvent("Hello world button clicked", 
            "prop1", "value1", "prop2", "value2");
        telemetryService.TrackEvent("Hello world button clicked", 
            new Dictionary<string, string>
            {
                {"prop1", "value1"},
                {"prop2", "value2"}
            });
    }
    
    public void NullReferenceException()
    {
        string s = null;
        var a = s.ToString();
    }
    
    public void AccessDestroyedObject()
    {
        var g = new GameObject();
        StartCoroutine(AccessDestroyedObject(g));
    }
    
    private IEnumerator AccessDestroyedObject(GameObject g)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(g);
        g.SetActive(false);
    }
    
    public void OtherError()
    {
        try
        {
            var a = new int[10];
            var b = a[11];
        }
        catch (Exception e)
        {
            telemetryService.TrackException(e);
            throw;
        }
    }
}
