using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using API.Prosper;
using Json;
using System.Reflection;

public class ProsperInvestAPI : MonoBehaviour {

	private bool lastInvestmentSuccessful = false;
	private bool orderStillPendingResult = false;

	public void Invest(int listingId, int amount)
	{
		StartCoroutine (CallInvestAPI(listingId, amount));
	}
	
	IEnumerator CallInvestAPI(int listingId, int amount) {
		if (!orderStillPendingResult) {
			lastInvestmentSuccessful = false;
			orderStillPendingResult = true;
			Debug.Log ("Calling Prosper Invest API - investing $" + amount + " in listing " + listingId);
			Dictionary<string, string> headers = ProsperAPIConfiguration.CreateProsperAPIHeader ();
			headers.Add ("Content-Type", "application/json");
			string url = ProsperAPIConfiguration.InvestAPIUrl;
			// create simple json -- 'listingId' and 'amount'
			string jsonBody = "{ listingId: " + listingId + ", amount: " + amount + "}";
			Debug.Log ("Investment post body json: \n" + jsonBody);
			WWW www = new WWW (url, System.Text.Encoding.UTF8.GetBytes (jsonBody), headers);
			yield return www;
			Debug.Log ("Received response from from Prosper Invest API\n" + www.text);
			// a succesful bid looks like this, so check for it
			// {"Status":"SUCCESS","Message":"NO_ERROR","ListingId":3286957,"RequestedAmount":"25","AmountInvested":"25"}
			lastInvestmentSuccessful = false;
			if ( www.isDone && string.IsNullOrEmpty(www.error) && !string.IsNullOrEmpty(www.text))
			{
				string lowerCaseResponse = www.text.ToLower();
				if ( lowerCaseResponse.Contains("success") && lowerCaseResponse.Contains("no_error"))
				{
					lastInvestmentSuccessful = true;
				}
			}
			orderStillPendingResult = false;
		}
	}

	public bool WasLastInvestmentSuccessful()
	{
		return lastInvestmentSuccessful;
	}
}
