using UnityEngine;


namespace API.Prosper
{
	public class ProsperAccountVO
	{
		public float AvailableCashBalance;
		public float OutstandingPrincipalOnActiveNotes;
		public float PendingInvestmentsPrimaryMkt;
		public float PendingInvestmentsSecondaryMkt;
		public float PendingQuickInvestOrders;
		public float TotalAccountValue;
		public float TotalAmountInvestedOnActiveNotes;
		public float TotalPrincipalReceivedOnActiveNotes;

		public override string ToString ()
		{
			return string.Format ("[ProsperAccountVO: AvailableCashBalance={0}, OutstandingPrincipalOnActiveNotes={1}, PendingInvestmentsPrimaryMkt={2}, PendingInvestmentsSecondaryMkt={3}, PendingQuickInvestOrders={4}, TotalAccountValue={5}, TotalAmountInvestedOnActiveNotes={6}, TotalPrincipalReceivedOnActiveNotes={7}]", AvailableCashBalance, OutstandingPrincipalOnActiveNotes, PendingInvestmentsPrimaryMkt, PendingInvestmentsSecondaryMkt, PendingQuickInvestOrders, TotalAccountValue, TotalAmountInvestedOnActiveNotes, TotalPrincipalReceivedOnActiveNotes);
		}

		public string GetAccountBalanceSpeechString()
		{
			return "Your Prosper Account cash balance is $" + AvailableCashBalance + "! ";
		}

		public string GetPendingInvestmentsBalanceSpeechString()
		{
			return "Your total Pending Quick Invest orders is $"+PendingQuickInvestOrders+"! ";;
		}

		public string GetActiveInvestmentsBalanceSpeechString()
		{
			return "Your total Investments in Active Notes is $"+TotalAmountInvestedOnActiveNotes+"! ";
		}

			

	}
}

