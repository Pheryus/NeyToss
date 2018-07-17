using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ShopControl : MonoBehaviour {

	public GameObject shop_content;

	public SnapScroll snap_scroll;

	public Menu menu;

	string game_data_name = "save.json";

	private void Start() {
		menu = FindObjectOfType<Menu>();
		snap_scroll.defineSnapScroll(DataManager.instance.powerups.Length);
		
		definePowerUps();
	}

	public void openShop(){
		gameObject.SetActive(true);
	}

	public void closeShop(){
	
		if (transform.parent.GetComponent<InGameShop>() != null)
			closeInGameWindow();
		
		if (menu != null)
			menu.closeBalloon();
			
		Destroy(gameObject);
	}

	void closeInGameWindow(){
		InGameShop game_shop = FindObjectOfType<InGameShop>();
		DataManager.instance.closeAllBalloons();
		game_shop.closeShop();
	}

	public void definePowerUps(){
		int i = 0;
		foreach (Transform t in shop_content.transform){
			updateButton(t, i);
			i++;
		}
	}

	void buyUpgrade(Transform button, int i){
		int cost = DataManager.instance.powerups[i].getPrice(i);
		PlayerData.money -= cost;
		PlayerData.powerups[i]++;
		PlayerData.checked_news[i] = true;
		DataManager.instance.saveLocalData();

		PlayerPrefs.SetInt("saw" + i, 0);

		definePowerUps();
	}

	bool isEnabled(int index, int new_highscore){
		return (index > 0 && PlayerData.powerups[index-1] == 0) || PlayerData.highscore < new_highscore;
	}


	void updateButton(Transform t, int i){
		PowerUp power = DataManager.instance.powerups[i];

		int new_highscore = power.unlock_highscore + power.scalling * PlayerData.powerups[i];

		bool can_buy = true;

		Text price = t.GetChild(3).GetComponent<Text>();


		if (isEnabled(i, new_highscore)){

			if (i > 0 && PlayerData.powerups[i-1] == 0)
				createUnlockedWindow(t, new_highscore, i, true);
			else
				createUnlockedWindow(t, new_highscore, i, false);

			can_buy = false;
			price.gameObject.SetActive(false);
			t.GetChild(5).gameObject.SetActive(false);
		}
		else{
			price.gameObject.SetActive(true);
			t.GetChild(5).gameObject.SetActive(true);
			t.GetChild(4).gameObject.SetActive(false);
		}

		if (!PlayerData.checked_news[i])
			t.GetChild(6).gameObject.SetActive(true);
		else
			t.GetChild(6).gameObject.SetActive(false);
		
			
		int cost = power.getPrice(i);

		Image img = t.GetChild(1).GetComponent<Image>();
		img.sprite = power.sprite;

		Text text = t.GetChild(0).GetComponent<Text>();
		text.text = power.name;
		
		Text lv = t.GetChild(2).GetComponent<Text>();

		if (power.has_max_level && PlayerData.powerups[i] == power.max_level){
			lv.text = "Lv. Max!";
			price.gameObject.SetActive(false);
			can_buy = false;
			t.GetChild(4).gameObject.SetActive(false);
			t.GetChild(5).gameObject.SetActive(false);
		}
		else
			lv.text = "Lv. " + PlayerData.powerups[i].ToString();

		Button button = img.GetComponent<Button>();

		if (button != null)
			button.onClick.RemoveAllListeners();

		price.text = cost.ToString();

		if (cost > PlayerData.money){
			price.color = new Color(0,0,0,0.5f);
			text.color =  new Color(0,0,0,0.5f);
			if (button != null)
				Destroy(button);
		}
		else if (can_buy){
			int value = i;

			if (button == null)
				button = img.gameObject.AddComponent<Button>();
			
			button.onClick.AddListener(delegate {buyUpgrade(t, value); });
		}
	}

	void createUnlockedWindow (Transform t, int new_highscore, int i, bool need_upgrade){
		Transform unlocked = t.GetChild(4);
		unlocked.gameObject.SetActive(true);
		
		Transform img_transform = unlocked.GetChild(2);
		Transform cost_transform = unlocked.GetChild(1);

		if (need_upgrade){
			unlocked.GetChild(0).GetComponent<Text>().text = "Desbloqueado ao comprar ";
			cost_transform.GetComponent<Text>().text = DataManager.instance.powerups[i-1].name;
		}
		else{
			unlocked.GetChild(0).GetComponent<Text>().text = "Desbloqueado ao chegar em ";
			cost_transform.GetComponent<Text>().text = new_highscore.ToString() + " m"; 
		}

		if (PlayerData.powerups[i] > 0)
			unlocked.GetComponent<Image>().color = new Color(0,0,0,0.5f);
		
		else
			unlocked.GetComponent<Image>().color = Color.black;

		
	}


}
