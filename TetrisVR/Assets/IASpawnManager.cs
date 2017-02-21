using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IASpawnManager : MonoBehaviour {

	public GameObject Target;

	List<GameObject> spawners = new List<GameObject>();
	Camera camera;

	// Use this for initialization
	void Start () {
		foreach(Transform child in this.transform) {
			spawners.Add(child.gameObject);
		}

		camera = Camera.main;
	}
	
	void spawnHelico() {
		Vector3 spawnPosition =
			Target.transform.position - (camera.transform.forward * 500) + new Vector3(0, 200, 0);


	}

	void spawnTank() {

	}

	void spawnMilitary() {

	}

	// Update is called once per frame
	void Update () {
		
	}
}
