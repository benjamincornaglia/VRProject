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
    public int m_iRubbleStacks = 3;

    public bool m_bCanSpawnRubbles = false;

    List<Vector3> avPos = new List<Vector3>();
    float fDiff = 0;
    List<float> afSpeeds = new List<float>();

    public bool m_bGrabbed = false;

    public ScoreManager _themanager = null;
    private bool _collisioned = false;
    float m_fColTimer = 0f;

	public bool m_bCanScore = false;

    enum PlayMode { VR, Debug };
    [SerializeField]
    PlayMode m_ePlayMode;

    // Use this for initialization
    void Start () {
        m_pExploder = this.GetComponent<Exploder>();

        switch (m_ePlayMode)
        {
            case PlayMode.Debug:
                _themanager = GameObject.Find("CharacterController").GetComponent<ScoreManager>();
                break;
            case PlayMode.VR:
                _themanager = GameObject.Find("VRController").GetComponent<ScoreManager>();
                break;
        }
        
	}
	
	// Update is called once per frame
	void Update () {

        ManageSpawnTimer();
        CheckMovementVelocity();

        if (_collisioned)
            m_fColTimer += Time.deltaTime;
        if (m_fColTimer > 1)
        {
            _collisioned = false;
            m_fColTimer = 0f;
        }
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
        
        int i = Random.Range(0, 1000);
        if(i >= 950)
        {
			if (!m_bHasSpawnedRubble && m_bCanSpawnRubbles && this.gameObject.tag == "Piece") {
				//for(int index = 0; index <= m_iRubbleStacks; i++)
				//{
				GameObject pRubble = GameObject.Instantiate (m_pRubble, _vPos, new Quaternion (Random.Range (0, 360), Random.Range (0, 360), Random.Range (0, 360), Random.Range (0, 360)));
				pRubble.transform.localScale = new Vector3 (Random.Range (1, 3), Random.Range (1, 3), Random.Range (1, 3));
				m_bHasSpawnedRubble = true;
				pRubble.GetComponent<Destruction> ().m_bCanSpawnRubbles = false;
				//pRubble.GetComponent<shaderGlow>().lightOff();
				pRubble.GetComponent<Rigidbody> ().velocity = Vector3.zero;
				pRubble.GetComponent<Rigidbody> ().useGravity = true;
				//}
                

			} else if(m_bCanSpawnRubbles && this.gameObject.tag == "Destructor"){
			
				GameObject pRubble = GameObject.Instantiate (m_pRubble, _vPos, new Quaternion (Random.Range (0, 360), Random.Range (0, 360), Random.Range (0, 360), Random.Range (0, 360)));
				pRubble.transform.localScale = new Vector3 (Random.Range (1, 3), Random.Range (1, 3), Random.Range (1, 3));
				m_bHasSpawnedRubble = true;
				pRubble.GetComponent<Destruction> ().m_bCanSpawnRubbles = false;
				//pRubble.GetComponent<shaderGlow>().lightOff();
				pRubble.GetComponent<Rigidbody> ().velocity = Vector3.zero;
				pRubble.GetComponent<Rigidbody> ().useGravity = true;

				Debug.Log ("Rubble Spawn");
			}
        }
    }

    

    void CheckMovementVelocity()
    {

        if (avPos.Count > 2)
        {
            avPos.RemoveAt(2);
        }

        avPos.Insert(0, transform.position);

        if(avPos.Count >= 2)
            fDiff = Vector3.Distance(avPos[0], avPos[1]);

        if (afSpeeds.Count > 10)
            afSpeeds.RemoveAt(10);
        afSpeeds.Insert(0, fDiff);

        if (afSpeeds.Count > 3)
        {
            if(!m_bGrabbed)
                fDiff = 2 + Mathf.Clamp(afSpeeds[2] + 1, 1, m_fMaxExplosionRadius);
            else
                fDiff = 2 + Mathf.Clamp(afSpeeds[2]*10f + 1, 1, m_fMaxExplosionRadius);

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
		if(this.gameObject.tag == "Destructor" && collision.gameObject.tag == "PicaVoxelVolume")
        {
            if (fDiff > m_fDestructionThreshold)
            {
                //float fExplosionRadius = collision.relativeVelocity.magnitude * 1000f;
                float fExplosionRadius = fDiff;
				m_pExploder.ExplosionRadius = 5f;
                m_pExploder.Explode();
				Debug.Log ("Swipe");
                if (GetComponent<AudioSource>() != null)
                {
                    GetComponent<AudioSource>().pitch = Random.Range(0f, 1.5f);
                    if (!GetComponent<AudioSource>().isPlaying)
                    {
                        GetComponent<AudioSource>().Play();
                        //Debug.Log ("Explosion Cue");

                    }
                }
                //RandomRubbleSpawn(collision.contacts[0].point);
                if(_collisioned == false)
                {
                    _collisioned = true;
                    //_themanager.swipeHappen(collision.contacts[0].point);
                    transform.GetChild(1).GetComponent<AudioSource>().pitch = Random.Range(1f, 1.1f);
                    transform.GetChild(1).GetComponent<AudioSource>().Play();
                }
                
                //Debug.Log ("Relative Velo = " + collision.relativeVelocity.magnitude * 1000f);
                //Debug.Log ("Explosion Radius = " + Mathf.Clamp(Mathf.Round(fExplosionRadius), 0, m_fMaxExplosionRadius));
            } 
        }
        else if(this.gameObject.tag == "Piece" && collision.gameObject.tag == "PicaVoxelVolume")
        {
            if (fDiff > m_fDestructionThreshold && m_bCanScore)
            {
                if(!m_bGrabbed)
                    m_bCanScore = false;
                float fExplosionRadius = fDiff;
                //float fExplosionRadius = Random.Range(2, 10);
                m_pExploder.ExplosionRadius = Mathf.Clamp(Mathf.Round(fExplosionRadius), 1, m_fMaxExplosionRadius);
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
                //RandomRubbleSpawn(collision.contacts[0].point);
				if(!m_bGrabbed && !_collisioned)
                {
                    _collisioned = true;
                    //_themanager.throwhappen(collision.contacts[0].point);
                    transform.GetChild(0).GetComponent<AudioSource>().pitch = Random.Range(1f, 1.1f);
                    transform.GetChild(0).GetComponent<AudioSource>().Play();
                }
                else if(m_bGrabbed && !_collisioned)
                {
                    _collisioned = true;
                   // _themanager.swipeHappen(collision.contacts[0].point);
                    transform.GetChild(1).GetComponent<AudioSource>().pitch = Random.Range(1f, 1.1f);
                    transform.GetChild(1).GetComponent<AudioSource>().Play();
                }
            }
            //Debug.Log("Boom.");
        }
        else if(this.gameObject.tag == "PicaVoxelVolume" && collision.gameObject.tag == "PicaVoxelVolume")
        {
            if (collision.relativeVelocity.magnitude > m_fDestructionThreshold)
            {
                float fExplosionRadius = fDiff;
                m_pExploder.ExplosionRadius = Mathf.Clamp(Mathf.Round(fExplosionRadius), 1, m_fMaxExplosionRadius);
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
