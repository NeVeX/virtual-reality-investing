using UnityEngine;
using System.Collections;
using API.Prosper;

public class InvestmentController : MonoBehaviour
{
	public Canvas InvestmentOptionCanvas;
	private ProsperListingControl ListingControl;
	private SpeechWithATT speech;
	public ProsperInvestAPI investAPI;

	void Awake()
	{
		InvestmentOptionCanvas.enabled = false;
		speech = SpeechWithATT.Instance;
		ListingControl = this.GetComponentInParent<ProsperListingControl> ();
	}

	// Use this for initialization
	void Start ()
	{
		InvestmentOptionCanvas.enabled = false;
	}

	void Enable() {
		//Debug.Log ("in enable");
		InvestmentOptionCanvas.enabled = false;
	}

	void OnEnable() {
		//Debug.Log ("in onenable");
		InvestmentOptionCanvas.enabled = false;
	}
	     
	void OnDisable() {
		//Debug.Log ("in on disable");
		InvestmentOptionCanvas.enabled = false;
	}
	
	public void ShowInvestmentOptionCanvas()
	{
		speech.CancelAll ();
		speech.ConvertTextToSpeech ("Are you sure you want to invest $25 in this listing "+GameState.PLAYER_NAME+"?");
		Invoke ("EnableInvestCanvas", 5.5f);
	}

	private void EnableInvestCanvas()
	{
		InvestmentOptionCanvas.enabled = true;
	}

	public void DoInvest()
	{
		ListingControl.RemoveOtherListingPeople (false);
		speech.ConvertTextToSpeech ("An investment order of $25 dollars has being placed on your behalf, "+GameState.PLAYER_NAME+". Now awaiting confirmation");//("An investment has being placed on your behalf "+GameState.PLAYER_NAME+". Now, sit back, relax, and watch the money roll in.");
		// get the data
		ProsperListingVO listingVo = ListingControl.ListingVO;
		// set to 25 dollars
		int amount = 25;
		int listingId = listingVo.ListingNumber;
		Debug.Log ("About to invest ["+amount+"] in listing ["+listingId+"]");
		if (!GameState.IN_TEST_USER) {
			investAPI.Invest (listingId, amount);
			Invoke("CheckInvestmentWasSuccessful", 5);
		}
	}

	private void CheckInvestmentWasSuccessful()
	{
		bool investSuccess = investAPI.WasLastInvestmentSuccessful ();
		if (investSuccess) {
			speech.ConvertTextToSpeech("Congratulations "+GameState.PLAYER_NAME+". The investment order was placed succesfully");
		} else {
			speech.ConvertTextToSpeech(GameState.PLAYER_NAME+ ", It looks like there was a problem with that investment order. Please try again later");
		}
	}

}

