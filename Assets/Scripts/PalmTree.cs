using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalmTree : MonoBehaviour {

	public float growthTime = 60;
	public int stage = 1;
	public GameObject[] stages;

	private float growthTimer;

	void Start () {
		growthTimer = growthTime;

		for (int i = 0; i < stages.Length; i++) {
			ToggleStageSections(i, false);
		}
	}
	
	void Update () {
		for (int i = 0; i < stages.Length; i++) {
			if (stage >= i + 1) {
				ToggleStageSections(i, true);
			}
		}

		growthTimer -= Time.deltaTime;
		if (growthTimer <= 0) {
			Grow();
			growthTimer = growthTime;
		}
	}

	private void Grow() {
		stage++;
	}

	private void ToggleStageSections (int index, bool op) {
		MeshRenderer[] mr = stages[index].GetComponentsInChildren<MeshRenderer>();
		foreach(MeshRenderer m in mr) {
			m.enabled = op;
		}
	}
}
