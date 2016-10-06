
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MonkeyScript : MonoBehaviour {

	public float jumpHeight = 6.0f;
	public float jumpIncrement = 0.2f;
	public AudioSource audioSource;
	public AudioClip[] audioClips;
	private float maxJumpHeight;
	private float minJumpHeight;
	private bool jumpingUp = true;
	private float startOffsetTime;
	private bool IsAnimating = false;
	private int nextSoundIndex = 0;

	void Start()
	{
		maxJumpHeight = this.transform.position.y + jumpHeight;
		minJumpHeight = this.transform.position.y;
		nextSoundIndex = UnityEngine.Random.Range (0, audioClips.Length);

	}

	private void PlayMonkeySound()
	{
		if (!audioSource.isPlaying) {
			audioSource.clip = audioClips[nextSoundIndex];
			nextSoundIndex++;
			if ( nextSoundIndex >= audioClips.Length)
			{
				nextSoundIndex = 0;
			}
			audioSource.Play();
		}
	}


	void Update()
	{
		if (LendingClubState.IsPlayerInRange && !IsAnimating) {
			// we are about to start animating, but start each animation at random
			IsAnimating = true;
			startOffsetTime = Time.time + UnityEngine.Random.Range (0f, 3f);
		}
		if (IsAnimating && Time.time > startOffsetTime) {
			PlayMonkeySound();
			if ( jumpingUp )
			{
				if (transform.position.y < maxJumpHeight)
				{
					this.transform.Translate(0f, jumpIncrement, 0f);
				}
				else {
					jumpingUp = false;
				}
			}
			else
			{
				if ( transform.position.y > minJumpHeight)
				{
					this.transform.Translate(0f, -jumpIncrement, 0f);
				}
				else
				{
					jumpingUp = true;

					transform.position = new Vector3(transform.position.x, minJumpHeight, transform.position.z);
					// since we are landed, check if should continue to animate
					if (!LendingClubState.IsPlayerInRange) {
						IsAnimating = false;
					}
				}
			}
		}

	}



}