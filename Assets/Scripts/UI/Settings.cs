using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour {

	public AudioMixer audio_mixer;

	Animator anim;

	public Image se_img, bgm_img;

	public Color disable_color;

	Color start_color;

	public AudioSource click_sound_SE, close_window_SE, open_window_SE;

	public GameObject confirm_delete_go;

	void Start(){
		start_color = se_img.color;
		anim = GetComponent<Animator>();
		updateAudioImage("BGM");
		updateAudioImage("SE");
		gameObject.SetActive(false);
	}

	public void openSettings(){
		//open_window_SE.Play();
		gameObject.SetActive(true);
		anim.Play("OpenSettings");
	}

	public void confirmDelete(){
		DataManager.instance.deleteSave();
		confirm_delete_go.SetActive(false);
		click_sound_SE.Play();
	}

	public void cancelDelete(){
		confirm_delete_go.SetActive(false);
		close_window_SE.Play();
	}

	public void closeSettings(){
		anim.Play("CloseSettings");
		//close_window_SE.Play();
	}

	public void endCloseAnimation(){
		gameObject.SetActive(false);
	}

	public void clickAudio(string type){
		if (!PlayerPrefs.HasKey(type) || PlayerPrefs.GetInt(type) == 1){
			audio_mixer.SetFloat(type + "Param", -80);
			PlayerPrefs.SetInt(type, 0);
		}
		else{
			if (type == "BGM")
				audio_mixer.SetFloat(type + "Param", 3);
			else
				audio_mixer.SetFloat(type + "Param", 0);
			PlayerPrefs.SetInt(type,1);
		}
		updateAudioImage(type);
		//click_sound_SE.Play();
	}

	void updateAudioImage(string type){
		if (!PlayerPrefs.HasKey(type) || PlayerPrefs.GetInt(type) == 1){
			if (type == "BGM")
				bgm_img.color = start_color;
			else
				se_img.color = start_color;
		}
		else{
			if (type == "BGM")
				bgm_img.color = disable_color;
			else
				se_img.color = disable_color;
		}
	}
	
	public void deleteProgress(){
		confirm_delete_go.SetActive(true);
	}

}
