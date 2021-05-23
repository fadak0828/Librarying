using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingAR : MonoBehaviour
{
    public List<GameObject> people;
    public List<GameObject> animals;
    public Transform[] peoplePosition;
    public Transform[] animalsPosition;
    void Start()
    {
        for (int i = 0; i < people.Count; i++)
        {
            people[i].SetActive(false);
        }
        for (int i = 0; i < animals.Count; i++)
        {
            animals[i].SetActive(false);
        }

        for (int i = 0; i < 4; i++)
        {
            int r = Random.Range(0, people.Count);
            int rr = Random.Range(0, animals.Count);

            people[r].SetActive(true);
            people[r].transform.position = peoplePosition[i].position;
            people[r].transform.rotation = peoplePosition[i].rotation;
            people.RemoveAt(r);

            animals[r].SetActive(true);
            animals[r].transform.position = animalsPosition[i].position;
            animals[r].transform.rotation = animalsPosition[i].rotation;
            animals.RemoveAt(rr);
        }
    }
}
