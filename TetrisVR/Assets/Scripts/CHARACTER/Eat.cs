using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eat : MonoBehaviour {

	public GameObject m_pMyController;
	public GameObject m_pBloodParticles;
	public float m_fHealthRestoredPerComestible = 25f;
	public GameObject m_pUsedCtrl;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Comestible")
		{
			
			m_pMyController.GetComponent<HealthManager>().HealthInput(m_fHealthRestoredPerComestible);
			m_pBloodParticles.GetComponent<ParticleSystem>().Play();
			GameObject.Destroy(other.gameObject);
			m_pUsedCtrl.GetComponent<Manipulation> ().m_bHasObject = false;
			Debug.Log("Health Up");
			//bonjour
		}
	}
}
