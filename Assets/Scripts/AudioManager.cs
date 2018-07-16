 	using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public AudioSource audio;

	public AudioClip[] clips;

	public static AudioManager instance;

	void Awake(){
		if (instance == null){
			instance = this;
		}
		else{
			Destroy(gameObject);
			return;
		}
	}

	public void play(int i){
		audio.clip = clips[i];
		audio.Play();
	}

}
