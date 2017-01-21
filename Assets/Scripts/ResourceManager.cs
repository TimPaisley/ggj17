using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {
	public float minAppearTime = 1f;
	public float maxAppearTime = 10f;

	public GameObject[] resources;

	float nextAppearTime;

	void Start() {
		nextAppearTime = Random.Range(minAppearTime, maxAppearTime);

		StartCoroutine(spawnResources());
	}

	IEnumerator spawnResources() {
		while (true) {
			yield return new WaitForSeconds(nextAppearTime);
			spawnResource();

			// Alter the appear time range depending on what it was last time
			// If something took ages to spawn, spawn something quickly and vice versa
			var minNextAppearTime = Mathf.Max(minAppearTime, (maxAppearTime - nextAppearTime) / 2);
			var maxNextAppearTime = Mathf.Min(maxAppearTime, minNextAppearTime * 3);
			nextAppearTime = Random.Range(minNextAppearTime, maxAppearTime);
		}
	}

	void spawnResource() {
		var resource = resources[Random.Range(0, resources.Length - 1)];

		Instantiate(resource);
	}
}
