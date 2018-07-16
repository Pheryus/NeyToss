using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBar : MonoBehaviour {


	public Animator anim;

	public GameObject menu;

	public void openWindow(){
		Time.timeScale = 0;
		menu.SetActive(true);
		anim.Play("openWindow");
	}

	public void closeWindow(){
		anim.Play("closeWindow");
		Invoker.InvokeDelayed(endCloseAnimation, 0.5f);
	}

	public void resetGameButton(){
		anim.Play("closeWindow");
		Invoker.InvokeDelayed(resetGame, 0.5f);
	}

	public void returnToMenu(){
		endCloseAnimation();
		SceneManager.LoadScene("Menu");
	}

	void resetGame(){
		Time.timeScale = 1;
		SceneManager.LoadScene("DistanceGame");
	}

	void endCloseAnimation(){
		Time.timeScale = 1;
		menu.SetActive(false);
	}
}
