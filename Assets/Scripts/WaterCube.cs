using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCube : MonoBehaviour {

    private MeshRenderer meshRenderer;
	LandChecker checker;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
		checker = GetComponentInParent<LandChecker>();
    }

	public bool CheckUnderLand() {
		int mask = (1 << LayerMask.NameToLayer("Land"));
        
        Vector3 startPoint = transform.position+new Vector3(0,-1f,0);

		var hit = new RaycastHit();
		var ray = new Ray(startPoint, Vector3.up);

		return Physics.Raycast(ray, out hit, 1, mask);
	}

	// Update is called once per frame
	void Update () {
		if (checker.IsUnderLand(transform.localPosition.x, transform.localPosition.z)) {
			meshRenderer.enabled = false;
		} else {
			meshRenderer.enabled = true;
		}
    }
}
