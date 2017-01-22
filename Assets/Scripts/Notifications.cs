using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notifications : MonoBehaviour {

	public Text notification;
	private Text[] notifications;
	private TwitterManager tm;

	void Start () {
		tm = FindObjectOfType<TwitterManager>();
		tm.OnTweet += HandleNotification;
	}

	private void HandleNotification(Twitter.Tweet tweet) {
		StartCoroutine(CreateNotification(tweet));
	}

	private IEnumerator CreateNotification(Twitter.Tweet tweet) {
		Text textui = Instantiate(notification);
		textui.text = tweet.userName + " has sent you a bottle!";
		textui.transform.SetParent(this.transform, false);
		Debug.Log(tweet.userName + " has sent you a bottle!");
		yield return new WaitForSeconds(5);
		GameObject.Destroy(textui);
	}

	public void OnDestroy() {
		tm.OnTweet -= HandleNotification;
	}
}
