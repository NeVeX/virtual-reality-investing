using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonAdder : MonoBehaviour
{

	public int DEFAULT;
	private int currentNumber;
	public int UPPER = 100;
	public int LOWER = 0;
	public Text text;

	void Start()
	{
		currentNumber = DEFAULT;
	}

	// Use this for initialization
	public void Add ()
	{
		currentNumber++;
		if (currentNumber > UPPER) {
			currentNumber = UPPER;
		}
		Update ();
	}
	
	// Update is called once per frame
	public void Subtract ()
	{
		currentNumber--;
		if (currentNumber < LOWER) {
			currentNumber = LOWER;
		}
		Update ();
	}


	private void Update()
	{
		text.text= ""+currentNumber;
	}
}

