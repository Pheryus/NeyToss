using UnityEngine;
using UnityEngine.UI;


public class ZidaneUI : MonoBehaviour {
    public Image img, zidane;

    float cooldown = 18;

	float timer = 0;

	Color transp = new Color(1,1,1,0.5f);

	public bool on_cooldown = false;
	public static ZidaneUI instance;

	public GameObject zidane_prefab;

    private void Start() {
		cooldown -= PlayerData.powerups[(int)DataManager.powerUp.zidane] - 1;
        instance = this;
        gameObject.SetActive(false);
    }


	void Update(){
		if (on_cooldown){
			zidane.color = transp;
			timer += Time.deltaTime;
			img.fillAmount = timer / cooldown;

			if (timer > cooldown){
				timer = 0;
				zidane.color = Color.white;
				on_cooldown = false;
			}
		}
		
	}

	public void buttonClicked(){
		if (GameControl.instance.state == GameControl.game_state.game){
			shotZidane();
			MisselUI.instance.aim_active = false;
			MisselUI.instance.aim.SetActive(false);
		}
	}

	public void checkIfItsActive(){
		if (PlayerData.powerups[(int)DataManager.powerUp.zidane] > 0)
			ZidaneUI.instance.gameObject.SetActive(true);
	}
	void shotZidane(){
		if (!on_cooldown && gameObject.activeSelf){
			on_cooldown = true;
			GameObject z = Instantiate(zidane_prefab, GameControl.instance.canvas);
			Destroy(z, 2.5f);
		}
		/*
		if (zidane_nb == 0)
			gameObject.SetActive(false);
		*/

	}


}