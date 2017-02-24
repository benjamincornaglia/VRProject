using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDisp : MonoBehaviour {

	// Use this for initialization
	void Start () {
		TextMesh score = GetComponent<TextMesh>();
		score.text = "Score : " + ScoreManager._score.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
