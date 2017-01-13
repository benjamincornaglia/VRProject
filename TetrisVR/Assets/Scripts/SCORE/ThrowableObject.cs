using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour {

	public ScoreManager scoreManager = null;
	private bool _collisioned = false;

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Piece" &&_collisioned == false) 
		{
			_collisioned = true;
			scoreManager.throwhappen ();
		}
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (scoreManager == null) {
			throw new System.ArgumentException ("SwipCounter : Merci de renseigner le score manager !");
		}
	}
}
