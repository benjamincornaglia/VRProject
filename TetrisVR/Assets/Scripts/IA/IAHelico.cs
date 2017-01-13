using UnityEngine;
using System.Collections;

public class IAHelico : MonoBehaviour {

	public GameObject Target;
	public float Speed = 10.0f;
	public float FireInterval = 5;
	public GameObject Projectile;

	private float nextFire;
	private Rigidbody rb;

	float lastDistance = 0;

	private bool alive = true;

	// Use this for initialization
	void Start () {
		transform.position = new Vector3 (transform.position.x, Target.transform.position.y + 200f, transform.position.z);
		nextFire = FireInterval;
		rb = transform.parent.GetComponent<Rigidbody> ();
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
	void FixedUpdate () {
		if (!alive) {
			return;
		}

		var targetDirection = (Target.transform.position - transform.position).normalized;

		shoot (targetDirection);

		targetDirection.y = 0;
		targetDirection.Normalize ();

		var rotation = Quaternion.LookRotation (targetDirection);
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime);

		float distance = Vector3.Distance (transform.position, 2.0f * Target.transform.position);

		if (distance > 200.0f) {

			if (distance < lastDistance) {
				rb.AddForce (Vector3.ClampMagnitude (targetDirection * Speed, 1.0f));
			} else {
				rb.AddForce (targetDirection * Speed);
			}
		}

		lastDistance = distance;

		if (transform.position.y < Target.transform.position.y + 150.0f) {
			rb.AddForce(-Physics.gravity * 5.5f * Time.deltaTime);
		}

		if (rb.velocity.y < -10) {
			rb.AddForce (-Physics.gravity * 5.0f * Time.deltaTime);
		}
	}

	void OnCollisionEnter(Collision collision) {
		alive = false;
	}
}
