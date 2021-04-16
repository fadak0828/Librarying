using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MazeCard : MonoBehaviour
{
    public MazeCard upsideCard;
    public MazeCard leftsideCard;
    public MazeCard rightsideCard;
    public MazeCard downsideCard;

    private Bounds bounds;

    private void OnEnable() {
        bounds = GetBounds();
    }

    void Update()
    {
        
    }

    // 옆에 붙어있는 카드들 체크
    private void CheckSideCards() {
        MazeCard[] cards =  Physics.OverlapBox(transform.position, bounds.size * 1.5f, transform.rotation, LayerMask.GetMask("MazeCard"))
                                    .Select(col => col.GetComponent<MazeCard>())
                                    .ToArray();
    }

    private Bounds GetBounds()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0) return new Bounds();

        Bounds bounds = renderers[0].bounds;
        for(int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }
        return bounds;
    }
}
