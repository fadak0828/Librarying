using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public enum ArSceneState {
    IDLE = 99,
    FIND_PLACE = 0,
    FIND_BOOK = 1,
    FIND_NEXT_PAGE = 2,
}

public class ArSceneManager : MonoBehaviour
{
    public static ArSceneManager Instance;

    public MarkerManager markerManager;
    public MainSceneUiManager uiManager;
    public OnboardingUIManager onboadringUIManager;

    [SerializeField]
    private float currentStateTimer = 0;

    private void Awake() {
        Instance = this;
    }

    [SerializeField]
    private ArSceneState _state = ArSceneState.FIND_PLACE;
    public ArSceneState state {
        get { return _state; }
        set {
            if (_state == value) return;
            OnExitState(_state);
            _state = value;
            OnEnterState(_state);
        }
    }

    public void SetState(ArSceneState state) {
        this.state = state;
    }

    public void SetState(int state) {
        this.state = (ArSceneState)state;
    }

    private void Update() {
        if (markerManager.targetPageInfos?.Count > 0) {
            onboadringUIManager.loadingPercentage = markerManager.targetPageInfos.Max(p => p.showInScreenTime) / MarkerManager.fixedTimeThreshold;
        }
        OnState(state);
    }

    private void OnState(ArSceneState currentState) {
        currentStateTimer += Time.deltaTime;

        // 현재 state에 있음
        switch(currentState) {
            case ArSceneState.FIND_BOOK:
                int trackedCount = markerManager.targetList.Count(t => t.createdObj != null && t.createdObj.activeSelf);
                if (currentStateTimer >= 3) {
                    if (onboadringUIManager.GetCurrentClipName() == "None" && trackedCount == 0) {
                        onboadringUIManager.ShowUiByName("FindBook");
                    }
                }

                if (trackedCount > 0) {
                    if (onboadringUIManager.GetCurrentClipName() == "OnFindBook") {
                        onboadringUIManager.HideUiByName("FindBook");
                    }
                    state = ArSceneState.IDLE;
                }
            break;
        }
    }

    private void OnEnterState(ArSceneState state) {
        // state 들어옴
        // switch(state) {
        // }
        currentStateTimer = 0;
    }

    private void OnExitState(ArSceneState state) {
        // state 나감
        // switch(state) {
        // }
    }
}
