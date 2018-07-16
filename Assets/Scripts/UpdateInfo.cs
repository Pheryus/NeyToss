using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateInfo : MonoBehaviour {

	Text text;

	public bool score;

	private void Start() {
		text = GetComponent<Text>();
	}

	private void Update() {
		if (score)
			text.text = PlayerData.highscore.ToString();	
		else
			text.text = PlayerData.money.ToString();	
	}
}
