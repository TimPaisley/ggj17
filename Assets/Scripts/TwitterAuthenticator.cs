using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class TwitterAuthenticator : MonoBehaviour {
    [Header("UI")]
    public Text pinText;
    public RectTransform errorMessage;
    public RectTransform loadingIndicator;
	public RectTransform notAuthedPanel;
	public RectTransform authedPanel;
	public Text authedText;

    [Header("Auth")]
    public TwitterManager twitterManager;
    public int authTimeout = 10;

    void Awake() {
        errorMessage.gameObject.SetActive(false);
        loadingIndicator.gameObject.SetActive(false);
    }

	void Start() {
		if (twitterManager.LoadCredentials()) {
			notAuthedPanel.gameObject.SetActive(false);
			authedPanel.gameObject.SetActive(true);
			authedText.text = "You're logged in as @" + twitterManager.access.screenName + "!";
		} else {
			notAuthedPanel.gameObject.SetActive(true);
			authedPanel.gameObject.SetActive(false);
		}
	}

	public void Logout() {
		twitterManager.Disconnect();
		twitterManager.DestroySavedCredentials();
		notAuthedPanel.gameObject.SetActive(true);
		authedPanel.gameObject.SetActive(false);
	}

    public void OpenAuthorizationUrl() {
		twitterManager.OpenAuthUrl();
    }

    public void AttemptPINAuthorization() {
        string pin = pinText.text;
        int parseTest;

        if (!int.TryParse(pin, out parseTest)) {
            setError("Get your PIN first, then type in the number.");
            return;
        }

        setError(null);

        StartCoroutine(doAuth(pin));
    }

	public void StartGame() {
		twitterManager.StartListening();
		SceneManager.LoadScene("UITesting");
	}

	IEnumerator doAuth(string pin) {
        float time = 0f;

		twitterManager.StartAuth(pin);

        loadingIndicator.gameObject.SetActive(true);

        while (time < authTimeout && !twitterManager.access.IsOAuthed()) {
            yield return new WaitForSeconds(0.5f);
            Debug.Log("Waiting for auth ...");
            time += 0.5f;
        }

		twitterManager.SaveCredentials();

        loadingIndicator.gameObject.SetActive(false);

        if (twitterManager.access.IsOAuthed()) {
			StartGame();
        } else {
            setError("Unable to authenticate: Timed out.");
        }
    }



    void setError(string error) {
        if (string.IsNullOrEmpty(error)) {
            errorMessage.gameObject.SetActive(false);
        } else {
            errorMessage.gameObject.SetActive(true);
            errorMessage.GetComponentInChildren<Text>().text = error;
        }
    }
}
