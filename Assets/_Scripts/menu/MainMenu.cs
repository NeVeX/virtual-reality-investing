using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
	public InputField nameText;
	public InputField userNameText;
	public InputField passwordText;
	public Text accountCheckText;
	public ProsperAccountAPI accountAPI;
	private SpeechWithATT speech;
	private bool shouldPlayAfterAccountValidated = false;

	void Start()
	{
		speech = SpeechWithATT.Instance;
	}
	
	public void ValidateAccount()
	{
		Debug.Log ("Validating user account");
		// get all the info
		GameState.PLAYER_NAME = nameText.text;
		string userName = userNameText.text;
		string password = passwordText.text;
		accountCheckText.text = "Checking...";
		speech.ConvertTextToSpeech ("Checking if I can authenticate you against Prosper's services");
		ProsperAPIConfiguration.SetAuthorizationString (userName, password);
		accountAPI.CheckAccountInformationValid (userName, password);
		Invoke ("CheckIfAuthValid", 2);
	}

	private void CheckIfAuthValid()
	{
		if (GameState.API_AUTH_SUCCESS) {
			accountCheckText.text = "Ok";
			speech.ConvertTextToSpeech("You are authenticated.");
			if ( shouldPlayAfterAccountValidated)
			{
				Invoke ("StartGameNow", 2);
			}
		} else {
			accountCheckText.text = "Nope";
			speech.ConvertTextToSpeech("I could not authenticate you, please make sure your credentials are correct.");
		}
		shouldPlayAfterAccountValidated = false;
	}

	public void LoadTestUser()
	{
		Debug.Log ("Loading user as Test");
		ProsperAPIConfiguration.SetTestAuthorizationString ();
		GameState.IN_TEST_USER = true;
		speech.ConvertTextToSpeech ("Test User has being loaded");
	}


	public void StartGame()
	{
		if (!GameState.IN_TEST_USER) {
			shouldPlayAfterAccountValidated = true;
			ValidateAccount ();
		} else {
			StartGameNow();
		}
	}

	private void StartGameNow()
	{
		// no validation needed
		Debug.Log ("Starting the game");
		GameState.CAN_PLAYER_MOVE = true;
		speech.ConvertTextToSpeech ("Welcome to the Prosper Virtual Reality World, "+GameState.PLAYER_NAME);
		this.GetComponentInChildren<Canvas>().enabled = false; // remove the menu

	}


}

