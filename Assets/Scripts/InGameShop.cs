using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameShop : MonoBehaviour {

	public GameObject rayblocker;

	public GameObject shop, balloon, shop_button;

	public void enableShop(){
		updateBalloon();
		shop_button.SetActive(true);
	}

	void updateBalloon(){
		balloon.SetActive(DataManager.instance.hasBalloonUpdate()); 
	}

	public void openShop(){
		rayblocker.SetActive(true);
		GameObject go = Instantiate(shop, transform);
		go.transform.GetChild(2).GetComponent<Button>().enabled = true;
	}

	public void closeShop(){
		rayblocker.SetActive(false);
		updateBalloon();
	}

}
