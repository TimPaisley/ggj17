using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public GameObject marker;

	public Material sand;
	public Material dirt;
	public Material rock;

	private Stack<Block> inventory;
	private Block focus;
	
	void Start () {
		inventory = new Stack<Block>();
	}
	
	void Update () {
		Vector3 normal = RaycastForBlock();

		// PLACE BLOCK: left mouse button
		if (Input.GetMouseButtonDown(0) && focus != null && inventory.Count > 0) {
			Block block = inventory.Pop();
			block.transform.position = focus.transform.position + normal;
			block.gameObject.SetActive(true);
		}

		// PICK UP BLOCK: right mouse button
		else if (Input.GetMouseButtonDown(1) && focus != null && inventory.Count < 3) {
			inventory.Push(focus);
			focus.gameObject.SetActive(false);
		}

		// middle mouse button
		else if (Input.GetMouseButtonDown(2)) {
			// rotate camera
		}
	}

	private Vector3 RaycastForBlock() {
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit, 100.0f)) {
			Block block = hit.transform.GetComponent<Block>();

			if (block != null) {
				marker.transform.position = block.transform.position + (hit.normal * 0.52f);
				marker.transform.rotation = Quaternion.LookRotation(hit.normal);

				focus = block;
				return hit.normal;
			} else {
				marker.transform.position = new Vector3(0.0f, 1000.0f, 0.0f);
			}
		}

		focus = null;
		return Vector3.zero;
	}
}
