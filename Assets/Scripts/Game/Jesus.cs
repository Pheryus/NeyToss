using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jesus : MonoBehaviour {

	public Slider slider;

	public GameObject jesus_button;

	public Text percentage;

	public Rigidbody2D rb;

	public Vector2 force;

	bool maximum = false;

	void Start(){
		updateSlider();
	}

	void updateSlider(){
		slider.value = PlayerData.jesus / 100;

		if (slider.value >= 1 && !jesus_button.activeSelf && !maximum){
			jesus_button.SetActive(true);
			maximum = true;
		}
		else if (!maximum)
			jesus_button.SetActive(false);
	}

	void updateText(){
		if (PlayerData.jesus <= 100)
			percentage.text = ((int)(PlayerData.jesus)).ToString() + "%";
	}

	private void Update() {
		updateSlider();
		updateText();
	}

	public void clickJesus(){
		maximum = false;
		rb.AddForce(force);
		PlayerData.jesus = 0;
	}
}
