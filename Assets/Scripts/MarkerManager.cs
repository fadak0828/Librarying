using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[System.Serializable]
public class TargetInfo
{
    [Tooltip("ImageLibrary에 등록된 이미지 이름")]
    public string name;

    [Tooltip("이미지를 따라서 위치 변경할 것인지?")]
    public bool enableTracking = true;

    [Tooltip("이미지 따라서 회전할 것인지?")]
    public bool trackingRoation = true;

    [Tooltip("이미지가 안보이면 파괴할 것인지?")]
    public bool dontDestory = false;
    public GameObject targetPref;

    [HideInInspector]
    public GameObject createdObj;
}

public class MarkerManager : MonoBehaviour
{
    public static MarkerManager Instance;
    public List<TargetInfo> targetList;
    private ARTrackedImageManager manager;
    private string[] trackingNames;

    private bool isLocked;

    private void Awake() {
        Instance = this;
    }

    private void OnEnable()
    {
        manager = GetComponent<ARTrackedImageManager>();
        trackingNames = targetList.Select(t => t.name).ToArray();
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

        foreach(ARTrackedImage trackedImg in args.updated) {
            foreach(TargetInfo target in targetList) {
                // 추적 목록에 포함된 이미지인지 체크
                if (trackedImg.referenceImage.name != target.name) continue;;

                if (trackedImg.trackingState == TrackingState.Tracking) {
                    // 화면에 이미지가 보이는 경우
                    if (target.createdObj == null) {
                        // 오브젝트가 아직 생성되지 않았으면 새로 생성
                        target.createdObj = Instantiate(target.targetPref);
                        target.createdObj.transform.parent = trackedImg.transform;
                        target.createdObj.SetActive(true);
                    }

                    if (target.trackingRoation || target.enableTracking) {
                        // 오브젝트 위치, 회전값 초기화
                        target.createdObj.transform.parent = trackedImg.transform;
                        if (target.enableTracking) {
                            target.createdObj.transform.localPosition = Vector3.zero;
                        }
                        if (target.trackingRoation) {
                            target.createdObj.transform.localRotation = Quaternion.identity;
                        }
                        target.createdObj.transform.parent = null;
                    }
                } else {
                    // 화면에서 이미지가 사라진 경우
                    if (target.createdObj != null) {
                        if (target.dontDestory) {
                            target.createdObj.SetActive(false);
                        } else {
                            // 만들어진 오브젝트가 있으면 파괴
                            Destroy(target.createdObj);
                            target.createdObj = null;
                        }
                    } 
                }
            }
        }
    }
}