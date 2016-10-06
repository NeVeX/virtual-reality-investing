using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using API.Prosper;
using Json;
using System.Reflection;

public class ProsperAccountAPI : MonoBehaviour {

	public Text prosperAccountBalanceText;
	public Text prosperActiveInvestmentText;
	public Text prosperPendingInvestmentText;

	public string AccountBalancePreText;
	public string ActiveInvestmentsPreText;
	public string PendingInvestmentsPreText;

	private SpeechWithATT speech; //= new SpeechWithATT();

	void Start () {
		speech = SpeechWithATT.Instance;
		prosperAccountBalanceText.text = "";//AccountBalancePreText;
		prosperActiveInvestmentText.text = "";//ActiveInvestmentsPreText;
		prosperPendingInvestmentText.text = "";//PendingInvestmentsPreText;
	}

	public void GetProsperAccountInformation()
	{
		speech.CancelAll ();
		StartCoroutine (CallProsperAccountAPI(false));
	}

	public void CheckAccountInformationValid(string username, string password)
	{
		ProsperAPIConfiguration.SetAuthorizationString (username, password);
		CheckAccountInformationValid ();
	}

	/**
	 * this will set the static public if a valid response is obtained
	 */
	public void CheckAccountInformationValid()
	{
		StartCoroutine (CallProsperAccountAPI(true));
	}

	private ProsperAccountVO GetAccountVOFromReponse(WWW www)
	{
		if (www.isDone && www.error == null) {
			return JsonConverter.ConvertToObject<ProsperAccountVO> (www.text);
		}
		return null;
	}

	IEnumerator CallProsperAccountAPI(bool authCheck) {
		Debug.Log ("Calling Prosper Account API");
		WWW www = new WWW (ProsperAPIConfiguration.AccountAPIUrl, null, ProsperAPIConfiguration.CreateProsperAPIHeader());
		yield return www;
		ProsperAccountVO accountVO = GetAccountVOFromReponse (www);
		if (accountVO != null) {
			if (authCheck) {
				GameState.API_AUTH_SUCCESS = true;
			}
			else{
				Debug.Log ("Received response from Prosper Account API:\n" + accountVO.ToString ());
				prosperAccountBalanceText.text = AccountBalancePreText + " $" + accountVO.AvailableCashBalance;
				prosperActiveInvestmentText.text = ActiveInvestmentsPreText + " $" + accountVO.TotalAmountInvestedOnActiveNotes;
				prosperPendingInvestmentText.text = PendingInvestmentsPreText + " $" + accountVO.PendingQuickInvestOrders;
				speech.ConvertTextToSpeech (accountVO.GetAccountBalanceSpeechString ());
				speech.ConvertTextToSpeech (accountVO.GetActiveInvestmentsBalanceSpeechString ());
				speech.ConvertTextToSpeech (accountVO.GetPendingInvestmentsBalanceSpeechString ());
			}
		}
	}

}
