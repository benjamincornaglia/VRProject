using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbleLife : MonoBehaviour {

	float m_fTimer = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		m_fTimer += Time.deltaTime;

		if (m_fTimer > 60 && GetComponent<Destruction> ().m_bGrabbed == false)
			Destroy (this.gameObject);
	}
}
