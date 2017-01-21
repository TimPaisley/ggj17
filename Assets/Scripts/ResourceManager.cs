using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceManager : MonoBehaviour {
	public float minAppearTime = 1f;
	public float maxAppearTime = 10f;
	public int spawnRegionSize = 5;
	public int spawnRegionInnerSize = 3;
	public Transform positioner;
	public Transform messageBottle;

	Resource[] resources;
	float nextAppearTime;

	LandChecker landChecker;
	List<Rect> spawnLocations;
	Dictionary<Vector2, Resource> spawnedObjects = new Dictionary<Vector2, Resource>();

	void Start() {
		var twitterManager = FindObjectOfType<TwitterManager>();
		if (twitterManager != null) {
			twitterManager.OnTweet += spawnBottledResource;
		}

		resources = GetComponentsInChildren<Resource>();
		foreach (var resource in resources) {
			resource.gameObject.SetActive(false);
		}

		Debug.Log(resources.Count());

		nextAppearTime = Random.Range(minAppearTime, maxAppearTime);
		landChecker = FindObjectOfType<LandChecker>();
		spawnLocations = calculateSpawnLocations();

		StartCoroutine(spawnResources());
	}

	void spawnBottledResource(Twitter.Tweet reason) {
		var bottle = Instantiate(messageBottle);
		bottle.transform.position = new Vector3(0, 5, 0);
	}

	IEnumerator spawnResources() {
		while (true) {
			yield return new WaitForSeconds(nextAppearTime);
			spawnResource();

			// Alter the appear time range depending on what it was last time
			// If something took ages to spawn, spawn something quickly and vice versa
			var minNextAppearTime = Mathf.Max(minAppearTime, (maxAppearTime - nextAppearTime) / 2);
			var maxNextAppearTime = Mathf.Min(maxAppearTime, minNextAppearTime * 3);
			nextAppearTime = Random.Range(minNextAppearTime, maxNextAppearTime);
		}
	}

	void spawnResource() {
		cleanupSpawnedObjects();
		var locations = availableSpawnLocations().ToList();

		if (locations.Any()) {
			List<Vector2> location;
			Vector2 position;

			do {
				location = locations.ElementAt(Random.Range(0, locations.Count()));
				position = location.ElementAt(Random.Range(0, location.Count()));
			} while (spawnedObjects.Keys.Contains(position));

			var totalWeight = resources.Sum(res => res.spawnWeighting);
			var target = Random.Range(0, totalWeight);
			var resource = resources.First(res => {
				if (target < res.spawnWeighting) return true;
				target -= res.spawnWeighting;
				return false;
			});

			var instance = Instantiate(resource);

			instance.transform.parent = positioner;
			instance.transform.localPosition = new Vector3(position.x, 0, position.y);
			instance.gameObject.SetActive(true);
			spawnedObjects.Add(position, instance);
		}
	}

	List<Rect> calculateSpawnLocations() {
		var locations = new List<Rect>();
		var padding = (spawnRegionSize - spawnRegionInnerSize) / 2;

		for (var x = 0; x < landChecker.size; x += spawnRegionSize) {
			for (var y = 0; y < landChecker.size; y += spawnRegionSize) {
				locations.Add(new Rect(x + padding, y + padding, spawnRegionInnerSize, spawnRegionInnerSize));
			}
		}

		return locations;
	}

	IEnumerable<List<Vector2>> availableSpawnLocations() {
		var all = allAvailableLocations();
		var preferred = locationsNearLand(all);

		return preferred.Any() ? preferred : all;
	}

	IEnumerable<List<Vector2>> allAvailableLocations() {
		return spawnLocations
			.Where(location => !spawnedObjects.Keys.Any(location.Contains))
			.Select(location => allPoints(location).ToList());
	}

	IEnumerable<List<Vector2>> locationsNearLand(IEnumerable<List<Vector2>> allLocations) {
		return allLocations.Where(location => location.Count > 0 && location.Count < spawnRegionInnerSize * spawnRegionInnerSize);
	}

	IEnumerable<Vector2> allPoints(Rect location) {
		var points = new List<Vector2>();

		for (var x = location.xMin; x <= location.xMax; x++) {
			for (var y = location.yMin; y <= location.yMax; y++) {
				if (!landChecker.IsUnderLand(x, y)) points.Add(new Vector2(x, y));
			}
		}

		return points;
	}

	void cleanupSpawnedObjects() {
		var toRemove = new List<Vector2>();

		foreach (var pair in spawnedObjects) {
			if (!pair.Value.gameObject.activeSelf) {
				toRemove.Add(pair.Key);
				Destroy(pair.Value.gameObject);
			} else if (!pair.Value.floater.enabled) {
				toRemove.Add(pair.Key);
			}
		}

		foreach (var key in toRemove) {
			spawnedObjects.Remove(key);
		}
	}
}
