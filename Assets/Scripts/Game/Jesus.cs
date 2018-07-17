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

	Vector2 bonus_force;

	void Start(){
		defineBonusForce();
		updateSlider();
	}

	void defineBonusForce(){
		int index = (int)DataManager.powerUp.fcanarinho;
		int lv = PlayerData.powerups[index];
		PowerUp force = DataManager.instance.powerups[index];
		float start_force = force.bonus_force.x;

		float to_add = start_force;

		float bonus_force = 0;

		for (int i = 0; i < lv; i++){
			bonus_force += to_add;
			to_add *= (100 - force.reduce_percentage) / 100;

			if (to_add < force.min_bonus_force.x){
				to_add = force.min_bonus_force.x;
			}
		}
		this.force += new Vector2(bonus_force, 0);
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
