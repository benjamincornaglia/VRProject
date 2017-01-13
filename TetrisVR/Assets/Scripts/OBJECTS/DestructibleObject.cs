using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DestructibleObject : MonoBehaviour {

    public GameObject FragmentedObject;

    [Range(0.0f, 100.0f)]
    public float Solidity = 0;

    public Rigidbody[] m_aRigidbodies;

	// Use this for initialization
	void Start () {
        //enabled = GetComponent<Rigidbody>().isKinematic;
        m_aRigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach(Rigidbody rb in m_aRigidbodies)
        {
            rb.isKinematic = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        print("test1  " + collision.impulse.magnitude);
        if (collision.impulse.magnitude >= Solidity)
        {
            foreach (Rigidbody rb in m_aRigidbodies)
            {
                rb.isKinematic = false;
            }
        }
    }
}
