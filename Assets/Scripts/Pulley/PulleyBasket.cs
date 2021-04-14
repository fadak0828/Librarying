using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PulleyBasket : MonoBehaviour
{
    public int mass;
    
    public float destinationY;
    public Vector3 massCheckBounds = new Vector3(0.056f, 0.038f, 0.056f);
    public Vector3 offset = new Vector3(0, 0.005f, 0);

    [HideInInspector]
    public float speed;
    private Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        destinationY = transform.position.y;
    }

    public void FixedUpdate() {
        Vector3 cur = transform.position;
        Vector3 dest = transform.position;
        dest.y = destinationY;

        Vector3 dir = dest - cur;

        rb.MovePosition(cur + (dir * Time.deltaTime * speed));

        PulleyMass[] massArr = Physics.OverlapBox(transform.position + offset, massCheckBounds, Quaternion.identity)
                                    .Select(c => c.GetComponent<PulleyMass>())
                                    .Where(pulleyMass => pulleyMass != null)
                                    .ToArray();
        mass = massArr.Select(pulleyMass => pulleyMass.mass).Sum();
    }

    void OnDrawGizmosSelected()
    {
        // 무게 감지 범위 씬뷰에 그리기
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + offset, massCheckBounds);
    }
}
