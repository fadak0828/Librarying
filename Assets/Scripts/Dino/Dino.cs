using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    }
    
    void Update()
    {
        UpdateMaterial();
    }

    private void UpdateMaterial() {
        if (transform.localPosition.y >= escapeHeight) {
            if (_renderer.material != originMat) {
                _renderer.material = originMat;
            }
        } else {
            if (_renderer.material != stencilMat) {
                _renderer.material = stencilMat;
            }
        }
    }
}
