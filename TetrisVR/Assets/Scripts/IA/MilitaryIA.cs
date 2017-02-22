using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilitaryIA : ArmedIA {

	public float UpdateDistance = 5f;

	private Vector3 lastTargetPosition = Vector3.zero;

	private UnityEngine.AI.NavMeshAgent agent = null;

    void addMeshFilter()
    {
        var chunks = transform.FindChild("Frame 1").GetChild(0);
        MeshFilter[] meshFilters = chunks.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            i++;
        }
        MeshFilter filter = gameObject.AddComponent<MeshFilter>();
        filter.mesh = new Mesh();
        filter.mesh.CombineMeshes(combine);
    }

	// Use this for initialization
	void Start () {
		base.Start();
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();
		
		if (Vector3.Distance(lastTargetPosition, Target.transform.position) > UpdateDistance) {
			lastTargetPosition = Target.transform.position;
            agent.destination = new Vector3(Target.transform.position.x, 0, Target.transform.position.z);
		}
	}
}
