using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmedIA : MonoBehaviour {

	[Range(5.0f, 500.0f)]
	public float FireDistance = 100.0f;

	[Range(5f, 20f)]
	public float FireInterval = 5f;

	public GameObject WeaponPosition1 = null;
	public GameObject WeaponPosition2 = null;

	public GameObject Projectile;

	[Range(1, 10)]
	public int ProjectilePoolSize = 1;

	private ObjectPool projectilePool = new ObjectPool();

	private bool weaponAlternance;

	private float nextFire;

	public GameObject Target;

	public ParticleSystem Particles;

	private ParticleSystem particles;

    protected bool alive = false;

    protected GameObject CurrentProjectile
    {
        get;
        private set;
    }

    bool meshFilterAdded = false;

    bool addMeshFilter()
    {
        var chunks = transform.FindChild("Frame 1").GetChild(0);
        MeshFilter[] meshFilters = chunks.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        while (i < meshFilters.Length)
        {
            if (meshFilters[i].mesh == null)
            {
                print("test");
                return false;
            }
            combine[i].mesh = meshFilters[i].mesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            i++;
        }
        MeshFilter filter = gameObject.AddComponent<MeshFilter>();
        filter.mesh = new Mesh();
        filter.mesh.CombineMeshes(combine);
        GetComponent<shaderGlow>().enabled = true;
        return true;
    }

    private void createProjectilePool() {
		for (int i=0; i<ProjectilePoolSize; i++) {
			var proj = Instantiate (Projectile);
			proj.SetActive (false);
			projectilePool.AddObject(proj);
		}
	}

    private bool facingTarget() {
        Vector3 vectorToTarget = Target.transform.position - this.transform.position;
        vectorToTarget.y = 0;
        return Vector3.Angle(transform.forward, vectorToTarget) < 10;
    }

    private void fire() {
        if (facingTarget()) {
            var proj = projectilePool.getNext();
            CurrentProjectile = proj;
            if (weaponAlternance) {
                proj.transform.position = WeaponPosition1.transform.position;
            } else {
                if (WeaponPosition2 != null) {
                    particles.transform.position = WeaponPosition2.transform.position;
                    proj.transform.position = WeaponPosition2.transform.position;
                } else {
                    particles.transform.position = WeaponPosition1.transform.position;
                    proj.transform.position = WeaponPosition1.transform.position;
                }
            }
            var rb = proj.GetComponent<Rigidbody>();
            if (rb != null) {
                proj.SetActive(true);
                rb.AddForce((Target.transform.position - this.transform.position).normalized * 500.0f);
            }
            particles.Play();
            weaponAlternance = !weaponAlternance;
        }
	}

	// Use this for initialization
	public void Start () {
		if (WeaponPosition1 == null) {
			throw new UnityException ("ArmedIA : WeaponPosition1 have to be assigned");
		}

		createProjectilePool();
		nextFire = FireInterval;

		if (Particles == null) {
			throw new UnityException("ArmedIA : particles have to be assigned.");
		}

		particles = Instantiate(Particles) as ParticleSystem;
		particles.Stop();
	}
	
	// Update is called once per frame
	public void Update () {
        if (!meshFilterAdded)
        {
            meshFilterAdded = addMeshFilter();
        }

        if (!alive)
        {
            return;
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
