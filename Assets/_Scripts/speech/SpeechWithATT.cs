
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using external.JSON;
using System;

public class SpeechWithATT : MonoBehaviour {

	private enum ListenFor { NAME, NOTHING }
	private ListenFor listenFor = ListenFor.NOTHING;
	public float gapBetweenPlayedClips = 1.0f; // in seconds
	public bool shouldPlayAudioClips = true;
	private String playerName;
	private string attTextToSpeechUrl = "https://api.att.com/speech/v3/textToSpeech";
	private string attSpeechToTextUrl = "https://api.att.com/speech/v3/speechToText";
	Queue<AudioClip> clipQueue = new Queue<AudioClip> ();
	public AudioSource audioSource;
	private String PlayerName;

	private static SpeechWithATT speechSingleton;
	public static SpeechWithATT Instance
	{
		get{
//			if ( speechSingleton == null)
//			{
//				Debug.Log("Creating single instance of SpeechWithATT");
//				speechSingleton = new GameObject().AddComponent<SpeechWithATT>();
//				speechSingleton.name = "SpeechWithATTSingleton";
//			}
			return speechSingleton;
		}
	}

	void Awake()
	{
		speechSingleton = this;
	}

	void Start()
	{
		audioSource.loop = false;
//		ConvertTextToSpeech ("Welcome");
	}

	void Update()
	{
		PlayNextAudioClip ();
//		if (Input.GetKeyDown(KeyCode.M)) {
//			// start recording
//			StartCoroutine(ConvertSpeechToText());
//
//		}
	}

	private bool canPlayNextClip = true;

	void PlayNextAudioClip()	{
		if (GameState.AUDIO_ENABLED) {
			if (canPlayNextClip) {
				if (!audioSource.isPlaying) {
					if (audioSource.clip != null) {
						if (audioSource.clip.loadState == AudioDataLoadState.Loaded) {
							canPlayNextClip = false;
							audioSource.Play ();
							Invoke ("RemoveAudioClip", audioSource.clip.length);
							Invoke ("EnableNextClipToPlay", audioSource.clip.length + gapBetweenPlayedClips);
							Debug.Log ("Playing audio clip");
						}
					} else {
						if (clipQueue.Count > 0) {
							Debug.Log ("Enqueuing next audio clip");
							audioSource.clip = clipQueue.Dequeue (); // assign the next clip
						}
					}
				}
			}
		}
	}


	void GetPlayerName()
	{
		listenFor = ListenFor.NAME;
		ConvertTextToSpeech("What is your name?");
	}

	private void EnableNextClipToPlay()
	{
		canPlayNextClip = true;
	}

	public void CancelAll()
	{
		if (clipQueue != null) {
			while ( clipQueue.Count > 0 && clipQueue.Dequeue() != null)
			{
				// just drain it
			}
		}
	}

	private void RemoveAudioClip()
	{
		audioSource.clip = null;
	}

	public void ConvertTextToSpeech(string text)
	{
		StartCoroutine (ConvertTextToSpeechAsync (text));
	}

	private IEnumerator ConvertTextToSpeechAsync(string text) {
		Debug.Log ("Converting text [" + text + "] into speech using ATT");
		Dictionary<string, string> headers = new Dictionary<string, string> ();
		headers.Add ("Accept", "audio/x-wav");
		headers.Add ("Content-Type", "text/plain");
		headers.Add ("Authorization", "bearer BF-ACSI~2~20150702052234~7dQBRxht2pirB0USOmssfIb9sYGoZiis");
		headers.Add ("X-SpeechContext", "BusinessSearch");
		headers.Add ("X-Arg", "VoiceName=crystal,Volume=100,Tempo=1"); //mike, crystal
		//audioSource.clip = null;
		byte[] postData = System.Text.UTF8Encoding.UTF8.GetBytes (text);

		WWW www = new WWW(attTextToSpeechUrl, postData, headers);

		yield return www;
		if (string.IsNullOrEmpty (www.error) && www.isDone) {
			AudioClip ac = www.GetAudioClip (true, false, AudioType.WAV);
			if (ac != null) {
				Debug.Log ("Got audio file from ATT request: Seconds: " + ac.length);
				clipQueue.Enqueue (ac);//audioSource.clip = ac;
			}
		}
	}
	
	private IEnumerator ConvertSpeechToText() {
		Debug.Log ("Converting Speech into text using ATT");
		if (Microphone.devices != null && Microphone.devices.Length == 1) {
			if (Microphone.IsRecording(Microphone.devices[0])) {
				Debug.Log("Already recording, will not record again for now");
				yield break;
			}
			Debug.Log ("Using index one of microphones: " + Microphone.devices[0]);
			//int minFreq, maxFreq;
			//Microphone.GetDeviceCaps(Microphone.devices[0], out minFreq, out maxFreq);
			//Debug.Log("Min and Max Frequency of microphone: "+minFreq+", "+maxFreq);
			AudioClip recording = Microphone.Start(Microphone.devices[0], false, 2, 8000);
			yield return new WaitForSeconds(2);
			// recording should be done now
			//audioSource.clip = recording;
			Debug.Log("Got a new recording!");
			StartCoroutine(SendAudioForConversion(recording));
			//SendAudioForConversion(recording);

		} else {
			Debug.Log("Cannot record sound since unknown microphone configuration found");
		}
	}

	private IEnumerator SendAudioForConversion(AudioClip ac)
	{
		Dictionary<string, string> headers = new Dictionary<string, string> ();
		headers.Add ("Accept", "application/json");
		headers.Add ("Content-Type", "audio/wav");
		headers.Add ("Authorization", "bearer BF-ACSI~2~20150702052234~7dQBRxht2pirB0USOmssfIb9sYGoZiis");
		headers.Add ("Transfer-Encoding", "chunked");
		byte[] bytesData = SavWav.Save (DateTime.Now.Ticks+"_temp.wav", ac);
		Debug.Log ("Calling ATT with sound data size: " + bytesData.Length);
		//headers.Add ("Content-Length", byteArray.Length.ToString());
		WWW www = new WWW(attSpeechToTextUrl, bytesData, headers);
		yield return www;

		Debug.Log ("Response for Speech to Text is: " + www.text);
		JSONObject jo = new JSONObject (www.text);
		string determinedText = null;
		try
		{
			 determinedText = jo.GetField("Recognition").GetField("NBest")[0].GetField("ResultText").str;
		}
		catch (Exception e)
		{
			Debug.Log ("Could not extract Speech message from JSON returned in ATT response: "+ e.Message);
		}
		Debug.Log ("We think the words spoken were: " + determinedText);

//		if (!string.IsNullOrEmpty (determinedText)) {
//			string textToSayToPlayer = null;
//			switch (listenFor) {
//			case ListenFor.NAME:
//				playerName = determinedText.Replace (".", "");
//				textToSayToPlayer = "Hi, " + playerName + "! How are you today?";
//				break;
//			}
//			listenFor = ListenFor.NOTHING;
//			if (textToSayToPlayer == null) {
//				textToSayToPlayer = "I'm sorry " + playerName + ", you said " + determinedText + ", but I'm too stupid to understand";
//			}
//			ConvertTextToSpeech (textToSayToPlayer);
//		}
	}
}
