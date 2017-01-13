using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PicaVoxel;

public class Destruction : MonoBehaviour {

    public Exploder m_pExploder;

	// Use this for initialization
	void Start () {
        m_pExploder = this.GetComponent<Exploder>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        m_pExploder.Explode();
        
    }
}
