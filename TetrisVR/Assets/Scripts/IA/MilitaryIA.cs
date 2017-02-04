using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilitaryIA : MonoBehaviour {

	public GameObject Target;

	public float UpdateDistance = 5f;

	[Range(5.0f, 60.0f)]
	public float FireDistance = 30.0f;

	[Range(5f, 20f)]
	public float FireInterval = 5f;

	private float nextFire;

	private Vector3 lastTargetPosition = Vector3.zero;

	private UnityEngine.AI.NavMeshAgent agent = null;

	private void fire() {
		
	}

	// Use this for initialization
	void Start () {
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

		nextFire = FireInterval;
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance(lastTargetPosition, Target.transform.position) > UpdateDistance) {
			lastTargetPosition = Target.transform.position;
			agent.destination = Target.transform.position;
		}

		if (Vector3.Distance(this.transform.position, Target.transform.position) < FireDistance) {
			nextFire -= Time.deltaTime;
			if (nextFire <= 0.0f) {
				fire();
				nextFire = FireInterval;
			}
		} else {
			nextFire = FireInterval;
		}
	}
}
