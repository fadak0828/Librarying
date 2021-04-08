using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[System.Serializable]
public struct TargetInfo
{
    public string name;
    public GameObject targetObj;
}

public class MarkerManager : MonoBehaviour
{
    public static MarkerManager Instance;
    public List<TargetInfo> targetList;
    private ARTrackedImageManager manager;

    private bool isLocked;

    private void Awake() {
        Instance = this;
    }

    private void OnEnable()
    {
        manager = GetComponent<ARTrackedImageManager>();
        manager.trackedImagesChanged += OnTrackedImagesChaged;
    }

    private void OnDisable()
    {
        manager.trackedImagesChanged -= OnTrackedImagesChaged;
    }

    public void SetLock(bool isLocked) 
    {
        this.isLocked = isLocked;
    }

    private void OnTrackedImagesChaged(ARTrackedImagesChangedEventArgs args)
    {
        if (isLocked) return;
         
        targetList.ForEach(target => {
            args.updated
                .Where(ti => ti.referenceImage.name == target.name)
                .ToList()
                .ForEach(ti => {
                    if (ti.trackingState == TrackingState.Tracking) {
                        Vector3 targetNewPosition = ti.transform.position;

                        target.targetObj.SetActive(true);
                        target.targetObj.transform.SetParent(ti.transform);
                        target.targetObj.transform.localPosition = Vector3.zero;
                        target.targetObj.transform.localRotation = Quaternion.identity;
                        target.targetObj.transform.SetParent(null, true);
                    } else {
                        target.targetObj.SetActive(false);
                    }
                });
        });
    }
}
