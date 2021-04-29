using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public enum StencilCompMode {
    DISABLED,
    NEVER,
    LESS,
    EQUAL,
    LESS_EQUAL,
    GREATER,
    NOT_EQUAL,
    GREATER_EQUAL,
    ALWAYS
}

public class StencilHelper : MonoBehaviour
{
    private Renderer _renderer;
    private void Awake() {
        _renderer = GetComponent<Renderer>();
        _renderer.material = new Material(_renderer.material);
    }
    
    void Update()
    {
        Vector3 center = _renderer.bounds.center;
        float radius = _renderer.bounds.extents.magnitude;
        
        bool isInHole = Physics.OverlapBox(center, Vector3.one * radius, transform.rotation, LayerMask.GetMask(Tag.HOLE_BOUNDS)).Length > 0;

        if (isInHole) {
            _renderer.material.SetInt("_StencilComp", 3);
        } else {
            _renderer.material.SetInt("_StencilComp", 0);
        }
    }
}
