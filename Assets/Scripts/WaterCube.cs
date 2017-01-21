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
		Vector3 startPoint = transform.position+new Vector3(0,-0.5f,0);

		int mask = (1 << LayerMask.NameToLayer("Land"));

		var hit = new RaycastHit();
		var ray = new Ray(startPoint, Vector3.up);

		return Physics.Raycast(ray, out hit, 1, mask);

//		if (Physics.Raycast(ray, out hit, 1, mask)) {
//			meshRenderer.enabled = false;
//		}
//		else {
//			meshRenderer.enabled = true;
//		}
	}

	// Update is called once per frame
	void Update () {
		if (checker.IsUnderLand(transform.localPosition.x, transform.localPosition.z)) {
			meshRenderer.enabled = false;
		} else {
			meshRenderer.enabled = true;
		}
//
//        Vector3 startPoint = transform.position+new Vector3(0,-0.5f,0);
//
//            int mask = (1 << LayerMask.NameToLayer("Land"));
//
//            var hit = new RaycastHit();
//            var ray = new Ray(startPoint, Vector3.up);
//        if (Physics.Raycast(ray, out hit, 1, mask))
//        {
//            meshRenderer.enabled = false;
//        }
//        else {
//            meshRenderer.enabled = true;
//        }  
    }
}
