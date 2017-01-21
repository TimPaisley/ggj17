using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMaker : MonoBehaviour {

    public GameObject platformType;
    public int size = 50;
    public float scale = 7.00f;
    public bool enableHeight = false;
    public float scaleModifier = 5f;
    private bool exists = false;

	// Use this for initialization
	public void Start () {

        for (var x = 0;x<size;x++) {
            for (var z=0;z<size;z++) {
                var c = Instantiate(platformType,new Vector3(transform.position.x+x,0,transform.position.z+z),Quaternion.identity)as GameObject;
                c.transform.parent = transform;
            }
        }
        
	}

    private void Update()
    {
        UpdateTransform();
    }


    void UpdateTransform() {

        foreach (Transform child in transform) {
            var height = Mathf.PerlinNoise(child.transform.position.x / scale,child.transform.position.z / scale);
            SetMatColor(child,height);
            if (enableHeight) {
                ApplyHeight(child, height);
            }    
        }
    }
    void SetMatColor(Transform child,float height) {
        child.GetComponent<Renderer>().material.color = new Color(height,height,height);

    }

    void ApplyHeight(Transform child,float height) {
        var yValue = Mathf.RoundToInt(height*scaleModifier);
        var newVec3 = new Vector3(child.transform.position.x,yValue,child.transform.position.z);
        child.transform.position = newVec3;
    }
}
