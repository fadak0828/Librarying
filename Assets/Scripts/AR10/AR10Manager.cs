using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AR10Manager : MonoBehaviour
{
    public List<AR10AnimalAnimationController> animals;
    public float radius = 0.05f;

    void Start()
    {
        animals.ForEach(animal => {
            Vector2 pos = Random.insideUnitCircle.normalized * radius;
            animal.transform.localPosition = new Vector3(pos.x, 0, pos.y);
            animal.transform.LookAt(transform);
            animal.PlayAnim();
        });
    }

    public void OnFinishClean() {
        print("OnFinishClean");
        animals.ForEach(animal => {
            Vector2 pos = Random.insideUnitCircle.normalized * 0.5f;
            animal.transform.localPosition = new Vector3(pos.x, 0, pos.y);
            animal.transform.LookAt(transform);
            animal.PlayCleanFinish();
        });
    }
}
