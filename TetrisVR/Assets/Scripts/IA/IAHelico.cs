using UnityEngine;
using System.Collections;

public class IAHelico : ArmedIA {

	public float Speed = 10.0f;

	public GameObject Helices;

	private Rigidbody rb;

	float lastDistance = 0;

	// Use this for initialization
	void Start () {
		base.Start();
		rb = GetComponent<Rigidbody> ();
	}

	void Update() {
        base.Update();

        if (alive) {
            Helices.transform.Rotate(0, 20, 0);
        }
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (!alive) {
			return;
		}

		var targetDirection = (Target.transform.position - transform.position).normalized;

		targetDirection.y = 0;
		targetDirection.Normalize ();

		if (targetDirection != Vector3.zero) {
			var rotation = Quaternion.LookRotation (targetDirection);
			transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime);
		}

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
}
