using UnityEngine;
using System.Collections;

public class TweetBlock : Block {
	public GameObject child;

	public override void Shifting() {
		Debug.Log("CHILD");
		Debug.Log(child);

		if (child == null) {
			Destroy(gameObject);
		} else {
			foreach (var rend in GetComponentsInChildren<Renderer>()) {
				rend.enabled = false;
			}

			foreach (var rend in child.GetComponentsInChildren<Renderer>()) {
				rend.enabled = true;
			}

			Debug.Log("toggled enabled on stuff");

			base.Shifting();
		}
	}
}
