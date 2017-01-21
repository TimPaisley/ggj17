using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCube : MonoBehaviour {

    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

	// Update is called once per frame
	void Update () {

        Vector3 startPoint = transform.position;

            int mask = (1 << LayerMask.NameToLayer("Land"));

            var hit = new RaycastHit();
            var ray = new Ray(startPoint, Vector3.up);
        if (Physics.Raycast(ray, out hit, 0.9f, mask))
        {
            meshRenderer.enabled = false;
        }
        else {
            meshRenderer.enabled = true;
        }  
    }
}
