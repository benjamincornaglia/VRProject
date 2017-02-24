using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;


public class CivilianIA : IA {

	public GameObject Target = null;

	[Range(10.0f, 500.0f)]
	public float FleeDistance = 200.0f;

	private NavMeshAgent agent;

	private int pathIndex = -1;
	private List<Vector3> m_path;

	public List<Vector3> path {
		set {
			pathIndex = -1;
			m_path = value;
		}
		get {
			return m_path;
		}
	}

	private int nearestPointIndex() {
		float minDist = float.PositiveInfinity;
		int minIndex = -1;

		for (int i=0; i<path.Count; i++) {
			Vector3 vec = this.transform.position - path[i];
			float distSq = vec.sqrMagnitude;
			if (distSq < minDist) {
				minDist = distSq;
				minIndex = i;
			}
		}

		return minIndex;
	}

	private int farestPointIndex() {
		float maxDist = -1;
		int maxIndex = -1;

		for (int i=0; i<path.Count; i++) {
			Vector3 vec = this.transform.position - path[i];
			float distSq = vec.sqrMagnitude;
			if (distSq > maxDist) {
				maxDist = distSq;
				maxIndex = i;
			}
		}

		return maxIndex;
	}

	private void flollowPath() {
		if (pathIndex == -1) {
			pathIndex = nearestPointIndex();
			agent.destination = path[pathIndex];
		}

		if (agent.hasPath && !agent.pathPending) {
			if (agent.remainingDistance <= agent.stoppingDistance) {
				pathIndex = (pathIndex + 1) % path.Count;
				agent.destination = path[pathIndex];
				/*GameObject o = GameObject.CreatePrimitive (PrimitiveType.Sphere);
				o.transform.position = agent.destination;
				o.name = gameObject.name + pathIndex;*/
			}
		}/* else {
			pathIndex = (pathIndex + 1) % path.Count;
			agent.destination = path[pathIndex];
		}*/
	}

	// Use this for initialization
	void Start () {

		agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!alive) {
			agent.enabled = false;
			return;
		}
			
		agent.enabled = true;

		//if (Vector3.Distance(this.transform.position, Target.transform.position) > FleeDistance) {
			flollowPath();
		//} else {
			//pathIndex = -1;
			//agent.destination = path[farestPointIndex()];
		//}
	}
}
