using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SoundManager : MonoBehaviour {

	public AudioSource m_pAirbornCue;
	public AudioSource m_pLandingCue;
	public GameObject m_pMusicSource;

	// Use this for initialization
	void Start () {
		m_pAirbornCue = GameObject.Find("AirbornSource").GetComponent<AudioSource>();
		m_pLandingCue = GameObject.Find("LandingSource").GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () {

	}

	private void OnCollisionEnter(Collision collision)
	{  if(collision.gameObject.tag == "Floor" || collision.gameObject.tag == "PicaVoxelVolume")
		{

			m_pLandingCue.pitch = Random.Range(0.5f, 1f);
			if(!m_pLandingCue.isPlaying)
			{
				m_pLandingCue.Play();
				//Debug.Log("LandingCue");
			}
			if (m_pAirbornCue.isPlaying)
				m_pAirbornCue.Stop();

		}

		//Debug.Log ("Hit");
	}

	private void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "PicaVoxelVolume")
		{

			if (m_pLandingCue.isPlaying)
				m_pLandingCue.Stop();

			if (m_pAirbornCue.volume < 1f)
				m_pAirbornCue.DOFade(1f, 2f);
			if (m_pAirbornCue.isPlaying == false)
				m_pAirbornCue.Play();
		}

		//ebug.Log ("EndHit");
	}
}
