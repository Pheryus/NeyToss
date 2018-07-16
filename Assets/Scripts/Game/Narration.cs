using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Narration : MonoBehaviour {

	public static Narration instance;

	public AudioSource audio;

	public AudioClip[] clips;

	public enum galvao {eh_agora, fazendo_falta, brasil_sil, random};

	float delay_time = 0;

	int clip_id = 0;
	bool casa_grande = false;

	void Start(){
		instance = this;
		audio.clip = clips[(int)galvao.eh_agora];
		audio.Play();
	}

	public void checkAudio(){
		if (!audio.isPlaying){
			delay_time += Time.deltaTime;
		    if (delay_time > 5){
				int n;

				bool test = false;

				do {
					n = Random.Range((int)galvao.random, clips.Length);
					
					test = true;
					if (n == 7){
						if (!casa_grande && Random.Range(0, 100) > 90)
							casa_grande = true;
						else
							test = false;
					}
				}
				while (n == clip_id && !test);

				audio.clip = clips[n];
				clip_id = n;
				audio.Play();
				delay_time = 0;
			}
		}
	}

	public void newRecord(){
		audio.clip = clips[(int)galvao.brasil_sil];
		audio.Play();
	}

	public void falta(){
		audio.clip = clips[(int)galvao.fazendo_falta];
		audio.Play();
	}

}
