using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class IAHelico : MonoBehaviour {

	public GameObject Target;
	public float Speed = 10.0f;
	public float FireInterval = 5;
	public GameObject Projectile;

	private float nextFire;
	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		transform.position = new Vector3 (transform.position.x, Target.transform.position.y + 100f, transform.position.z);
		nextFire = FireInterval;
		rb = GetComponent<Rigidbody> ();
	}

	private void shoot(Vector3 targetDirection) {
		nextFire -= Time.deltaTime;
		if (nextFire <= 0) {
			if (Vector3.Distance (transform.position, Target.transform.position) < 250.0f) {
				var proj = Instantiate (Projectile);
				proj.transform.position = transform.position;
				proj.GetComponent<Rigidbody> ().AddForce (targetDirection * 1000.0f);
			}
			nextFire = FireInterval;
		}
	}

	// Update is called once per frame
	void Update () {
		var targetDirection = (Target.transform.position - transform.position).normalized;

		shoot (targetDirection);

		targetDirection.y = 0;
		targetDirection.Normalize ();

		var rotation = Quaternion.LookRotation (targetDirection);
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime);

		rb.AddForce(targetDirection * Speed);

		if (transform.position.y < Target.transform.position.y + 100.0f) {
			rb.AddForce(-Physics.gravity * 20.0f * Time.deltaTime);
		}
	}
}
