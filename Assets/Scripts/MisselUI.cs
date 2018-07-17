using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MisselUI : MonoBehaviour {
	
	public static MisselUI instance;

	public Image img, missel;
	public Text text;

	public float cooldown = 1;

	float timer = 0;

	Color transp = new Color(1,1,1,0.5f);

	public GameObject missel_prefab;


	public bool on_cooldown = false;

	int missel_nb;

	public bool aim_active;

	public GameObject aim;

	public LayerMask ui_layer;

	void Start() {
		missel_nb = PlayerData.powerups[(int)DataManager.powerUp.missel];
		instance = this;
		gameObject.SetActive(false);
	}

	void Update(){
		text.text = missel_nb.ToString();

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
		
		if (aim_active)
			tryFireMissel();
		
	}

	private void FixedUpdate() {
		if (aim_active){

			Vector2 v = Vector2.zero;

			#if UNITY_EDITOR
			v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			#else

			bool touched = false;

			foreach (Touch t in Input.touches){
				if (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary){
					touched = true;
					v = Camera.main.ScreenToWorldPoint(t.position);
				}
			}

			aim.gameObject.SetActive(touched);

			#endif
			aim.transform.position = v;

		}
	}



	public void tryFireMissel(){

		Vector2 position = Vector2.zero;

		#if UNITY_EDITOR
		position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		#else

		bool released = false;

		foreach (Touch t in Input.touches){
			if (t.phase == TouchPhase.Ended){
				position = Camera.main.ScreenToWorldPoint(t.position);
				released = true;
			}
		}

		if (!released)
			return;

		#endif

		RaycastHit2D rh = Physics2D.Raycast(position, Vector2.zero, Mathf.Infinity, ui_layer);
		if (rh.collider == null)
			shotMissel();
	}

	public void buttonClicked(){
		if (GameControl.instance.state != GameControl.game_state.game)
			return;
		
		aim_active = !aim_active;

		if (aim_active)
			img.color = Color.blue;
		else
			img.color = Color.red;

		aim.SetActive(aim_active);
	}

	public void checkIfItsActive(){
		if (missel_nb > 0)
			MisselUI.instance.gameObject.SetActive(true);
	}
	public void shotMissel(){

		if (missel_nb > 0 && !on_cooldown && gameObject.activeSelf){

			Vector2 pos = new Vector2(GameControl.instance.player_torso.position.x - 18, 0);

			GameObject go = Instantiate(missel_prefab, pos, Quaternion.identity, GameControl.instance.canvas);

			Vector2 mouse_pos = Input.mousePosition;

			go.GetComponent<Missel>().position = mouse_pos;
			MisselUI.instance.on_cooldown = true;
			missel_nb--;
		}

		if (missel_nb == 0)
			gameObject.SetActive(false);
	}
}
