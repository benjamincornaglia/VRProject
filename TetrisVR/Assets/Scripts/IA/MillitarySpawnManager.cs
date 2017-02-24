using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PicaVoxel;

public class MillitarySpawnManager : MonoBehaviour {

	public GameObject Target;

	[Range(5, 60)]
	public int ObjectPoolSize = 5;

	public GameObject PrefabHelico;
	private ObjectPool helicoPool;

	public GameObject PrefabMillitary;
	private ObjectPool millitaryPool;

	public GameObject PrefabTank;
	private ObjectPool tankPool;

	private List<GameObject> spawners = new List<GameObject>();

	private Camera camera = null;

	public int SpawnInterval = 5;

	private float spawnTimer;

	private bool isInTheFieldOfView(Vector3 position) {
		Vector3 viewSpacePosition = camera.WorldToViewportPoint(position);
		return viewSpacePosition.x >= 0 && viewSpacePosition.x <= 1 &&
			viewSpacePosition.y >= 0 && viewSpacePosition.y <= 1 &&
			viewSpacePosition.z > 0;
	}

	private GameObject getNextDeadOutsideOfFOV(ObjectPool pool) {
		for (int i=0; i<pool.size(); i++) {
			GameObject obj = pool.getNext();
			if (!obj.GetComponent<IA>().alive && !isInTheFieldOfView(obj.transform.position)) {
				return obj;
			}
		}
		return null;
	}

	private bool spawnObject(Vector3 spawnPosition, ObjectPool pool) {
		if (isInTheFieldOfView(spawnPosition)) {
			return false;
		}

		GameObject obj = getNextDeadOutsideOfFOV(pool);

		if (obj == null) {
			return false;
		}

		//spawnPosition.y = 0;
		
		obj.transform.position = spawnPosition;
		obj.SetActive(true);
		obj.GetComponent<IA>().alive = true;
		return true;
	}
	
	bool spawnHelico() {
        Vector3 spawnPosition =
			Target.transform.position - (camera.transform.forward * 500) + new Vector3(0, 400, 0);

		return spawnObject(spawnPosition, helicoPool);
	}

	GameObject getRandomSpawnerOutsideFOV(int maxTry) {
		for (int i=0; i<maxTry; i++) {
			GameObject current = spawners[Random.Range(0, spawners.Count)];
			if (!isInTheFieldOfView(current.transform.position)) {
				return current;
			}
		}
		return null;
	}

	bool spawnNavMeshGuided(ObjectPool pool) {
		GameObject spawner = getRandomSpawnerOutsideFOV(10);
		if (spawner == null) {
			return false;
		}

		return spawnObject(spawner.transform.position, pool);
	}

	bool spawnTank() {
        return spawnNavMeshGuided(tankPool);
	}

	bool spawnMilitary() {
		return spawnNavMeshGuided(millitaryPool);
	}

	void spawnRandom() {
		int type = Random.Range(0, 10);
		if (type == 0) {
			spawnHelico();
		} else if (type < 3) {
			spawnTank();
		} else {
			spawnMilitary();
		}
	}

	// Use this for initialization
	void Start () {
		foreach(Transform child in this.transform) {
			spawners.Add(child.gameObject);
		}

		if (spawners.Count == 0) {
			throw new UnityException("IASpawnManager : SpawnManagerObject must contain at least one spwaner.");
		}

		if (PrefabTank == null) {
			throw new UnityException("IASpawnManager : Tank prefab has to be assigned.");
		}

		tankPool = new ObjectPool(PrefabTank, 1, Target);

		if (PrefabMillitary == null) {
			throw new UnityException("IASpawnManager : Millitary prefab has to be assigned.");
		}

		millitaryPool = new ObjectPool(PrefabMillitary, ObjectPoolSize, Target);

		if (PrefabHelico == null) {
			throw new UnityException("IASpawnManager : Helico prefab has to be assigned.");
		}

		helicoPool = new ObjectPool(PrefabHelico, ObjectPoolSize, Target);

		spawnTimer = SpawnInterval;
	}

	// Update is called once per frame
	void Update () {
		if (camera == null) {
			camera = Camera.main;
			return;
		}

		spawnTimer -= Time.deltaTime;
		if (spawnTimer <= 0.0f) {
			spawnRandom();
			spawnTimer = SpawnInterval;
		}
	}
}
