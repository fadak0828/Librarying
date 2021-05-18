using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum OnboardingUIState {
    NONE = 0,
    ZOOM_QR,
    SHAKE_PHONE,
    FIND_PLANE,
    FIND_BOOK,
    FIND_NEXT_PAGE
}

public class OnboardingUI {
    public readonly static string ZOOM_QR = "ZoomQR";
    public readonly static string SHAKE_PHONE = "ShakePhone";
}

public class OnboardingUIManager : MonoBehaviour
{
    public static OnboardingUIManager Instance;
    private void Awake() {
        Instance = this;
    }

    public Image loadingMask;

    private float _loadingPercentage = 0;
    public float loadingPercentage {
        get { return _loadingPercentage; }
        set {
            if (value >= 0.95) {
                if (GetCurrentClipName() == "None") {
                    animator.Play("ExitLoading");
                }
            } 

            if (GetCurrentClipName() != "ExitLoading") {
                loadingMask.fillAmount = value;
            } else {
                loadingMask.fillAmount = 1;
            }
            _loadingPercentage = value;
        }
    }

    [SerializeField]
    private OnboardingUIState _state;
    public OnboardingUIState state {
        get { return _state; }
        set {
            if (_state == value) return;
            OnExitState(_state);
            _state = value;
            OnEnterState(_state);
        }
    }

    [SerializeField]
    private string TestUI;
    private Animator animator;

    private void OnEnable() {
        animator = GetComponent<Animator>();
        InitOnboardUI();
    }

    private void InitOnboardUI() {
        GetComponent<Image>().color = new Color(0, 0, 0, 0);
        HideAllOnboardingUI();
        transform.Find("Loading").gameObject.SetActive(true);
    }

    private void HideAllOnboardingUI() {
        for(int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void ShowUiByName(string name) {
        if (state != StateStringToStateEnum(name)) {
            state = StateStringToStateEnum(name);
            animator.Play($"Enter{name}");
        }
    }

    public void HideUiByName(string name) {
        animator.Play($"Exit{name}");
    }

    [ContextMenu("ShowTestUI")]
    private void ShowTestUI() {
        ShowUiByName(TestUI);
    }

    [ContextMenu("HideTestUI")]
    private void HideTestUI() {
        HideUiByName(TestUI);
    }

    private void Update() {
        OnState(state);
    }

    private void OnState(OnboardingUIState state) {
        // 현재 state에 있음
    }

    private void OnEnterState(OnboardingUIState state) {
        // state 들어옴
        ShowUiByName(StateEnumToStateString(state));

        switch (state) {
            case OnboardingUIState.FIND_BOOK:

            break;

            case OnboardingUIState.SHAKE_PHONE:

            break;
        }
    }

    private void OnExitState(OnboardingUIState state) {
        // state 나감
        // switch(state) {
        // }
    }

    private OnboardingUIState StateStringToStateEnum(string state) {
        switch (state)
        {
            case "FindBook": return OnboardingUIState.FIND_BOOK;
            case "ShakePhone": return OnboardingUIState.SHAKE_PHONE;
            case "FindPlane": return OnboardingUIState.FIND_PLANE;
            case "FindNextPage": return OnboardingUIState.FIND_NEXT_PAGE;
            case "ZoomQR": return OnboardingUIState.ZOOM_QR;
            default: return OnboardingUIState.NONE;
        }
    }

    private string StateEnumToStateString(OnboardingUIState state) {
        switch(state) {
            case OnboardingUIState.FIND_BOOK: return "FindBook";
            case OnboardingUIState.SHAKE_PHONE: return "ShakePhone";
            case OnboardingUIState.FIND_PLANE: return "FindPlane";
            case OnboardingUIState.FIND_NEXT_PAGE: return "FindNextPage";
            case OnboardingUIState.ZOOM_QR: return "ZoomQR";
            default: return "None";
        }
    }

    private AnimatorClipInfo[] clipInfos;
    public string GetCurrentClipName(){
        clipInfos = animator.GetCurrentAnimatorClipInfo(0);
        if (clipInfos == null || clipInfos.Length == 0) {
            return "None";
        } else {
            return clipInfos[0].clip.name;
        }
    }
}
