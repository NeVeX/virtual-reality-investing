using UnityEngine;
using System.Collections;
using API.Prosper;
using UnityEngine.UI;
using System.Collections.Generic;

public class ProsperListingControl : MonoBehaviour
{
	public Canvas[] canvases;
	public Canvas dynamicCanvas;
//	private bool showCanvas;
	public ProsperListingVO ListingVO;
	private SpeechWithATT speech;
	public Text ListingPurposeTextBox;
	public Text ListingNumberTextBox;
	public Text ListingLoanAmountTextBox;
	public Text ListingLenderYieldTextBox;
	public Text ListingInquiresLast6MonthsTextBox;
	public Text ListingStateTextBox;
	public Text ListingOccupationTextBox;
	public Text ListingProsperRatingTextBox;
	public Text ListingTermRatingTextBox;
	public Text ListingPercentFundedRatioTextBox;
	
	public static List<GameObject> InactiveGameObjects = new List<GameObject> ();

	public static void DestroyAllListingObjects()
	{
		Debug.Log ("Destroying all listing objects and not saving them");

		if (ProsperListingAreaState.CurrentProsperListingShowing != null) {
			ProsperListingAreaState.CurrentProsperListingShowing.ControlCanvas (false);
		}

		GameObject[] array = GameObject.FindGameObjectsWithTag ("LISTING_PERSON");
		if ( array != null)
		{
			foreach ( GameObject go in array)
			{
				Destroy(go);
			}
		}
		if ( InactiveGameObjects != null)
		{
			foreach ( GameObject go in InactiveGameObjects)
			{
				Destroy(go);
			}
			InactiveGameObjects.Clear();
		}

	}

	void Awake()
	{
		ControlCanvas (false);
	}

	void Start()
	{
		speech = SpeechWithATT.Instance;
	}

	public void ShowCanvas()
	{
		// show this players canvas
		Debug.Log ("Enabling canvas for this listing person");
		if (ProsperListingAreaState.CurrentProsperListingShowing != null) {
			ProsperListingAreaState.CurrentProsperListingShowing.ControlCanvas(false);
		}

		ProsperListingAreaState.CurrentProsperListingShowing = this;
		ListingPurposeTextBox.text = ListingVO.ListingTitle;
		ListingNumberTextBox.text = "#"+ListingVO.ListingNumber;

		float amount = ListingVO.ListingRequestAmount;
		ListingLoanAmountTextBox.text = "$"+amount.ToString("#,##0.00");

		float yieldReturn = ListingVO.EffectiveYield * 100;
		ListingLenderYieldTextBox.text = ""+yieldReturn.ToString("#,##0.00")+"%";
	
		float percentFunded = ListingVO.PercentFunded * 100;
		ListingPercentFundedRatioTextBox.text = "" + percentFunded.ToString ("#0") + "%";

		ListingInquiresLast6MonthsTextBox.text = ""+ListingVO.InquiriesLast6Months;
		ListingStateTextBox.text = ListingVO.BorrowerState;
		ListingOccupationTextBox.text = ListingVO.Occupation;
		ListingProsperRatingTextBox.text = ListingVO.ProsperRating;
		ListingTermRatingTextBox.text = ""+(ListingVO.ListingTerm / 12);

		// determine the speech
		string speak = "The borrower's occupation is " + ListingVO.Occupation;
		speech.ConvertTextToSpeech (speak);
		if (ListingVO.PercentFunded >= 0.7) {
			speak = "The listing is " + ListingPercentFundedRatioTextBox.text + " funded, this will originate soon.";
		} else if (ListingVO.PercentFunded >= 0.4) {
			speak = "The listing is " + ListingPercentFundedRatioTextBox.text + " funded, it's not there yet, but we can help it get funded too.";
		} else {
			speak = "The listing is " + ListingPercentFundedRatioTextBox.text + " funded, still got a ways to go, maybe we can help.";
		}
		speech.ConvertTextToSpeech (speak);

		if (ListingVO.ListingRequestAmount > 10000) {
			speak = "The borrower wants a good chunk of money here";
			speech.ConvertTextToSpeech (speak);
		}
		speak = "The requested loan amount is " + ListingLoanAmountTextBox.text + ".";
		speech.ConvertTextToSpeech (speak);

		if (ListingVO.EffectiveYield >= 0.1f) {
			speak = "Oh my god "+GameState.PLAYER_NAME+". The yield is " + ListingLenderYieldTextBox.text + ", that is amazing!";
		} else {
			speak = "The yield for you is " + ListingLenderYieldTextBox.text + ", there's money to be made here "+GameState.PLAYER_NAME;
		}
		speech.ConvertTextToSpeech (speak);
		speak = "The listing title is " + ListingVO.ListingTitle + ".";
		speech.ConvertTextToSpeech (speak);
		speak = "The term of the loan is for " + ListingTermRatingTextBox.text + " years.";
		speech.ConvertTextToSpeech (speak);
		RemoveOtherListingPeople (true);
		ControlCanvas (true);
		Debug.Log ("Enabled Canvas for player listing");
	}

	public void RemoveOtherListingPeople(bool saveGameObjects)
	{
		Debug.Log ("Disabling all the listing people");
		ControlListingPeople (false, saveGameObjects);
	}

	public void AddAllListingPeople()
	{
		Debug.Log ("Enabling all the listing people");
		speech.CancelAll ();
		// remove all other listings for now
		ControlListingPeople (true, true);
		dynamicCanvas.enabled = false;
		if (ProsperListingAreaState.CurrentProsperListingShowing != null) {
			ProsperListingAreaState.CurrentProsperListingShowing.ControlCanvas(false);
		}
		ProsperListingAreaState.CurrentProsperListingShowing = null;
	}

	private void ControlListingPeople(bool enable, bool saveGameObjects)
	{
		if (enable) {
			if (InactiveGameObjects != null) {
				foreach (GameObject go in InactiveGameObjects) {
					go.SetActive (true);
				}
				InactiveGameObjects.Clear();
			}
		} else {
			// remove all other listings for now
			GameObject[] array = GameObject.FindGameObjectsWithTag ("LISTING_PERSON");
			if (array != null) {
				Debug.Log ("Setting [" + InactiveGameObjects.ToArray().Length + "] player listings to " + enable);
				foreach (GameObject go in array) {
					if (go.GetComponentInParent<ProsperListingControl> () != this) {
						if ( saveGameObjects )
						{
							InactiveGameObjects.Add(go);
						}
						dynamicCanvas.enabled = false;
						go.SetActive (false);
					}
				}
			}
		}
	}



	public void ControlCanvas(bool enable)
	{
		if (canvases != null) {
			foreach (Canvas c in canvases)
			{
				c.enabled = enable;
			}
		}
	}




}

