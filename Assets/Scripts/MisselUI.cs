using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MisselUI : MonoBehaviour {
	
	public static MisselUI instance;

	public Image img;

	float cooldown = 18;

	float timer = 0;

	Color transp = new Color(1,1,1,0.5f);

	public GameObject missel_prefab;

	public bool on_cooldown = false;

	public bool aim_active;

	public GameObject aim;

	public LayerMask ui_layer;

	bool touched, can_fire = false;

	public Sprite unpressed_missile, pressed_missile;

	public Image sprite;

	void Start() {
		cooldown -= PlayerData.powerups[(int)DataManager.powerUp.missel] - 1;
		instance = this;
		gameObject.SetActive(false);
	}

	void Update(){

		if (on_cooldown){
			sprite.color = transp;
			timer += Time.deltaTime;

			img.fillAmount = timer / cooldown;

			if (timer > cooldown){
				timer = 0;
				sprite.color = Color.white;
				on_cooldown = false;
			}
		}
		
		if (aim_active && can_fire)
			tryFireMissel();
		
	}

	private void FixedUpdate() {
		
		Vector2 v = Vector2.zero;

		#if UNITY_EDITOR
		v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		if (aim_active && can_fire)
			touched = true;
		#else

		touched = false;

		foreach (Touch t in Input.touches){
			if (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary){
				if (aim_active && can_fire)
					touched = true;
				v = Camera.main.ScreenToWorldPoint(t.position);
			}
		}
		if (aim_active && can_fire)
			aim.gameObject.SetActive(touched);

		#endif
		aim.transform.position = v;

		if (on_cooldown){
			aim_active = false;
			aim.SetActive(false);
		}

		if (aim_active)
			img.sprite = pressed_missile;
		else
			img.sprite = unpressed_missile;
	}

	public void tryFireMissel(){

		Vector2 position = Vector2.zero;
		bool released = false;


		#if UNITY_EDITOR
		position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		if (Input.GetMouseButtonUp(0))
			released = true;

		#else

		foreach (Touch t in Input.touches){
			if (t.phase == TouchPhase.Ended){
				position = Camera.main.ScreenToWorldPoint(t.position);
				released = true;
			}
		}

		#endif

		if (!released)
			return;

		RaycastHit2D rh = Physics2D.Raycast(position, Vector2.zero, Mathf.Infinity, ui_layer);
		if (rh.collider == null)
			shotMissel();
	}

	void enableFire(){
		can_fire = true;
	}

	public void buttonClicked(){
		if (GameControl.instance.state != GameControl.game_state.game)
			return;

		if (on_cooldown)
			return;
		
		aim_active = !aim_active;

		if (aim_active)
			Invoke("enableFire", 0.1f);

		aim.SetActive(aim_active);
	}

	public void checkIfItsActive(){
		if (PlayerData.powerups[(int)DataManager.powerUp.missel] > 0)
			MisselUI.instance.gameObject.SetActive(true);
	}
	public void shotMissel(){

		if (!on_cooldown && gameObject.activeSelf){

			can_fire = false;

			Vector2 pos = new Vector2(GameControl.instance.player_torso.position.x - 18, 0);

			GameObject go = Instantiate(missel_prefab, pos, Quaternion.identity, GameControl.instance.canvas);

			Vector2 mouse_pos = Input.mousePosition;

			go.GetComponent<Missel>().position = mouse_pos;
			MisselUI.instance.on_cooldown = true;
		}

		/* 
		if (missel_nb == 0)
			gameObject.SetActive(false);
		*/
	}
}
