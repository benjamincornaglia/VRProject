using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool {

	private List<GameObject> objects = new List<GameObject>();
	private int index = 0;

	public void AddObject(GameObject obj) {
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

}
