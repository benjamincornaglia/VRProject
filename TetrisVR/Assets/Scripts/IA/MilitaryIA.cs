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

	public GameObject WeaponPosition1 = null;
	public GameObject WeaponPosition2 = null;

	public GameObject Projectile;

	[Range(1, 10)]
	public int ProjectilePoolSize = 5;

	private bool weaponAlternance;

	private float nextFire;

	private Vector3 lastTargetPosition = Vector3.zero;

	private UnityEngine.AI.NavMeshAgent agent = null;

	private List<GameObject> projectilePool = new List<GameObject>();
	private int projectileIndex = 0;

	private void fire() {
		var proj = projectilePool [projectileIndex];
		if (weaponAlternance) {
			proj.transform.position = WeaponPosition1.transform.position;
		} else {
			if (WeaponPosition2 != null) {
				proj.transform.position = WeaponPosition2.transform.position;
			} else {
				proj.transform.position = WeaponPosition1.transform.position;
			}
		}
		var rb = proj.GetComponent<Rigidbody> ();
		if (rb != null) {
			proj.SetActive (true);
			rb.AddForce ((Target.transform.position - this.transform.position).normalized * 1000.0f);
		}
		projectileIndex = (projectileIndex + 1) % ProjectilePoolSize;
		weaponAlternance = !weaponAlternance;
	}

	private void createProjectilePool() {
		for (int i=0; i<ProjectilePoolSize; i++) {
			var proj = Instantiate (Projectile);
			proj.SetActive (false);
			projectilePool.Add (proj);
		}
	}

	// Use this for initialization
	void Start () {
		if (WeaponPosition1 == null) {
			throw new UnityException ("MilitaryIA : WeaponPosition1 have to be assigned");
		}

		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

		nextFire = FireInterval;

		createProjectilePool ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance(lastTargetPosition, Target.transform.position) > UpdateDistance) {
			lastTargetPosition = Target.transform.position;
            agent.destination = new Vector3(Target.transform.position.x, 0, Target.transform.position.z);
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
