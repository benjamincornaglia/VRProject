using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA : MonoBehaviour {

	[Range(0.01f, 20.0f)]
	public float Solidity = 0.4f;

	public GameObject CurrentProjectile
    {
        get;
        protected set;
    }

    public bool alive {
        get;
        set;
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject != CurrentProjectile && collision.impulse.magnitude > Solidity) {
            alive = false;
        }
	}
}
