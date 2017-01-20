using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {

	private GameManager gm;
	private Renderer rend;

	public enum BlockType { Sand, Dirt, Rock };
	public BlockType type;

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
	}

	void Update() {

	}
}
