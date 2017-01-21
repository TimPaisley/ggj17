// Example setup script for Streamer
using System;
using Twitter;
using System.Threading;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class TwitterManager : MonoBehaviour
{
	public TwitterAccess access { get; private set; }

    void Awake() {
        access = new TwitterAccess();
        DontDestroyOnLoad(gameObject);
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
			access.Disconnect();
		}
	}

	string credentialsFile() {
		return Application.persistentDataPath + "/credentials.gd";
	}
	
	//public void Main ( string[] args )
	//{
	//	// Create twitteraccess object
	//	access = new TwitterAccess();
		
	//	// Request authorisation key
	//	access.GetOAuthURL( OnReceiveAuthURL );
	
	//	// Get all tweets with a location tag
	//	QueryLocations world = new QueryLocations( new Coordinates( -180f, -90f ), new Coordinates( 180f, 90f ) );
	//	access.AddQueryParameter( world );
		
	//	// and with the word "bieber" in them
	//	access.AddQueryParameter( new QueryTrack( "bieber" ) );
	//}
	
	//static void OnReceiveAuthURL( string url )
	//{
	//	// redirect usert to given URL to authorise this app
	//	System.Diagnostics.Process.Start ( url );
		
	//	Debug.Log ( "Please enter passcode from authorisation URL: " );
		
	//	string passcode = "";
	//	int testParse = 0;
	//	passcode = Console.ReadLine();
		
	//	while ( !int.TryParse ( passcode, out testParse ) )
	//	{
	//		Debug.Log ( "Sorry, invalid passcode, please re-enter: " );
	//		passcode = Console.ReadLine();
	//	}
		
	//	// use given passcode to get signing pair for all requests
	//	access.GetUserTokens ( passcode );
		
	//	while ( !access.IsOAuthed () )
	//	{
	//		Thread.Sleep( 500 );
	//		Console.Write ( "." );
	//	}
		
	//	Debug.Log ( "Authorised...connecting..." );
	//	// Is authorised, can now connect
	//	access.Connect ( false );
		
	//	while ( true )
	//	{
	//		if ( access.tweets.Count > 0 )
	//		{
	//			Tweet newTweet = access.tweets.Dequeue();
	//			Debug.Log( newTweet.userName  + ": " + newTweet.status );
	//		}
	//	}
		
	//	// SHOULD ALWAYS CALL DISCONNECT on access object. THIS DOESN"T!!!
	//}
}

