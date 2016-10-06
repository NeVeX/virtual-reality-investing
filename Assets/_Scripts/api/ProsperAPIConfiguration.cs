
using UnityEngine;
using System.Collections.Generic;

public class ProsperAPIConfiguration {
	
	public static string AccountAPIUrl = "https://api.prosper.com/api/Account/";
	public static string ListingAPIUrl = "https://api.prosper.com/api/Listings";
	public static string InvestAPIUrl = "https://api.prosper.com/api/Invest/";

	private static string testAuthorizationString = "bWFya2N1bm5pbmdoYW05QGdtYWlsLmNvbTp0ZXI0NXRlcg==";
	private static string authorizationString = "";
	public static string UserName = "";

	public static Dictionary<string, string> CreateProsperAPIHeader()
	{
		Dictionary<string, string> headers = new Dictionary<string, string> ();
		headers.Add ("Accept", "application/json");
		string authStringToUse = authorizationString;
		if (string.IsNullOrEmpty (authStringToUse)) {
			authStringToUse = testAuthorizationString; // use mine be default
		}
		headers.Add ("Authorization", "Basic "+authStringToUse); 
		return headers;
	}

	public static void SetAuthorizationString(string username, string password)
	{
		UserName = username;
		string userAuth = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(username+":"+password));
		if (!string.IsNullOrEmpty (userAuth)) {
			authorizationString = userAuth;
		}
	}

	public static void SetTestAuthorizationString()
	{
		authorizationString = testAuthorizationString;

	}



	
}