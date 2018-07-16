using UnityEngine;
using UnityEngine.UI;


public class ZidaneUI : MonoBehaviour {
    public Image img, missel;
	public Text text;

    public float cooldown;

	float timer = 0;

	Color transp = new Color(1,1,1,0.5f);

	public bool on_cooldown = false;
	public static ZidaneUI instance;

	int zidane_nb;

	public GameObject zidane_prefab;

    private void Start() {
		zidane_nb = PlayerData.powerups[(int)DataManager.powerUp.zidane];
        text.text = zidane_nb.ToString();
        instance = this;
        gameObject.SetActive(false);
    }


	void Update(){
		text.text = zidane_nb.ToString();

		if (on_cooldown){
			text.color = new Color(0,0,0,0.5f);
			missel.color = transp;
			timer += Time.deltaTime;

			img.fillAmount = timer / cooldown;

			if (timer > cooldown){
				timer = 0;
				missel.color = Color.white;
				text.color = Color.black;
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
		if (zidane_nb > 0)
			ZidaneUI.instance.gameObject.SetActive(true);
	}
	void shotZidane(){

		if (zidane_nb > 0 && !on_cooldown && gameObject.activeSelf){
			on_cooldown = true;
			zidane_nb--;

			GameObject z = Instantiate(zidane_prefab, GameControl.instance.canvas);
			Destroy(z, 2.5f);
		}

		if (zidane_nb == 0)
			gameObject.SetActive(false);

	}


}