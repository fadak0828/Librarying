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
    public List<TargetInfo> targetList;
    public float smoothTime = 0.1f;
    public List<TargetInfo> targetPageInfos;
    public GameObject btnPageReplay;
    public XRReferenceImageLibrary trackingLibrary;
    private ARTrackedImageManager manager;

    [HideInInspector]
    public int currentPageIndex = 0;
    private Camera mainCam;
    public List<TargetInfo> currentTrackingTargets;

    private void Awake()
    {
        Instance = this;
        targetList.ForEach(t => {
            if (!t.noClone) {
                t.createdObj = Instantiate(t.targetPref);
            } else {
                t.createdObj = t.targetPref;
            }
            t.createdObj.SetActive(false);
        });
        targetPageInfos.ForEach(t => {
            if (!t.noClone) {
                t.createdObj = Instantiate(t.targetPref);
            } else {
                t.createdObj = t.targetPref;
            }
            t.createdObj.SetActive(false);
        });
    }

    private void OnEnable()
    {
        manager = GetComponent<ARTrackedImageManager>();
        ResetTrackingLibrary();
        manager.trackedImagesChanged += OnTrackedImagesChaged;
        mainCam = Camera.main;
        // targetPageInfos = targetList.Where(t => t.pageOption.pageNumber >= 0).ToList();
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

                        if (!target.createdObj.activeSelf) {
                            shouldPlayAnim = target.appearAnimation != ArAnimationType.NONE;
                            target.createdObj.transform.parent = trackedImg.transform;
                            target.createdObj.transform.localPosition = Vector3.zero;
                            target.createdObj.transform.localEulerAngles = Vector3.zero;
                            target.createdObj.transform.parent = null;
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
                                target.createdObj.transform.position = Vector3.SmoothDamp(
                                    target.createdObj.transform.position, 
                                    trackedImg.transform.position, 
                                    ref target.positionVelocity, 
                                    smoothTime
                                );
                            } else {
                                target.createdObj.transform.position = trackedImg.transform.position;
                            }

                            target.createdObj.transform.rotation = QuaternionUtil.SmoothDamp(
                                target.createdObj.transform.rotation, 
                                trackedImg.transform.rotation, 
                                ref target.rotationVelocity, 
                                smoothTime
                            );
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
                                    ResetTrackingLibrary();
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
        target.createdObj.SetActive(false);
    }

    private IEnumerator IeDestroyTimer(TargetInfo target, float time = 3)
    {
        yield return new WaitForSeconds(time);

        DestroyImageTrackingObj(target);
    }

    public void ResetTrackingLibrary() {
        manager.enabled = false;
        manager.referenceLibrary = manager.CreateRuntimeLibrary(trackingLibrary);
        manager.enabled = true;

        targetPageInfos.ForEach(pi => pi.showInScreenTime = 0);
    }
}