using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using API.Prosper;

public class ProsperListingModelsFactory : MonoBehaviour
{
	private IList<ProsperListingVO> apiListings = new List<ProsperListingVO>();
	private IList<ProsperListingVO> newApiListings = new List<ProsperListingVO>();
	private bool updateListings = false;
	public GameObject[] prosperListingPrefabArray;
	public GameObject parentArea;
	float gapWidthMultiplier = 6;
	private Random rand = new Random();
	private SpeechWithATT speech;
	public Canvas NoListingsFoundCanvas;

	void Start()
	{
		speech = SpeechWithATT.Instance;
		NoListingsFoundCanvas.enabled = false;
	}

	public void setAPIListings(IList<ProsperListingVO> apiListings)
	{
		Debug.Log ("New listings are being set in the model factory");
		this.newApiListings = apiListings;
		updateListings = true;
	}

	void Update()
	{
		if (updateListings) {
			// we have new listings to set
			//destroy all the old models...etc
			apiListings = newApiListings;
			newApiListings = null;
			updateListings = false;

			if ( apiListings != null && apiListings.Count > 0)
			{
				NoListingsFoundCanvas.enabled = false;
				Debug.Log ("About to build ["+apiListings.Count+"] listing models");
				float startXPosition = parentArea.transform.position.x;
				// GET A DUMMY FIRST
				GameObject dummyGO = (GameObject) Instantiate (prosperListingPrefabArray[0], new Vector3(-100,-100,-100), prosperListingPrefabArray[0].transform.rotation);
				float prefabWidth = dummyGO.GetComponentInChildren<Collider> ().bounds.size.x;
				// determine the starting position
				float startOffsetX = (apiListings.Count * prefabWidth * gapWidthMultiplier)/2;
				startXPosition -= startOffsetX;
				Destroy(dummyGO); // destroy the dummy
				for ( int i = 0; i < apiListings.Count; i++)
				{
					int randIndex= Random.Range(0, prosperListingPrefabArray.Length);
					Vector3 position = new Vector3(startXPosition + (prefabWidth * gapWidthMultiplier),
					                               prosperListingPrefabArray[randIndex].transform.position.y, 
					                               parentArea.transform.position.z + (prefabWidth * gapWidthMultiplier)); 

					GameObject newGo = (GameObject) Instantiate (prosperListingPrefabArray[randIndex], position, prosperListingPrefabArray[randIndex].transform.rotation);
					// show the arrow
					newGo.GetComponentInChildren<Canvas>().enabled = false;
					newGo.transform.parent = parentArea.transform;
					ProsperListingControl dataHolder = newGo.GetComponent<ProsperListingControl>();
					dataHolder.ListingVO = apiListings[i];
					startXPosition = newGo.transform.position.x;
					Debug.Log ("Set model position vector to: "+newGo.transform.position);
				}
			}
			else{
				Debug.Log ("No listings models to build");
				speech.ConvertTextToSpeech("Looks like there are no listings for that selection of filters.");
				if ( ProsperListingAreaState.NoListingsFoundCanvas == null)
				{
					ProsperListingAreaState.NoListingsFoundCanvas = NoListingsFoundCanvas;
				}
				// show the no listings text
				NoListingsFoundCanvas.enabled = true;
			}
		}


	}



}

