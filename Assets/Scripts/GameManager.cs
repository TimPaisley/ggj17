using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	
	public GameObject marker;

	public Material sand;
	public Material dirt;
	public Material rock;
    public AudioSource dig;
    public AudioSource place;
	public AudioSource pickup;

	public Block treePrefab;

	private Block shift;
	private Block focus;

	LandChecker landChecker;
	
	void Start () {
		landChecker = FindObjectOfType<LandChecker>();
	}
	
	void Update () {
		RaycastForBlock();
		
		if (Input.GetMouseButtonDown(0) && focus != null) {
			Debug.Log("FOCUS");
			Debug.Log(focus);

			// Picking up block
			if (shift == null) {
				if (focus.isDecoration) {
					pickup.Play();
				} else {
					dig.Play();
				}
                
                shift = focus;
				shift.Shifting();
                
			}
			// Placing block
			else if (!focus.isDecoration) {
                place.Play();
                shift.ResetAll();
				if (shift.isTree) {
					Instantiate(treePrefab, shift.transform.position, Quaternion.identity);
					shift.gameObject.SetActive(false);
				}
				shift = null;
                
			}

			landChecker.Check();
		}
	}

	private void RaycastForBlock() {
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit, 100.0f)) {
			Block block = hit.transform.GetComponent<Block>();

			if (block != null) {
				if (shift != null) {
					shift.transform.position = block.transform.position + hit.normal;
				} else {
					block.Focusing();
				}

				focus = block;
			} else {
				if (shift != null) {
					shift.transform.position = new Vector3(0.0f, 1000.0f, 0.0f);
				}

				focus = null;
			}
		} else {
			if (shift != null) {
				shift.transform.position = new Vector3(0.0f, 1000.0f, 0.0f);
			}

			focus = null;
		}
	}
}
