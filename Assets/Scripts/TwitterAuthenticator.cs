using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwitterAuthenticator : MonoBehaviour {
    [Header("UI")]
    public Text pinText;
    public RectTransform errorMessage;
    public RectTransform loadingIndicator;

    [Header("Auth")]
    public TwitterManager twitterManager;
    public int authTimeout = 10;

    void Awake() {
        errorMessage.gameObject.SetActive(false);
        loadingIndicator.gameObject.SetActive(false);
    }

    public void OpenAuthorizationUrl() {
        twitterManager.access.GetOAuthURL(openInBrowser);
    }

    public void AttemptPINAuthorization() {
        string pin = pinText.text;
        int parseTest;

        if (!int.TryParse(pin, out parseTest)) {
            setError("Get your PIN first, then type in the number.");
            return;
        }

        setError(null);

        StartCoroutine(attemptAuth(pin));
    }

    void openInBrowser(string url) {
        System.Diagnostics.Process.Start(url);
    }

    IEnumerator attemptAuth(string pin) {
        float time = 0f;

        loadingIndicator.gameObject.SetActive(true);

        twitterManager.access.GetUserTokens(pin);

        while (time < authTimeout && !twitterManager.access.IsOAuthed()) {
            yield return new WaitForSeconds(0.5f);
            Debug.Log("Waiting for auth ...");
            time += 0.5f;
        }

        loadingIndicator.gameObject.SetActive(false);

        if (twitterManager.access.IsOAuthed()) {
            Debug.Log("Success! Authed!");
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
