using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RayCastChecker : MonoBehaviour
{
	private GameObject currentListingpPerson;
	// Use this for initialization
	void Start ()
	{
//		castTags.Add ("PROSPER_LISTING");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if ( ProsperListingAreaState.CurrentProsperListingShowing == null )
		{
			RaycastHit hit;
			if (Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out hit, 200.0f)) {
				string hitTag = hit.collider.gameObject.transform.tag;
				//Debug.Log ("Ray cast hit: " + hitTag);
				if (hitTag != null) {
					if (hitTag.Equals ("LISTING_PERSON")) {
						Debug.Log ("Listing Person RayCast hit!");
						// if the player is wants to show the listing, show it, otherwise show the arrow
						if (Input.GetButtonDown ("Fire1")) {
							hit.collider.gameObject.GetComponentInParent<ProsperListingControl> ().ShowCanvas ();
							// show the arrow
							hit.collider.gameObject.GetComponentInChildren<Canvas>().enabled = false;
						}
						else
						{
							if ( currentListingpPerson != null)
							{
								// remove the previous selection
								currentListingpPerson.GetComponentInChildren<Canvas>().enabled = false;
							}
							// show the arrow
							hit.collider.gameObject.GetComponentInChildren<Canvas>().enabled = true;
							currentListingpPerson = hit.collider.gameObject;
						}
					}
				}
			}
		}
	}
}

