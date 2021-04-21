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

    [Tooltip("페이지 이미지인가? 다른 페이지가 인식되면 꺼진다")]
    public bool isPageImage;

    [Tooltip("이미지를 따라서 위치 변경할 것인지?")]
    public bool enableTracking = true;

    [Tooltip("이미지가 안보이면 파괴할 것인지? 체크시 Destroy 대신 SetActive true/false로 조절")]
    public bool dontDestory = false;

    public GameObject targetPref;

    [HideInInspector]
    public GameObject createdObj;

    [HideInInspector]
    public bool isSceneObject;
}

public class MarkerManager : MonoBehaviour
{
    public static MarkerManager Instance;
    public List<TargetInfo> targetList;
    private ARTrackedImageManager manager;

    private TargetInfo _pageObject;
    public TargetInfo pageObject {
        get => _pageObject;
        set {
            if (pageObject == value) return;

            if (_pageObject != null) {
                if (_pageObject.dontDestory) {
                    _pageObject.createdObj.SetActive(false);
                } else {
                    Destroy(_pageObject.createdObj);
                    _pageObject.createdObj = null;
                }
            }

            _pageObject = value;
        }
    }

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

        foreach(ARTrackedImage trackedImg in args.updated) {
            foreach(TargetInfo target in targetList) {
                // 추적 목록에 포함된 이미지인지 체크
                if (trackedImg.referenceImage.name != target.name) continue;;

                if (trackedImg.trackingState == TrackingState.Tracking) {
                    // 화면에 이미지가 보이는 경우
                    if (target.createdObj == null) {
                        if (target.targetPref.gameObject.scene.name == null) {
                            // 프리팹을 사용하는 경우
                            // 오브젝트가 아직 생성되지 않았으면 새로 생성
                            target.createdObj = Instantiate(target.targetPref);
                        } else {
                            // 씬 오브젝트를 사용하는 경우
                            // target 정보에 연결
                            target.createdObj = target.targetPref;
                            target.isSceneObject = true;
                        }
                        target.createdObj.transform.parent = trackedImg.transform;

                        target.createdObj.transform.localPosition = Vector3.zero;
                        target.createdObj.transform.localRotation = Quaternion.identity;

                        if (target.isPageImage) {
                            pageObject = target;
                        }
                    }

                    target.createdObj.SetActive(true);
                    
                    if (!target.enableTracking) {
                        target.createdObj.transform.parent = null;
                    }

                    // if (target.trackingRoation || target.enableTracking) {
                    //     // 오브젝트 위치, 회전값 초기화
                    //     if (target.enableTracking) {
                    //         target.createdObj.transform.localPosition = Vector3.zero;
                    //     }
                    //     if (target.trackingRoation) {
                    //         target.createdObj.transform.localRotation = Quaternion.identity;
                    //     }
                    // }
                } else {
                    // 화면에서 이미지가 사라진 경우 
                    if (target.isPageImage) continue;

                    if (target.createdObj != null) {
                        // dontDestory가 체크되어 있거나, 프리팹이 아닌 씬 오브젝트를 사용한 경우 파괴하지 않고 비활성화
                        if (target.dontDestory || target.isSceneObject) {
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