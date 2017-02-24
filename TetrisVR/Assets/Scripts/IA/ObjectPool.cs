using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool {

	private List<GameObject> objects = new List<GameObject>();
	private int index = 0;

	public ObjectPool(GameObject prefab, int poolSize) {
		for (int i=0; i<poolSize; i++) {
			var proj = GameObject.Instantiate (prefab);
			proj.SetActive (false);
			addObject(proj);
		}
	}

	public ObjectPool(GameObject prefab, int poolSize, GameObject target) {
		for (int i=0; i<poolSize; i++) {
			var proj = GameObject.Instantiate (prefab);
			proj.GetComponent<ArmedIA> ().Target = target;
			proj.SetActive (false);
			addObject(proj);
		}
	}

	public ObjectPool(GameObject prefab, int poolSize, GameObject target, List<Vector3> path) {
		for (int i=0; i<poolSize; i++) {
			var proj = GameObject.Instantiate (prefab);
			proj.GetComponent<CivilianIA> ().Target = target;
			proj.GetComponent<CivilianIA> ().path = path;
			proj.SetActive (false);
			addObject(proj);
		}
	}

	public void addObject(GameObject obj) {
		objects.Add(obj);
	}

	public GameObject getNext() {
		if (objects.Count == 0) {
			throw new UnityException("Object pool has to be filled before calling getNext().");
		}

		GameObject obj = objects[index];
		index = (index + 1) % objects.Count;
		return obj;
	}

	public int size() {
		return objects.Count;
	}

}
