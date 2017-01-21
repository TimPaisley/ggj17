using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {

	private GameManager gm;
	private Renderer rend;

	public enum BlockType { Sand, Dirt, Rock };
	public BlockType type;

	public bool isMoveable = true;

	public Material transparent;
	private Material original;
	private Shader originalShader;

	private bool isFocusing=false;

	void Start () {
		gm = FindObjectOfType<GameManager>();
		rend = GetComponent<Renderer>();

		if (type == BlockType.Sand) {
			rend.material = gm.sand;
		} else if (type == BlockType.Dirt) {
			rend.material = gm.dirt;
		} else if (type == BlockType.Rock) {
			rend.material = gm.rock;
		}

		original = rend.material;
		originalShader = rend.material.shader;
	}

	void Update() {
		if (isFocusing) {
			rend.material.shader = originalShader;
		}
	}

	public void Focusing() {
		isFocusing = true;
		rend.material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
	}

	public void Shifting() {
		isFocusing = false;
		rend.material.shader = originalShader;
		Color col = original.color;
		rend.material = transparent;
		rend.material.color = new Color(col.r, col.g, col.b, 0.5f);
		GetComponent<BoxCollider>().enabled = false;
	}

	public void ResetAll() {
		rend.material = original;
		GetComponent<BoxCollider>().enabled = true;
	}
}
