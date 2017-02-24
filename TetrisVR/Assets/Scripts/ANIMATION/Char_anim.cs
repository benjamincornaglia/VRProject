using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Char_anim : MonoBehaviour 
{
	public bool _charIsMoving = true;
	bool _doneCheckMoveOnce;

	// Use this for initialization
	void Start () 
	{
		if (_charIsMoving) 
		{
			Jumpy ();
		} 
		else 
		{
			Bigger ();
		}
	}

	void Update()
	{
		if (!_doneCheckMoveOnce && _charIsMoving) 
		{
			CheckJumpy ();
			_doneCheckMoveOnce = true;
		}
		else if (_doneCheckMoveOnce && !_charIsMoving) 
		{
			Bigger ();
			_doneCheckMoveOnce = false;
		}
	}
	
	void Normal()
	{
		this.transform.DOScale (new Vector3 (1.05f, 1, 1), 0.05f).SetEase (Ease.OutFlash).OnComplete (Bigger);
		JumpyOnce ();
	}

	void Bigger()
	{
		this.transform.DOScale (new Vector3 (1, 0.95f, 1), 0.05f).SetEase (Ease.OutFlash).OnComplete(Normal).SetDelay(UnityEngine.Random.Range(0.0f,2f));
	}

	void JumpyOnce()
	{
		this.transform.GetChild (0).transform.DOLocalJump (Vector3.zero, 0.5f, 1, 0.25f, false).SetEase (Ease.InSine);
	}

	void Jumpy()
	{
		this.transform.DOScale (new Vector3 (1f, 0.95f, 1), 0.25f).SetEase (Ease.OutFlash);
		this.transform.DOScale (new Vector3 (1.05f, 1f, 1), 0.25f).SetEase (Ease.OutFlash).SetDelay(0.26f);
		this.transform.GetChild (0).transform.DOLocalJump (Vector3.zero, 1f, 1, 0.25f, false).SetEase (Ease.InSine).OnComplete(CheckJumpy)/*.SetDelay(0.51f)*/;
	}

	void CheckJumpy()
	{
		if (_charIsMoving) 
		{
			Jumpy ();
		}
	}
}
