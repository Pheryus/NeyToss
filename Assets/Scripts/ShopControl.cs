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
		snap_scroll.defineSnapScroll(5);
		
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
			if (i == 0)
				i++;
			else
				i+= 2;
		}
	}

	void buyUpgrade(Transform button, int i){
		int cost = DataManager.instance.powerups[i].getPrice(i);
		PlayerData.money -= cost;

		if ((i == 1 || i == 3 || i == 5) && PlayerData.powerups[i] == 0)
			snap_scroll.updatePanInstance(1 + (i-1)/2);

		if (i == (int)DataManager.powerUp.canarinho && PlayerData.powerups[i] == 0){
			PlayerData.jesus = 75f;
		}

		PlayerData.powerups[i]++;
		PlayerData.checked_news[i] = true;
		DataManager.saveData();

		PlayerPrefs.SetInt("saw" + i, 0);

		definePowerUps();
	}

	bool isEnabled(int index, int new_highscore){
		return !((index > 0 && ((index == 1 && PlayerData.powerups[index-1] == 0) || 
		(index > 1 && PlayerData.powerups[index-2] == 0))
		|| PlayerData.highscore < new_highscore));
	}

	void setupBuyItemWindow(Transform t, int i, int next = 0){

		bool can_buy = true;

		Transform buy_item = t.GetChild(4 + next);
		Text cost_item = buy_item.GetChild(0).GetComponent<Text>();
		Text lvl_item = buy_item.GetChild(2).GetComponent<Text>();
		GameObject price_image_go = buy_item.GetChild(1).gameObject;
		PowerUp power = DataManager.instance.powerups[i];

		bool buy_button_appears = true;

		lvl_item.gameObject.SetActive(true);

		if (next == 0){
			defineImage(t, DataManager.instance.powerups[i]);
			defineName(t, DataManager.instance.powerups[i]);
		}

		if (!isEnabled(i, power.unlock_highscore) && PlayerData.powerups[i] == 0 && next == 0){
			if (i > 0 && ((i == 1 && PlayerData.powerups[0] == 0) || PlayerData.powerups[i - 2] == 0))
				createUnlockedWindow(t, power.unlock_highscore, i, true);
			else
				createUnlockedWindow(t, power.unlock_highscore, i, false);

			can_buy = false;
			buy_button_appears = false;
		}
		else{
			t.GetChild(2).gameObject.SetActive(false);
		}

		int cost = power.getPrice(i);
		cost_item.text = cost.ToString();

		Button button = buy_item.GetComponent<Button>();

		if (PlayerData.powerups[i] > 0)
			t.GetChild(0).GetComponent<Text>().color = Color.black;
		
		if ((power.has_max_level && PlayerData.powerups[i] == power.max_level)){
			lvl_item.text = "Lv. Max!";
			buy_button_appears = false;
			can_buy = false;
		}
		else{
			if (i == 1 || i == 3 || i == 5){
				if (PlayerData.powerups[i] == 0)
					lvl_item.gameObject.SetActive(false);
				else
					lvl_item.text = "Lv. " + (PlayerData.powerups[i] - 1).ToString();
			}
			else if (PlayerData.powerups[i] > 0 || ((i == 2 || i == 4 || i == 6) && PlayerData.powerups[i-1] > 0))
				lvl_item.text = "Lv. " + PlayerData.powerups[i].ToString();
			else
				lvl_item.gameObject.SetActive(false);
		}

		buy_item.GetComponent<Image>().enabled = buy_button_appears;
		price_image_go.SetActive(buy_button_appears);
		cost_item.gameObject.SetActive(buy_button_appears);

		if (cost > PlayerData.money){
			cost_item.color = new Color(0,0,0,0.5f);
			if (button != null)
				Destroy(button);
		}
		else if (can_buy)
			createButton(buy_item, button, i);

	}

	void createButton(Transform t, Button button, int i){
		int value = i;

		if (button != null)
			button.onClick.RemoveAllListeners();
		if (button == null)
			button = t.gameObject.AddComponent<Button>();
		button.onClick.AddListener(delegate {buyUpgrade(t, value); });
	}

	void defineImage(Transform t, PowerUp power){
		Image img = t.GetChild(1).GetComponent<Image>();
		img.sprite = power.sprite;
	}

	void defineName(Transform t, PowerUp power){
		Text text = t.GetChild(0).GetComponent<Text>();
		text.text = power.name;
	}

	void updateButton(Transform t, int i){
		PowerUp power = DataManager.instance.powerups[i];
				
		GameObject balloon = t.GetChild(3).gameObject;
		balloon.SetActive(!PlayerData.checked_news[i]);

		setupBuyItemWindow(t, i);
		
		if (i != 0 && i != 7 && PlayerData.powerups[i] > 0)
			setupBuyItemWindow(t, i+1, 1);
	}

	void createUnlockedWindow (Transform t, int new_highscore, int i, bool need_upgrade){
		Transform unlocked = t.GetChild(2);
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

		unlocked.GetComponent<Image>().color = Color.black;
	}


}
