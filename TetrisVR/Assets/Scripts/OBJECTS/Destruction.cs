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

    public GameObject m_pRubble;

    bool m_bHasSpawnedRubble = false;
    float m_fSpawnRubbleTimer = 0f;
    public float m_fSpawnTimer = 10f;

    public bool m_bCanSpawnRubbles = false;
	// Use this for initialization
	void Start () {
        m_pExploder = this.GetComponent<Exploder>();
	}
	
	// Update is called once per frame
	void Update () {

        ManageSpawnTimer();

	}

    void ManageSpawnTimer()
    {
        if (m_bHasSpawnedRubble)
        {
            m_fSpawnRubbleTimer += Time.deltaTime;
        }

        if (m_fSpawnRubbleTimer >= m_fSpawnTimer)
        {
            m_fSpawnRubbleTimer = 0;
            m_bHasSpawnedRubble = false;
        }
    }

    void RandomRubbleSpawn(Vector3 _vPos)
    {
        
        int i = Random.Range(0, 100);
        if(i >= 50)
        {
            if(!m_bHasSpawnedRubble && m_bCanSpawnRubbles)
            {
                GameObject pRubble = GameObject.Instantiate(m_pRubble, _vPos, new Quaternion(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
                pRubble.transform.localScale = new Vector3(Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3));
                m_bHasSpawnedRubble = true;
                pRubble.GetComponent<Destruction>().m_bCanSpawnRubbles = false;
                pRubble.GetComponent<shaderGlow>().lightOff();
                Debug.Log("Rubble Spawn");
            }
        }
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
                RandomRubbleSpawn(collision.contacts[0].point);
                //Debug.Log ("Relative Velo = " + collision.relativeVelocity.magnitude * 1000f);
				//Debug.Log ("Explosion Radius = " + Mathf.Clamp(Mathf.Round(fExplosionRadius), 0, m_fMaxExplosionRadius));
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
                RandomRubbleSpawn(collision.contacts[0].point);
                //Debug.Log("Relative Velo = " + collision.relativeVelocity.magnitude);
                //Debug.Log("Explosion Radius = " + Mathf.Clamp(Mathf.Round(fExplosionRadius), 0, m_fMaxExplosionRadius));
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
                RandomRubbleSpawn(collision.contacts[0].point);
                //Debug.Log("Relative Velo = " + collision.relativeVelocity.magnitude);
                //Debug.Log("Explosion Radius = " + Mathf.Clamp(Mathf.Round(fExplosionRadius), 0, m_fMaxExplosionRadius));
            }
        } 
    }
}
