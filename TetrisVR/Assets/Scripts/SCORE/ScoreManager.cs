using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

	public int _score,_bonuscombo,_swipecounter;
	public float _bonustime;

	private void increaseScore(int val) {
		_score += val * _bonuscombo;
	}

	public void swipeHappen() {
		_swipecounter++;
		if (_bonustime > 0) 
		{
			_bonustime += 2;
		}

		if (_swipecounter < 5) {
			increaseScore(100);
		} 

		else if (_swipecounter < 8) 
		{
			increaseScore(80);
		}

		else if (_swipecounter < 11) 
		{
			increaseScore(50);
		}

		else if (_swipecounter < 13) 
		{
			increaseScore(20);
		}

		else if (_swipecounter > 13) 
		{
			increaseScore(5);
		}
	}

	public void throwhappen()
	{
		_bonuscombo += 2;
		if (_bonustime == 0) {
			_bonustime += 5;
		} 
			
	}

	// Use this for initialization
	void Start () {

		_score = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (_bonustime != 0) 
		{
			_bonustime = _bonustime - Time.deltaTime;
		}

		if (_bonustime < 0) 
		{
			_bonustime = 0;
		}
		
	}
}
