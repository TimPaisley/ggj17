using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandChecker : MonoBehaviour {
	public int size;

	bool[,] landPositions;
	WaterCube[] cubes;

	void Start() {
		landPositions = new bool[size, size];
		cubes = GetComponentsInChildren<WaterCube>();
		Check();
	}

	public void Check() {
		foreach (var cube in cubes) {
			landPositions[
				Mathf.RoundToInt(cube.transform.localPosition.x),
				Mathf.RoundToInt(cube.transform.localPosition.z)
			] = cube.CheckUnderLand();
		}
	}
	
	public bool IsUnderLand(float x, float z) {
		return landPositions[Mathf.RoundToInt(x), Mathf.RoundToInt(z)];
	}
}
