using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour {

	public ScoreManager scoreManager = null;
	private bool _collisioned = false;
    float m_fColTimer = 0f;

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Piece" &&_collisioned == false && Manipulation.m_bHasObject == false) 
		{
			_collisioned = true;
			//scoreManager.throwhappen (col.contacts[0].point);
		}
        else if(col.gameObject.tag == "Piece" && _collisioned == false && Manipulation.m_bHasObject == true)
        {
            _collisioned = true;
            //scoreManager.swipeHappen(col.contacts[0].point);
        }
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (scoreManager == null) {
			throw new System.ArgumentException ("SwipCounter : Merci de renseigner le score manager !");
		}

        if (_collisioned)
            m_fColTimer += Time.deltaTime;
        if(m_fColTimer > 1)
        {
            _collisioned = false;
            m_fColTimer = 0f;
        }

	}
}
