using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour {
	public int spawnWeighting;
	public string hashtag;

	public FloatingItem floater { get; private set; }

	private void Start() {
		floater = GetComponent<FloatingItem>();
	}
}
