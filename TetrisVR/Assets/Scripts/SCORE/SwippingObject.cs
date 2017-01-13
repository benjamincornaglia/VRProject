using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwippingObject : MonoBehaviour {

	public ScoreManager _themanager = null;
	public string ObjectsTag = "wall";

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == ObjectsTag)
		{
			_themanager.swipeHappen ();
		}
	}

	void Start() {
		if (_themanager == null) {
			throw new System.ArgumentException ("SwipCounter : Merci de renseigner le score manager !");
		}

		if (ObjectsTag == null) {
			throw new System.ArgumentException ("SwipCounter : Merci de renseigner le tag des objets destructibles !");
		}
	}
}
