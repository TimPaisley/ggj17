using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TweetBlock : Block {
	public GameObject child;
	public Twitter.Tweet tweet;

	Text tweetText;
	Text tweetSender;
	RectTransform tweetContainer;

	void Start() {
		tweetContainer = FindObjectOfType<TweetContainer>().GetComponent<RectTransform>();
		var texts = tweetContainer.GetComponentsInChildren<Text>();
		tweetText = texts[0];
		tweetSender = texts[1];
	}

	public override void Shifting() {
		foreach (var rend in GetComponentsInChildren<Renderer>()) {
			rend.enabled = false;
		}

		if (child != null) {
			foreach (var rend in child.GetComponentsInChildren<Renderer>()) {
				rend.enabled = true;
			}

			base.Shifting();
		}

		StartCoroutine(showTweetMessage());
	}

	IEnumerator showTweetMessage() {
		tweetText.text = tweet.status;
		tweetSender.text = "Kind regards, " + tweet.fullName + " (@" + tweet.userName + ")";

		foreach (var image in tweetContainer.GetComponentsInChildren<Image>()) {
			image.enabled = true;
		}
		foreach (var text in tweetContainer.GetComponentsInChildren<Text>()) {
			text.enabled = true;
		}

		yield return new WaitForSeconds(5);

		if (tweetText.text == tweet.status) {
			foreach (var image in tweetContainer.GetComponentsInChildren<Image>()) {
				image.enabled = false;
			}
			foreach (var text in tweetContainer.GetComponentsInChildren<Text>()) {
				text.enabled = false;
			}
		}

		if (child == null) {
			Destroy(gameObject);
		}
	}
}
