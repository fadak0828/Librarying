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

    [Tooltip("오브젝트가 나타날 때 실행될 애니메이션 default: NONE")]
    public ArAnimationType appearAnimation;

    [Tooltip("오브젝트가 사라질 때 실행될 애니메이션 default: NONE")]
    public ArAnimationType disappearAnimation;

    [Tooltip("인식된 마커가 책 페이지인지?")]
    public PageOption pageOption = new PageOption(-1, false);

    [Tooltip("이미지를 따라서 위치 변경할 것인지?")]
    public bool enableTracking = true;

    [Tooltip("마커 인식이 끊기면 오브젝트를 바로 파괴할 것인지?")]
    public bool destroyImmediately = true;

    [Tooltip("마커 인식시 나타낼 게임오브젝트")]
    public GameObject targetPref;

    [Tooltip("targetPref을 복사하지 않고 그대로 사용할 것 인지?")]
    public bool noClone = false;

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
    public static float fixedTimeThreshold = 0.6f;

    public static MarkerManager Instance;
    public float smoothTime = 0.1f;

    public List<TargetInfo> targetList;
    public List<TargetInfo> targetPageInfos;
    public List<TargetInfo> currentTrackingTargets;

    public GameObject btnPageReplay;
    public XRReferenceImageLibrary trackingLibrary;
    private ARTrackedImageManager manager;

    [HideInInspector]
    public int currentPageIndex = 0;
    private Camera mainCam;

    private void Awake()
    {
        Instance = this;
        targetList
            .Concat(targetPageInfos)
            .ToList()
            .ForEach(t => {
                if (!t.noClone) t.createdObj = Instantiate(t.targetPref);
                else t.createdObj = t.targetPref;

                t.createdObj.SetActive(false);
            });
    }

    private void OnEnable()
    {
        manager = GetComponent<ARTrackedImageManager>();
        ResetTrackingLibrary();
        manager.trackedImagesChanged += OnTrackedImagesChaged;
        mainCam = Camera.main;
        RefreshCurretTrackingTargets();
    }

    private void OnDisable()
    {
        manager.trackedImagesChanged -= OnTrackedImagesChaged;
    }

    public void RefreshCurretTrackingTargets() {
        currentTrackingTargets = new List<TargetInfo>(targetList);
        targetPageInfos.ForEach(pi => currentTrackingTargets.Add(pi));
        // currentTrackingTargets.Add(targetPageInfos[currentPageIndex]);
    }

    private void OnTrackedImagesChaged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (ARTrackedImage trackedImg in args.updated)
        {
            foreach (TargetInfo target in currentTrackingTargets)
            {
                // 추적 목록에 포함된 이미지인지 체크
                if (trackedImg.referenceImage.name != target.name) continue;

                target.trackingState = trackedImg.trackingState;

                bool shouldPlayAnim = false;

                // 화면에 이미지가 보이는 경우
                if (trackedImg.trackingState == TrackingState.Tracking)
                {
                    // 페이지가 아닌 오브젝트 또는 고정된 페이지면
                    if (target.pageOption.pageNumber == -1 || target.isFixed) {

                        // 오브젝트가 나타나는 프레임이면 오브젝트 초기화
                        if (!target.createdObj.activeSelf) {
                            shouldPlayAnim = target.appearAnimation != ArAnimationType.NONE;
                            target.createdObj.transform.parent = trackedImg.transform;
                            target.createdObj.transform.localPosition = Vector3.zero;
                            target.createdObj.transform.localEulerAngles = Vector3.zero;
                            target.createdObj.transform.parent = null;
                            target.createdObj.SetActive(true);
                        }
                        
                        // appear/disappear 애니메이션 재생처리
                        if (shouldPlayAnim)
                        {
                            ArAnimation anim = target.createdObj.GetComponent<ArAnimation>();
                            if (anim == null) anim = target.createdObj.AddComponent<ArAnimation>();

                            anim.StartAnim(target.appearAnimation);
                        }
                    }

                    // 마커가 고정되지 않았으면
                    if (!target.isFixed) {
                        // 오브젝트가 나타나있는 상태면
                        if (target.createdObj != null) {
                            float moveDistance = Vector3.Distance(target.createdObj.transform.position, trackedImg.transform.position);

                            // 일정거리 이하에서는 오브젝트가 부드럽게 마커를 따라가게 해서 오브젝트가 떨리는 증상을 방지
                            if (moveDistance <= 0.1f) {
                                target.createdObj.transform.position = Vector3.SmoothDamp(
                                    target.createdObj.transform.position, 
                                    trackedImg.transform.position, 
                                    ref target.positionVelocity, 
                                    smoothTime
                                );
                            } else {
                                target.createdObj.transform.position = trackedImg.transform.position;
                            }

                            // 오브젝트 회전 부드럽게
                            target.createdObj.transform.rotation = QuaternionUtil.SmoothDamp(
                                target.createdObj.transform.rotation, 
                                trackedImg.transform.rotation, 
                                ref target.rotationVelocity, 
                                smoothTime
                            );
                        }

                        // 페이지 마커이거나 오브젝트가 마커를 따라다니지 않아도 되는 경우 처리
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
                                    if (OnboardingUIManager.Instance.GetCurrentClipName().Contains("FindNextPage")) {
                                        OnboardingUIManager.Instance.HideUiByName("FindNextPage");
                                    }
                                    ResetTrackingLibrary();

                                    targetPageInfos.ForEach(pi => pi.showInScreenTime = 0);

                                    foreach(var pageInfo in targetPageInfos.Where(p => p.createdObj != null).ToArray()) {
                                        if (pageInfo.pageOption.pageNumber == target.pageOption.pageNumber) continue;
                                        pageInfo.isFixed = false;
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

                    CleanImageTrackingObj(target);

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

        btnPageReplay.SetActive(targetPageInfos.FirstOrDefault(pi => pi.createdObj && pi.createdObj.activeSelf) != null);
    }

    // 현재 추적중인 페이지의 타겟 오브젝트를 파괴한다
    [ContextMenu("ResetCurrentPage")]
    public void ResetCurrentPage() {
        TargetInfo currentPage = targetPageInfos.FirstOrDefault(pi => pi.createdObj && pi.createdObj.activeSelf);
        if (currentPage == null) {
            Debug.LogError("열려있는 페이지가 없습니다");
        } else {
            GameObject newPageObj = Instantiate(currentPage.targetPref, currentPage.createdObj.transform.parent);
            GameObject prevObj = currentPage.createdObj;
            currentPage.createdObj = newPageObj;
            Destroy(prevObj);
        }
    }

    private void DestroyImageTrackingObj(TargetInfo target)
    {
        if (target.createdObj == null) return;

        if (target.disappearAnimation != ArAnimationType.NONE)
        {
            ArAnimation anim = target.createdObj.GetComponent<ArAnimation>();
            if (anim == null) 
                anim = target.createdObj.AddComponent<ArAnimation>();
                
            if (!anim.IsPlaying()) 
                anim.StartAnim(target.disappearAnimation, () => CleanImageTrackingObj(target));
        } else {
            CleanImageTrackingObj(target);
        }
    }

    private void CleanImageTrackingObj(TargetInfo target) {
        target.createdObj.SetActive(false);
    }

    private IEnumerator IeDestroyTimer(TargetInfo target, float time = 3)
    {
        yield return new WaitForSeconds(time);

        DestroyImageTrackingObj(target);
    }

    // 페이지 넘길 때 이전 페이지가 추적 중인것으로 나타나는 버그 때문에
    // referenceLibrary 초기화 필요
    public void ResetTrackingLibrary() {
        manager.enabled = false;
        manager.referenceLibrary = manager.CreateRuntimeLibrary(trackingLibrary);
        manager.enabled = true;

        targetPageInfos.ForEach(pi => pi.showInScreenTime = 0);
    }
}