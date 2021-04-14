using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Constants;

public class Dino : MonoBehaviour
{
    public GameObject holeTop;
    public Material originMat;
    public Material stencilMat;
    public float escapeHeight = 0.007f;
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
