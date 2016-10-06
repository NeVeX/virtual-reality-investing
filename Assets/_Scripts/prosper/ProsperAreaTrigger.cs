using UnityEngine;
using System.Collections;

public class ProsperAreaTrigger : MonoBehaviour
{
	private SpeechWithATT speech;
	
	void Start()
	{
		speech = SpeechWithATT.Instance;
	}
	
	void OnTriggerEnter(Collider otherCollider)
	{
		if (otherCollider.gameObject.tag.Equals ("MainCamera")) {
			ProsperListingAreaState.IsPlayerInRange = true;
			Debug.Log ("Entered the Prosper trigger area");
			if (ProsperListingAreaState.IsPlayerEnterForFirstTime) {
				ProsperListingAreaState.IsPlayerEnterForFirstTime = false;
				speech.ConvertTextToSpeech ("Welcome to the Prosper Investment Portal. Let's do some investing.");
			}
		}
	}
	
	void OnTriggerExit(Collider otherCollider)
	{
		if (otherCollider.gameObject.tag.Equals ("MainCamera")) {
			ProsperListingAreaState.IsPlayerInRange = false;
			Debug.Log ("Leaving the Prosper trigger area");
			if (ProsperListingAreaState.IsPlayerExitingForFirstTime) {
				ProsperListingAreaState.IsPlayerExitingForFirstTime = false;
				speech.ConvertTextToSpeech ("Are you leaving? Ok, Prosper will be here waiting for you, to help make you lots of money.");
			}
		}
	}
}

