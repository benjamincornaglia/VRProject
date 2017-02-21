using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingProjectile : MonoBehaviour {

	public ParticleSystem Particles;
	private ParticleSystem particles;

	// Use this for initialization
	void Start () {
		if (Particles == null) {
			throw new UnityException("ExplodingProjectile : Particles has to be assigned.");
		}

		particles = Instantiate(Particles) as ParticleSystem;
		particles.Stop();
	}

	void OnCollisionEnter(Collision collision) {
		particles.transform.position = transform.position;
		particles.Play();
	}
}
