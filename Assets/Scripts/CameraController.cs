using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float speed = 2.0f;

	void Start () {

	}
	
	void Update () {
		if (Input.GetMouseButton(1)) {
			float h = speed * Input.GetAxis("Mouse X");
			transform.parent.transform.Rotate(0, h, 0);
		}
	}
}
