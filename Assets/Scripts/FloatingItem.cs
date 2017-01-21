using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingItem : MonoBehaviour {
	public Transform floater;
	public Transform ripples;

	public float timeout = 5f;
	public float bobSize = 0.05f;
	public float bobSpeed = 3.5f;
	public float baseY = -0.05f;
	public float riseSinkVelocity = 1f;
	public float sinkDepth = -1f;

	float startTime;
	ParticleSystem rippleParticles;
	Coroutine lifecycle;

	void Start() {
		rippleParticles = ripples.GetComponent<ParticleSystem>();
		floater.position = new Vector3(floater.position.x, sinkDepth, floater.position.z);

		lifecycle = StartCoroutine(runLifecycle());
	}

	void Update() {
	}

	public void Disable() {
		StopCoroutine(lifecycle);
		floater.localPosition = new Vector3(0, 0, 0);
		enabled = false;
	}

	IEnumerator runLifecycle() {
		rippleParticles.Play();
		yield return new WaitForEndOfFrame();

		while (floater.position.y < baseY) {
			floater.position = floater.position + (Vector3.up * Time.deltaTime * riseSinkVelocity);
			yield return new WaitForEndOfFrame();
		}

		startTime = Time.time;
		var endTime = startTime + timeout;

		while (Time.time < endTime) {
			floater.position = new Vector3(
				floater.position.x,
				baseY + Mathf.Sin(bobSpeed * Time.time) * bobSize,
				floater.position.z
			);
			yield return new WaitForEndOfFrame();
		}

		while (floater.position.y > sinkDepth) {
			floater.position = floater.position - (Vector3.up * Time.deltaTime * riseSinkVelocity);
			yield return new WaitForEndOfFrame();
		}

		gameObject.SetActive(false);
	}
}
