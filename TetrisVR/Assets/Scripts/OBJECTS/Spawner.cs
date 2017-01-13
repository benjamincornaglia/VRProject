using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public List<GameObject> m_aSpawners = new List<GameObject>();

    public GameObject m_pObjectToSpawn;

    public float m_fTimer = 10f;
    private float m_fActualTimer = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (m_fActualTimer < m_fTimer)
        {
            m_fActualTimer += Time.deltaTime;
        }
        else if (m_fActualTimer >= m_fTimer)
        {
            SpawnObject();
            m_fActualTimer = 0;
        }
    }

    void SpawnObject()
    {
        int i = Random.Range(0, m_aSpawners.Count - 1);
        Instantiate(m_pObjectToSpawn, m_aSpawners[i].transform.position, Quaternion.identity);
        

    }

    
}
