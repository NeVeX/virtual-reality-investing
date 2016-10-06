using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using API.Prosper;
using Json;
using System.Reflection;

public class ProsperListingAPI : MonoBehaviour {

	public ProsperListingModelsFactory modelFactory;

//	void Awake()
//	{
//		ListingFilterVO f = new ListingFilterVO ();
//		f.ReturnYieldAbove = "100.00";
//		f.ReturnYieldBelow = "1.00";
//		f.PercentFundedAbove = "100.00";
//		f.PercentFundedBelow = "1.04";
//		GetListingInformation (f);
//	}

	public void GetListingInformation(ListingFilterVO filterVO)
	{
		StartCoroutine (CallProsperListingAPI(filterVO));
	}
	
	IEnumerator CallProsperListingAPI(ListingFilterVO filterVO) {
		Debug.Log ("Calling Prosper Listing API");

		string url = ProsperAPIConfiguration.ListingAPIUrl + "?$filter="+getFilterEncodedString(filterVO)+"&$top=5";
		WWW www = new WWW (url, null, ProsperAPIConfiguration.CreateProsperAPIHeader());
		yield return www;
		Debug.Log ("Received response from Prosper Listing API: \n"+www.text);
		if (www.isDone && string.IsNullOrEmpty(www.error)) {
			List<ProsperListingVO> listOfListings = JsonConverter.ConvertToList<ProsperListingVO> (www.text);
			Debug.Log ("Received listing data in response:\n" + listOfListings.ToString ());
			modelFactory.setAPIListings (listOfListings);
			Debug.Log ("Received response from Prosper Listing API:\n" + listOfListings.ToString ());
		} else {
			Debug.Log ("Received no data in response from listings service");
		}
	}

	private string getFilterEncodedString(ListingFilterVO filterVO)
	{
		string filterString = "";
		if ( !string.IsNullOrEmpty(filterVO.ProsperRating))
		{
			filterString += "ProsperRating eq '"+filterVO.ProsperRating+"' and ";
		}
		if ( !string.IsNullOrEmpty(filterVO.ReturnYieldAbove))
		{
			filterString += "EffectiveYield lt "+(float.Parse(filterVO.ReturnYieldAbove)/100)+"M and ";
		}
		if ( !string.IsNullOrEmpty(filterVO.ReturnYieldBelow))
		{
			filterString += "EffectiveYield gt "+(float.Parse(filterVO.ReturnYieldBelow)/100)+"M and ";
		}
		if ( !string.IsNullOrEmpty(filterVO.PercentFundedAbove))
		{
			filterString += "PercentFunded lt "+(float.Parse(filterVO.PercentFundedAbove)/100)+"M and ";
		}
		if ( !string.IsNullOrEmpty(filterVO.PercentFundedBelow))
		{
			filterString += "PercentFunded gt "+(float.Parse(filterVO.PercentFundedBelow)/100)+"M and ";
		}
		if ( !string.IsNullOrEmpty(filterVO.LoanTerm))
		{
			filterString += "ListingTerm eq "+filterVO.LoanTerm+" and ";
		}
		if ( !string.IsNullOrEmpty(filterVO.InquiriesLast6MonthsAbove))
		{
			filterString += "InquiriesLast6Months lt "+filterVO.InquiriesLast6MonthsAbove+" and ";
		}
		if ( !string.IsNullOrEmpty(filterVO.InquiriesLast6MonthsBelow))
		{
			filterString += "InquiriesLast6Months gt "+filterVO.InquiriesLast6MonthsBelow+" and ";
		}

		if (string.IsNullOrEmpty (filterString)) {
			return "";
		}
		filterString = filterString.Substring (0, filterString.Length - 5);
		Debug.Log ("Filter string for listing service is: " + filterString);
		string encoded = WWW.EscapeURL(filterString);
		Debug.Log("Encoded string is: "+encoded);
		return encoded;
	}


}
