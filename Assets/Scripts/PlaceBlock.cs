using UnityEngine;
using System.Collections;

public class PlaceBlock : MonoBehaviour {

	public GameObject placementBlock;

	public GameObject sandBlock;
	public GameObject dirtBlock;
	public GameObject grassBlock;
    
	void Start () {
	
	}
	
	void Update () {
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit, 100.0f)) {
			if (hit.transform.tag == "Block") {
				placementBlock.SetActive(true);
				placementBlock.transform.position = hit.transform.position + hit.normal;
			} else {
				placementBlock.SetActive(false);
			}
		} else {
			placementBlock.SetActive(false);
		}

		if (Input.GetMouseButtonDown(0)) {
			if (placementBlock.activeSelf) {
				Instantiate(sandBlock, placementBlock.transform.position, Quaternion.identity);
			}
		}
	}
}
