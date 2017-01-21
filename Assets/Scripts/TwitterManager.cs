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
        DontDestroyOnLoad(gameObject);
    }

	void OnDestroy() {
		Disconnect();
	}

	public void OpenAuthUrl() {
		access.GetOAuthURL(url => System.Diagnostics.Process.Start(url));
	}

	public void StartAuth(string pin) {
		access.GetUserTokens(pin);
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
			Disconnect();
		}
	}

	public void StartListening() {
		poller = StartCoroutine(pollForTweets());
	}

	public void Disconnect() {
		StopCoroutine(poller);
		access.Disconnect();
	}

	string credentialsFile() {
		return Application.persistentDataPath + "/credentials.gd";
	}

	IEnumerator pollForTweets() {
		access.AddQueryParameter(new Twitter.QueryTrack(access.screenName));
		access.Connect(false);
		Debug.Log("Listening for tweets to @" + access.screenName);

		while (true) {
			if (access.tweets.Count > 0) {
				Twitter.Tweet newTweet = access.tweets.Dequeue();
				if (newTweet.status.Contains("@" + access.screenName) && OnTweet != null) {
					Debug.Log("TWEET!");
					OnTweet(newTweet);
				}
			}

			yield return new WaitForEndOfFrame();
		}
	}
}
