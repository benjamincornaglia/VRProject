using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Turn : MonoBehaviour {

	bool m_bTurn = false;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown (KeyCode.Space)) {
			m_bTurn = true;
			transform.DORotate (new Vector3 (0, 0, 0), 1f, RotateMode.Fast);
		}
		if(!m_bTurn)
			transform.position -= transform.forward * Time.deltaTime  * 30;

		if(Input.GetKeyDown(KeyCode.Return))
			Application.LoadLevel(Application.loadedLevel);
	}
}
