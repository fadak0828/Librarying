using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[System.Serializable]
public class TargetInfo
{
    public string name;
    public GameObject targetPref;

    [HideInInspector]
    public GameObject createdObj;
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
                        // 화면에 이미지가 보이는 경우

                        if (target.createdObj == null) {
                            // 오브젝트가 아직 생성되지 않았으면 새로 생성
                            target.createdObj = Instantiate(target.targetPref);
                        } 

                        // 오브젝트 위치, 회전값 초기화
                        target.createdObj.transform.SetParent(ti.transform);
                        target.createdObj.transform.localPosition = Vector3.zero;
                        target.createdObj.transform.localRotation = Quaternion.identity;
                        target.createdObj.transform.SetParent(null, true);
                    } else {
                        // 화면에서 이미지가 사라진 경우

                        if (target.createdObj != null) {
                            // 만들어진 오브젝트가 있으면 파괴
                            Destroy(target.createdObj);
                            target.createdObj = null;
                        } 
                    }
                });
        });
    }
}
