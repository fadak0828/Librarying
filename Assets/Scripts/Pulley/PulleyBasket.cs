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
    public Collider m_Collider;
    private Vector3 colliderSize;

    [HideInInspector]
    public float speed;
    private Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        destinationY = transform.position.y;
        colliderSize = m_Collider.bounds.size;
    }

    public void FixedUpdate() {
        Vector3 cur = transform.position;
        Vector3 dest = transform.position;
        dest.y = destinationY;

        Vector3 dir = dest - cur;

        rb.MovePosition(cur + (dir * Time.deltaTime * speed));

        mass = Physics.OverlapBox(m_Collider.transform.position, colliderSize / 2, transform.rotation)
                        .Select(obj => obj.GetComponent<PulleyMass>())
                        .Where(pm => pm != null)
                        .Select(pm => pm.mass)
                        .Sum();
    }

    // private void OnTriggerEnter(Collider other) {
    //     PulleyMass pulleyMass = other.gameObject.GetComponent<PulleyMass>();
    //     if (pulleyMass) {
    //         mass += pulleyMass.mass;
    //     }    
    // }

    // private void OnTriggerExit(Collider other) {
    //     PulleyMass pulleyMass = other.gameObject.GetComponent<PulleyMass>();
    //     if (pulleyMass) {
    //         mass -= pulleyMass.mass;
    //     }   
    // }
}
