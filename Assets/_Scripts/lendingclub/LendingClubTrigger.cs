using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LendingClubTrigger : MonoBehaviour {

	private SpeechWithATT speech;

	void Start()
	{
		speech = SpeechWithATT.Instance;
	}
	
	void OnTriggerEnter(Collider otherCollider)
	{
		if (otherCollider.gameObject.tag.Equals ("MainCamera")) {
			LendingClubState.IsPlayerInRange = true;
			Debug.Log ("Entered the Lending club trigger area");
			if (LendingClubState.IsPlayerEnterForFirstTime) {
				LendingClubState.IsPlayerEnterForFirstTime = false;
				speech.ConvertTextToSpeech ("Look at the state of Lending Club. I sure wouldn't want to do business there.");
			}
		}
	}

	void OnTriggerExit(Collider otherCollider)
	{
		if (otherCollider.gameObject.tag.Equals ("MainCamera")) {
			LendingClubState.IsPlayerInRange = false;
			Debug.Log ("Leaving the Lending club trigger area");
			if (LendingClubState.IsPlayerExitingForFirstTime) {
				LendingClubState.IsPlayerExitingForFirstTime = false;
				speech.ConvertTextToSpeech ("Thank god we got away from Lending Club.");
			}
		}
	}
}