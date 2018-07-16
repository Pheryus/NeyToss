using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpUI : MonoBehaviour {

	public GameObject balloon;


	public void updateBalloon(){
		balloon.SetActive(!PlayerData.checked_news[transform.GetSiblingIndex()]);	
	}
}
