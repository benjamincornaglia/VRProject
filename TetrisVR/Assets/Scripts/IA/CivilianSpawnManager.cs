using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianSpawnManager : MonoBehaviour {

	public GameObject Target;

	[Range(1, 60)]
	public int ObjectPoolSize = 5;

	public GameObject PrefabMan1;
	private ObjectPool man1Pool;

	public GameObject PrefabMan2;
	private ObjectPool man2Pool;

	public GameObject PrefabLady1;
	private ObjectPool lady1Pool;

	public GameObject PrefabLady2;
	private ObjectPool lady2Pool;

	private List<GameObject> spawners = new List<GameObject>();
	private List<Vector3> path = new List<Vector3>();

	private Camera camera = null;

	public int SpawnInterval = 5;

	private float spawnTimer;

	private ObjectPool[] pools = new ObjectPool[4];

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

	void spawnRandom() {
		int type = Random.Range(0, pools.Length);
		spawnNavMeshGuided(pools[type]);
		//spawnNavMeshGuided(pools[0]);
	}

	// Use this for initialization
	void Start () {
		foreach(Transform child in this.transform) {
			spawners.Add(child.gameObject);
			path.Add(child.position);
		}

		if (spawners.Count == 0) {
			throw new UnityException("IASpawnManager : SpawnManagerObject must contain at least one spwaner.");
		}

		if (PrefabMan1 == null) {
			throw new UnityException("IASpawnManager :  Man1 prefab has to be assigned.");
		}

		man1Pool = new ObjectPool(PrefabMan1, ObjectPoolSize, Target, path);

		if (PrefabMan2 == null) {
			throw new UnityException("IASpawnManager : Man2 prefab has to be assigned.");
		}

		man2Pool = new ObjectPool(PrefabMan2, ObjectPoolSize, Target, path);

		if (PrefabLady1 == null) {
			throw new UnityException("IASpawnManager : Lady1 prefab has to be assigned.");
		}

		lady1Pool = new ObjectPool(PrefabLady1, ObjectPoolSize, Target, path);

		if (PrefabLady2 == null) {
			throw new UnityException("IASpawnManager : Lady2 prefab has to be assigned.");
		}

		lady2Pool = new ObjectPool(PrefabLady2, ObjectPoolSize, Target, path);

		spawnTimer = SpawnInterval;

		pools[0] = man1Pool;
		pools[1] = man2Pool;
		pools[2] = lady1Pool;
		pools[3] = lady2Pool;
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
