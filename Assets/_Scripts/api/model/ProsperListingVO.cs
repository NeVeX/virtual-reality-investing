using UnityEngine;


namespace API.Prosper
{
	public class ProsperListingVO
	{
		//public string MemberKey;
		public int ListingNumber;//int
		//public int LoanNumber; //int
		//public string ListingStartDate;
		//public string ListingEndDate;
		//public string LoanOriginationDate;
		//public int ListingStatus;//int
		//public int VerificationStage;//int
		public string ProsperRating;
		//public int InvestmentTypeID;//int
		//public float DTIwProsperLoan;//float
		public float ListingRequestAmount;//float
		//public string FICOScore;
//		public int ProsperScore;//int

		public string ListingTitle;
		public int ListingTerm;
		public float EffectiveYield;
		public string BorrowerState;
		public string Occupation;
		public int InquiriesLast6Months;
		public float PercentFunded;

	}
}

