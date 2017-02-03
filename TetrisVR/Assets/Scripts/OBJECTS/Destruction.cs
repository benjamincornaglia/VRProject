using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PicaVoxel;

[RequireComponent(typeof(AudioSource))]
public class Destruction : MonoBehaviour {

    public Exploder m_pExploder;

    public float m_fMaxExplosionRadius = 20f;
    public float m_fDestructionThreshold = 100f;
    public AudioClip m_pClip;

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
            if(collision.relativeVelocity.magnitude * 1000f > m_fDestructionThreshold)
            {
                float fExplosionRadius = collision.relativeVelocity.magnitude * 1000f;
                m_pExploder.ExplosionRadius = Mathf.Clamp(Mathf.Round(fExplosionRadius), 0, m_fMaxExplosionRadius);
                m_pExploder.Explode();
                if (GetComponent<AudioSource>() != null)
                {
                    GetComponent<AudioSource>().pitch = Random.Range(0f, 1.5f);
                    if (!GetComponent<AudioSource>().isPlaying)
                    {
                        GetComponent<AudioSource>().Play();
                        //Debug.Log ("Explosion Cue");

                    }
                }
                Debug.Log ("Relative Velo = " + collision.relativeVelocity.magnitude * 1000f);
				Debug.Log ("Explosion Radius = " + Mathf.Clamp(Mathf.Round(fExplosionRadius), 0, m_fMaxExplosionRadius));
            } 
        }
        else if(this.gameObject.tag == "Piece" && collision.gameObject.tag == "PicaVoxelVolume")
        {
            if (collision.relativeVelocity.magnitude > m_fDestructionThreshold)
            {
                float fExplosionRadius = collision.relativeVelocity.magnitude;
                m_pExploder.ExplosionRadius = Mathf.Clamp(Mathf.Round(fExplosionRadius), 0, m_fMaxExplosionRadius);
                m_pExploder.Explode();
                if (GetComponent<AudioSource>() != null)
                {
                    GetComponent<AudioSource>().pitch = Random.Range(0f, 1.5f);
                    if (!GetComponent<AudioSource>().isPlaying)
                    {
                        GetComponent<AudioSource>().Play();
                        //Debug.Log ("Explosion Cue");

                    }
                }
                Debug.Log("Relative Velo = " + collision.relativeVelocity.magnitude);
                Debug.Log("Explosion Radius = " + Mathf.Clamp(Mathf.Round(fExplosionRadius), 0, m_fMaxExplosionRadius));
            }
        }
        else if(this.gameObject.tag == "PicaVoxelVolume" && collision.gameObject.tag == "PicaVoxelVolume")
        {
            if (collision.relativeVelocity.magnitude > m_fDestructionThreshold)
            {
                float fExplosionRadius = collision.relativeVelocity.magnitude;
                m_pExploder.ExplosionRadius = Mathf.Clamp(Mathf.Round(fExplosionRadius), 0, m_fMaxExplosionRadius);
                m_pExploder.Explode();
                if (GetComponent<AudioSource>() != null)
                {
                    GetComponent<AudioSource>().pitch = Random.Range(0f, 1.5f);
                    if (!GetComponent<AudioSource>().isPlaying)
                    {
                        GetComponent<AudioSource>().Play();
                        //Debug.Log ("Explosion Cue");

                    }
                }
                //Debug.Log(GetComponent<AudioSource>().pitch);
                
                Debug.Log("Relative Velo = " + collision.relativeVelocity.magnitude);
                Debug.Log("Explosion Radius = " + Mathf.Clamp(Mathf.Round(fExplosionRadius), 0, m_fMaxExplosionRadius));
            }
        } 
    }
}
