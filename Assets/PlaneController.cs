using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneController : MonoBehaviour
{
    private ARPlaneManager arPlaneManager;

    private void Awake() {
        arPlaneManager = GetComponent<ARPlaneManager>();
    }

    public void SetAllPlanesActive(bool value)
    {
        arPlaneManager.enabled = value;
        foreach (var plane in arPlaneManager.trackables)
            plane.gameObject.SetActive(value);
    }
}
