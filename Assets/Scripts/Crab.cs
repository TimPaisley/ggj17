using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour {

	public float speed = 1.0f;
	public float frequency = 1.0f;
	public bool PickedUp = false;

	private float timer;
	private Vector3 goTowards;

	void Start () {
		timer = frequency;
		goTowards = new Vector3(Random.Range(-50, 50), transform.position.y, Random.Range(-50, 50));
	}
	
	void Update () {
		timer -= Time.deltaTime;

		if (timer <= 0.0f) {
			goTowards = new Vector3(Random.Range(-50, 50), transform.position.y, Random.Range(-50, 50));
			timer = frequency;
		}

		if (!PickedUp) {
			float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, goTowards, step);
			transform.LookAt(goTowards);
		}
	}
}
