using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using DodleUtils;

[System.Serializable]
public struct TrackedTarget
{
    public string name;
    public TrackingState state;

    public TrackedTarget(string name, TrackingState state) {
        this.name = name;
        this.state = state;
    }
}

[System.Serializable]
public struct PageOption
{
    public int pageNumber;
    public bool isStatic;

    public PageOption(int pageNumber, bool isStatic)
    {
        this.pageNumber = pageNumber;
        this.isStatic = isStatic;
    }
}

[System.Serializable]
public class TargetInfo
{
    [Tooltip("ImageLibrary에 등록된 이미지 이름")]
    public string name;
    public ArAnimationType appearAnimation;
    public ArAnimationType disappearAnimation;

    public PageOption pageOption = new PageOption(-1, false);

    [Tooltip("이미지를 따라서 위치 변경할 것인지?")]
    public bool enableTracking = true;

    public bool destroyImmediately = true;

    public GameObject targetPref;

    [HideInInspector]
    public GameObject createdObj;

    [HideInInspector]
    public bool isSceneObject;

    [HideInInspector]
    public Vector3 positionVelocity;

    [HideInInspector]
    public Vector3 rotationVelocity;

    [HideInInspector]
    public Coroutine destroyCoroutine;

    public float showInScreenTime;
    public bool isFixed;
    public TrackingState trackingState = TrackingState.None;
}

public class MarkerManager : MonoBehaviour
{
    public static float fixedTimeThreshold = 0.3f;

    public static MarkerManager Instance;
    public List<TargetInfo> targetList;
    public float smoothTime = 0.1f;
    public List<TargetInfo> targetPageInfos;
    private ARTrackedImageManager manager;

    private int currentPageIndex = -1;
    private Camera mainCam;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        manager = GetComponent<ARTrackedImageManager>();
        manager.trackedImagesChanged += OnTrackedImagesChaged;
        mainCam = Camera.main;
        targetPageInfos = targetList.Where(t => t.pageOption.pageNumber >= 0).ToList();
    }

    private void OnDisable()
    {
        manager.trackedImagesChanged -= OnTrackedImagesChaged;
    }

    private void OnTrackedImagesChaged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (ARTrackedImage trackedImg in args.updated)
        {
            foreach (TargetInfo target in targetList)
            {
                // 추적 목록에 포함된 이미지인지 체크
                if (trackedImg.referenceImage.name != target.name) continue;

                target.trackingState = trackedImg.trackingState;

                bool shouldPlayAnim = false;

                if (trackedImg.trackingState == TrackingState.Tracking)
                {
                    if (target.pageOption.pageNumber == -1 || target.isFixed) {
                        // 화면에 이미지가 보이는 경우
                        if (target.createdObj == null)
                        {
                            if (target.targetPref?.gameObject?.scene.name == null)
                            {
                                // 프리팹을 사용하는 경우
                                // 오브젝트가 아직 생성되지 않았으면 새로 생성
                                target.createdObj = Instantiate(target.targetPref);
                            }
                            else
                            {
                                // 씬 오브젝트를 사용하는 경우
                                // target 정보에 연결
                                target.createdObj = target.targetPref;
                                target.isSceneObject = true;
                            }

                            target.createdObj.transform.parent = trackedImg.transform;
                            target.createdObj.transform.localPosition = Vector3.zero;
                            target.createdObj.transform.localEulerAngles = Vector3.zero;
                            target.createdObj.transform.parent = null;

                            shouldPlayAnim = target.appearAnimation != ArAnimationType.NONE;
                        }

                        if (!target.createdObj.activeSelf) {
                            shouldPlayAnim = target.appearAnimation != ArAnimationType.NONE;
                            target.createdObj.SetActive(true);
                        }
                        
                        if (shouldPlayAnim)
                        {
                            ArAnimation anim = target.createdObj.GetComponent<ArAnimation>();
                            if (anim == null) anim = target.createdObj.AddComponent<ArAnimation>();

                            anim.StartAnim(target.appearAnimation);
                        }
                    }

                    if (!target.isFixed) {
                        if (target.createdObj != null) {
                            float moveDistance = Vector3.Distance(target.createdObj.transform.position, trackedImg.transform.position);

                            if (moveDistance <= 0.1f) {
                                target.createdObj.transform.position = Vector3.SmoothDamp(target.createdObj.transform.position, trackedImg.transform.position, ref target.positionVelocity, smoothTime);
                            } else {
                                target.createdObj.transform.position = trackedImg.transform.position;
                            }

                            target.createdObj.transform.rotation = QuaternionUtil.SmoothDamp(target.createdObj.transform.rotation, trackedImg.transform.rotation, ref target.rotationVelocity, smoothTime);
                        }

                        if (!target.enableTracking || target.pageOption.pageNumber >= 0) {
                            Vector3 viewportPoint = mainCam.WorldToViewportPoint(trackedImg.transform.position);
                            if (viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1) {
                                target.showInScreenTime += Time.deltaTime;
                            } else {
                                target.showInScreenTime = 0;
                            }

                            if (!target.isFixed && target.showInScreenTime > fixedTimeThreshold) {
                                target.isFixed = true;
                                // 고정되는 타겟이 페이지이면 다른 페이지들 오브젝트 감춤                                
                                if (target.pageOption.pageNumber >= 0) {
                                    target.showInScreenTime = 0;
                                    foreach(var pageInfo in targetPageInfos.Where(p => p.createdObj != null).ToArray()) {
                                        if (pageInfo.pageOption.pageNumber == target.pageOption.pageNumber) continue;
                                        pageInfo.isFixed = false;
                                        pageInfo.showInScreenTime = 0;
                                        CleanImageTrackingObj(pageInfo);
                                    }
                                }
                            }
                        }
                    }

                    if (target.destroyCoroutine != null)
                    {
                        StopCoroutine(target.destroyCoroutine);
                        target.destroyCoroutine = null;
                    }
                    
                    if (target.pageOption.pageNumber > -1)
                    {
                        currentPageIndex = target.pageOption.pageNumber;
                    }
                }
                // 화면에서 이미지가 사라진 경우 
                else
                {
                    target.showInScreenTime = 0;
                    if (target.pageOption.isStatic) {
                        if (target.createdObj != null) {
                            target.createdObj.transform.parent = null;
                        }
                        continue;
                    }
                    
                    target.isFixed = false;

                    if (target.createdObj != null) {
                        CleanImageTrackingObj(target);
                    }

                    if (target.destroyImmediately)
                    {
                        DestroyImageTrackingObj(target);
                    }
                    else if (target.destroyCoroutine == null)
                    {
                        target.destroyCoroutine = StartCoroutine(IeDestroyTimer(target));
                    }
                }
            }
        }
    }

    private void DestroyImageTrackingObj(TargetInfo target)
    {
        if (target.createdObj != null)
        {
            if (target.disappearAnimation != ArAnimationType.NONE)
            {
                ArAnimation anim = target.createdObj.GetComponent<ArAnimation>();
                if (anim == null) anim = target.createdObj.AddComponent<ArAnimation>();
                
                if (anim.IsPlaying()) return;

                anim.StartAnim(target.disappearAnimation, () => CleanImageTrackingObj(target));
            } else {
                CleanImageTrackingObj(target);
            }
        }
    }

    private void CleanImageTrackingObj(TargetInfo target) {
        // dontDestory가 체크되어 있거나, 프리팹이 아닌 씬 오브젝트를 사용한 경우 파괴하지 않고 비활성화
        if (target.isSceneObject)
        {
            target.createdObj.SetActive(false);
        }
        else
        {
            // 만들어진 오브젝트가 있으면 파괴
            Destroy(target.createdObj);
            target.createdObj = null;
        }
    }

    private IEnumerator IeDestroyTimer(TargetInfo target, float time = 3)
    {
        yield return new WaitForSeconds(time);

        DestroyImageTrackingObj(target);
    }
}