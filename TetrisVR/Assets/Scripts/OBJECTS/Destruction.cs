using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PicaVoxel;

public class Destruction : MonoBehaviour {

    public Exploder m_pExploder;

    public float m_fMaxExplosionRadius = 20f;
    public float m_fDestructionThreshold = 100f;

	// Use this for initialization
	void Start () {
        m_pExploder = this.GetComponent<Exploder>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(this.gameObject.tag == "Destructor" && collision.gameObject.tag == "PicaVoxelVolume")
        {
            if(collision.relativeVelocity.magnitude > m_fDestructionThreshold)
            {
                float fExplosionRadius = collision.relativeVelocity.magnitude / 100;
                m_pExploder.ExplosionRadius = Mathf.Clamp(Mathf.Round(fExplosionRadius), 0, m_fMaxExplosionRadius);
                m_pExploder.Explode();
            }
            Debug.Log("Impact force = " + collision.relativeVelocity.magnitude);
            Debug.Log("ExplosionRadius = " + m_pExploder.ExplosionRadius);
            
        }
        
        
    }
}
