using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilitaryIA : ArmedIA {

	public float UpdateDistance = 5f;

	private Vector3 lastTargetPosition = Vector3.zero;

	private UnityEngine.AI.NavMeshAgent agent = null;

    

	// Use this for initialization
	void Start () {
		base.Start();
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();
		
		if (!alive) {
			agent.enabled = false;
			return;
		}
			
		agent.enabled = true;

		if (Vector3.Distance(lastTargetPosition, Target.transform.position) > UpdateDistance) {
			lastTargetPosition = Target.transform.position;
            agent.destination = new Vector3(Target.transform.position.x, 0, Target.transform.position.z);
		}
	}
}
