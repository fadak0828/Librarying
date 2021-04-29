using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleCover : MonoBehaviour
{
    [SerializeField]
    private StencilCompMode _stencilMode = StencilCompMode.DISABLED;
    public StencilCompMode stencilMode {
        get { return _stencilMode; }
        set {
            _stencilMode = value;
            ApplyStencilMode();
        }
    }
    private void Start() {
        ApplyStencilMode();
    }

    [ContextMenu("ApplyStencilMode")]
    public void ApplyStencilMode() {
        GetComponent<Renderer>().material.SetInt("_StencilComp", (int)stencilMode);
    }
}
