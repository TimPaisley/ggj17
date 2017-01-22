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
			image.color = new Color(1, 1, 1, 0);
			image.enabled = true;
		}
		foreach (var text in tweetContainer.GetComponentsInChildren<Text>()) {
			text.color = new Color(1, 1, 1, 0);
			text.enabled = true;
		}

		while (tweetText.color.a < 1) {
			yield return new WaitForEndOfFrame();

			foreach (var image in tweetContainer.GetComponentsInChildren<Image>()) {
				image.color = new Color(1, 1, 1, Mathf.Min(1, tweetText.color.a + Time.deltaTime));
				image.enabled = true;
			}
			foreach (var text in tweetContainer.GetComponentsInChildren<Text>()) {
				text.color = new Color(0.1f, 0.1f, 0.1f, Mathf.Min(1, tweetText.color.a + Time.deltaTime));
				text.enabled = true;
			}
		}

		yield return new WaitForSeconds(10);

		if (tweetText.text == tweet.status) {
			while (tweetText.color.a > 0) {
				yield return new WaitForEndOfFrame();

				foreach (var image in tweetContainer.GetComponentsInChildren<Image>()) {
					image.color = new Color(1, 1, 1, Mathf.Max(0, tweetText.color.a - Time.deltaTime));
					image.enabled = true;
				}
				foreach (var text in tweetContainer.GetComponentsInChildren<Text>()) {
					text.color = new Color(0.1f, 0.1f, 0.1f, Mathf.Max(0, tweetText.color.a - Time.deltaTime));
					text.enabled = true;
				}
			}

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
