using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextTrigger : MonoBehaviour
{
//	public Canvas canvas;
	public Text text;
	private float alphaTween = 1.0f;
	private SpeechWithATT speech;
	private bool playedEnter = false;
	//public bool soundTrigger = false;
	public AudioSource audioSource;
//	private float timeJump = 0.1f;

	void Start()
	{
		speech = SpeechWithATT.Instance;
		text.enabled = true;
		text.CrossFadeAlpha (0.0f, 0.1f, true); // hack in a way

	}

	void OnTriggerEnter(Collider otherCollider)
	{
		if (otherCollider.gameObject.tag.Equals ("MainCamera")) {
			Debug.Log ("Entered the Billion trigger area");
			if ( !playedEnter)
			{
				speech.ConvertTextToSpeech(text.text);
				if ( audioSource != null)
				{
					Debug.Log("Playing audio in text trigger");
					audioSource.Play ();
				}
				playedEnter = true;
			}
			text.enabled = true;
			text.CrossFadeAlpha(1.0f, alphaTween, true); //canvas.GetComponent<CanvasRenderer> ().SetAlpha(0f);
//			ShowCanvas();
		}
	}
	
	void OnTriggerExit(Collider otherCollider)
	{
		if (otherCollider.gameObject.tag.Equals ("MainCamera")) {
			Debug.Log ("Leaving the Billion trigger area");
			text.CrossFadeAlpha(0.0f, alphaTween, true);//canvas.GetComponent<CanvasRenderer> ().SetAlpha(1f);
//			HideCanvas();
		}
	}

//	private void ShowCanvas()
//	{
//		Debug.Log ("Showing canvas");
//		CanvasRenderer cr = canvas.GetComponent<CanvasRenderer> ();
//		if (cr.GetAlpha () < 1.0f) {
//			cr.SetAlpha (cr.GetAlpha () + alphaJump);
//			Invoke ("ShowCanvas", timeJump);
//		} 
//	}
//
//	private void HideCanvas()
//	{
//		Debug.Log ("Hiding canvas");
//		CanvasRenderer cr = canvas.GetComponent<CanvasRenderer> ();
//		if (cr.GetAlpha () > 0.0f) {
//			cr.SetAlpha (cr.GetAlpha () - alphaJump);
//			Invoke ("HideCanvas", timeJump);
//		} 
////		else {
////			canvas.enabled = false;
////		}
//	}


}

