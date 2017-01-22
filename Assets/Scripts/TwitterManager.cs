// Example setup script for Streamer
using System;
using Twitter;
using System.Threading;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;

public class TwitterManager : MonoBehaviour
{
	public delegate void TweetEventHandler(Tweet tweet);
	public event TweetEventHandler OnTweet;

	public TwitterAccess access { get; private set; }

	Coroutine poller;

    void Awake() {
        access = new TwitterAccess();
		poller = StartCoroutine(pollForTweets());

        DontDestroyOnLoad(gameObject);
    }

	void OnDestroy() {
		if (poller != null) {
			StopCoroutine(poller);
		}
		access.Disconnect();
		access.tweetParser.Stop();
	}

	public void OpenAuthUrl() {
		access.GetOAuthURL(url => System.Diagnostics.Process.Start(url));
	}

	public void StartAuth(string pin) {
		access.GetUserTokens(pin);
	}

	public void Connect() {
		access.AddQueryParameter(new Twitter.QueryTrack(access.screenName));
		access.Connect(false);
		Debug.Log("Listening for tweets to @" + access.screenName);
	}

	public bool LoadCredentials() {
		if (File.Exists(credentialsFile())) {
			var bf = new BinaryFormatter();
			var file = File.Open(credentialsFile(), FileMode.Open);
			var auth = (UserAuth)bf.Deserialize(file);
			file.Close();

			return access.LoadCredentials(auth);
		}

		return false;
	}

	public void SaveCredentials() {
		var bf = new BinaryFormatter();
		var file = File.Create(credentialsFile());
		bf.Serialize(file, access.GetCredentials());
		file.Close();
	}

	public void DestroySavedCredentials() {
		if (File.Exists(credentialsFile())) {
			File.Delete(credentialsFile());
			access.Disconnect();
		}
	}

	string credentialsFile() {
		return Application.persistentDataPath + "/credentials.gd";
	}

	IEnumerator pollForTweets() {
		while (true) {
			if (access.IsOAuthed() && access.tweets.Count > 0) {
				Tweet newTweet = access.tweets.Dequeue();
				if (newTweet.status.Contains("@" + access.screenName) && newTweet.status.ToLower().Contains("#tisle") && OnTweet != null) {
					Debug.Log("TWEET!");
					OnTweet(newTweet);
				}
			}

			yield return new WaitForEndOfFrame();
		}
	}
}
