using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AR10Manager : MonoBehaviour
{
    public List<AR10AnimalAnimationController> animals;
    public float radius = 0.05f;
    public float animalGap = 0.005f;

    void Start()
    {
        SetAnimalRandomPositions();
        animals.ForEach(animal => animal.PlayAnim());
    }

    public void OnFinishClean() {
        SetAnimalRandomPositions();
        animals.ForEach(animal => animal.PlayCleanFinish());
    }

    private List<Vector2> SetAnimalRandomPositions() {
        List<Vector2> positions = new List<Vector2>();

        bool isUniquePos = false;
        Vector2 pos;

        for(int i = 0; i < animals.Count; i++) {
            while (!isUniquePos) {
                pos = Random.insideUnitCircle.normalized * radius;
                isUniquePos = positions.Count == 0 || positions.Select(p => Vector2.Distance(p, pos)).All(gap => gap >= animalGap);
                if (isUniquePos) {
                    positions.Add(pos);
                    animals[i].transform.localPosition = new Vector3(pos.x, 0, pos.y);
                    animals[i].transform.LookAt(transform);
                }
            }
            isUniquePos = false;
        }

        return positions;
    }
}
