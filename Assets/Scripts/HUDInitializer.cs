using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDInitializer : MonoBehaviour {
	public Text handleAndHashtag;

	void Start () {
		var twitterManager = FindObjectOfType<TwitterManager>();
		if (twitterManager != null) {
			handleAndHashtag.text = "@" + twitterManager.access.screenName + " #Tisle";
		}
	}
}
