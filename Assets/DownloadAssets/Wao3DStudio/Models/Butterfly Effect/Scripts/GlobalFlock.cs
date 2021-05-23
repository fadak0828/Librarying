using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFlock : MonoBehaviour {
	public GameObject butterflyPrefab;
	public float areaSize = 3;
	public float startPosMultiply = 10;

	public int numButterflies = 15;
	public GameObject[] allButterfly;
	//public GameObject goalPosGameobject;
	public Vector3 goalPos = Vector3.zero;


	// Use this for initialization
	void Start () 

	{
		allButterfly = new GameObject[numButterflies];
		goalPos = gameObject.transform.position;

		for (int i = 0; i< numButterflies; i++)
		{
			Vector3 pos = new Vector3 (Random.Range(-areaSize, areaSize),
			Random.Range(-areaSize,areaSize),
			Random.Range(-areaSize,areaSize)) * startPosMultiply;
			allButterfly[i] = (GameObject) Instantiate(butterflyPrefab);
			allButterfly[i].transform.parent = gameObject.transform;
			allButterfly[i].transform.localPosition = pos;
			//allButterfly[i].GetComponent<Flock>().startUpdate=true;
		}	
	}
	
	// Update is called once per frame
	void Update () {
		if (Random.Range (0, 10000) < 50) 
		{
			goalPos = gameObject.transform.position + new Vector3 (Random.Range(-areaSize, areaSize),
				Random.Range(-areaSize,areaSize),
				Random.Range(-areaSize,areaSize)); 
		}
		
	}
}
