﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InitializeScoreTxt : MonoBehaviour {

    float m_fLifeTime = 0;
    GameObject m_pMyController;

	// Use this for initialization
	void Start () {

        m_pMyController = GameObject.FindGameObjectWithTag("Player");
        transform.DOScale(new Vector3(-1.5f, 1.5f, -1.5f), 0.5f).SetEase(Ease.OutBack);
        transform.DOMove(transform.position + transform.up,1f);

	}
	
	// Update is called once per frame
	void Update () {
        m_fLifeTime += Time.deltaTime;
        if (m_fLifeTime > 1)
            Destroy(this.gameObject);

        transform.LookAt(m_pMyController.transform);

	}

    public void SetTxt(int _iValue, string _sTxt)
    {
        GetComponent<TextMesh>().text = _sTxt;

        if(_iValue > 99)
        {
            GetComponent<TextMesh>().color = Color.yellow;
        }
        else if(_iValue > 50)
        {
            GetComponent<TextMesh>().color = Color.blue;
        }
        else if(_iValue > 19)
        {
            GetComponent<TextMesh>().color = Color.green;
        }
        else if(_iValue == 1)
        {
            GetComponent<TextMesh>().color = Color.red;
        }
        else if(_iValue == 2 || _iValue == 5)
        {
            GetComponent<TextMesh>().color = Color.cyan;
        }
    }
}