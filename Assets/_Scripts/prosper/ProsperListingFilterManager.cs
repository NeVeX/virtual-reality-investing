using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProsperListingFilterManager : MonoBehaviour
{
	// hack for now
	public static ListingFilterVO FilterVO = new ListingFilterVO ();
	//public Canvas[] canvasFilters;
	public ProsperListingAPI listingAPI;
	private int currentCanvasIndex = -1;
	public Text returnYieldBelow; 
	public Text returnYieldAbove; 
	public Text percentFundedBelow; 
	public Text percentFundedAbove; 
	public Text inquiriesLast6MonthsBelow; 
	public Text inquiriesLast6MonthsAbove; 
	public Text TermSelectedText;
	public Text ProsperRatingSelectedText;

	private SpeechWithATT speech;

	void Start()
	{
		speech = SpeechWithATT.Instance;
	}

	public void SearchListings()
	{
		speech.CancelAll ();
		speech.ConvertTextToSpeech ("Searching for listings");

		if ( ProsperListingAreaState.NoListingsFoundCanvas != null)
		{
			ProsperListingAreaState.NoListingsFoundCanvas.enabled = false;
		}

		FilterVO.ReturnYieldAbove = returnYieldAbove.text;
		FilterVO.ReturnYieldBelow = returnYieldBelow.text;

		FilterVO.PercentFundedAbove = percentFundedAbove.text;
		FilterVO.PercentFundedBelow = percentFundedBelow.text;

		FilterVO.InquiriesLast6MonthsAbove = inquiriesLast6MonthsAbove.text;
		FilterVO.InquiriesLast6MonthsBelow = inquiriesLast6MonthsBelow.text;

		// need to disable any listings we have currently showing
		ProsperListingControl.DestroyAllListingObjects ();

		listingAPI.GetListingInformation (FilterVO);
	}

	public void OnProsperRatingClick(string rating)
	{
		Debug.Log ("Choosen rating is: " + rating);
		if (!string.IsNullOrEmpty (rating)) {
			if ( rating.Equals(FilterVO.ProsperRating))
			{
				// the same selection, so null it out
				rating = null;
			}
		}
		FilterVO.ProsperRating = rating;
		string textToShow = "";
		if ( !string.IsNullOrEmpty(FilterVO.ProsperRating))
		{
			 textToShow = "("+FilterVO.ProsperRating+")";
		}
		ProsperRatingSelectedText.text = textToShow;
	}

	public void OnLoanTermClick(string term)
	{
		Debug.Log ("Choosen term is: " + term);
		if (!string.IsNullOrEmpty (term)) {
			if ( term.Equals(FilterVO.LoanTerm))
			{
				// the same selection, so null it out
				term = null;
			}
		}
		FilterVO.LoanTerm = term;
		string textToShow = "";
		if ( !string.IsNullOrEmpty(FilterVO.LoanTerm))
		{
			textToShow = "("+FilterVO.LoanTerm+")";
		}
		TermSelectedText.text = textToShow;
	}

}

