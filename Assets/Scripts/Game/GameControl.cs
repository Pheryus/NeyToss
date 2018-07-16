using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour {

	Image angle_img;

	public float max_angle;

	public Rigidbody2D rb;

	float force;

	const float min_force = 1800;

	public float max_force;

	public Transform player;

	public Transform player_torso;

	public float gravity;

	public Text distance_text;

	public Transform arrow;

	public Image force_intensity;

	public bool left_rotation = true;

	public float player_x_multiplier;

	public float started_x;

	float arrow_angle;

	public enum game_state {start, set_angle, game, tixa, over, reborn, freeze_ney};

	public game_state state = game_state.start;

	float timer = 0, timer_speed;

	public AnimationCurve force_curve;

	public float curve_speed;

	public static GameControl instance;

	public float death_timer;

	public float min_speed;

	public Transform carinha;

	public Rigidbody2D pe_carinha;

	public GameObject game_over;

	public GameObject foot_particle;

	public Transform canvas;

	public GameObject zidane_reborn;

	public Floor floor;

	public bool reborn = true;

	public GameObject new_score;

	public Text highscore, new_highscore_text;

	public Rigidbody2D head;

	public GameObject neymarquezine;

	float marquezine_timer = 0;


	int last_meta = 0, n_between_meta = 50, last_mini_meta = 0, n_between_minimeta = 2, score = 0, highscore_value;

	bool can_reborn = false;

	public float impulse_force;

	bool left_click = true, started;

	public InGameShop game_shop;

	int neymarquezine_clicked = 0;

	public LayerMask ui_layer, action_layer;

	public bool can_collide_with_enemy = true;

	public GameObject canarinho_bar;

	public bool debug;


	[Range(0, 1)]
	public float debug_value;


	void Awake(){
		instance = this;
	}

	void Start(){
		defineCanarinho();
		defineBonusForce();
		started = false;
		can_collide_with_enemy = true;
	
		NeymarCollider.trigger = false;
		started_x = player_torso.position.x;
		highscore_value = PlayerData.highscore;

		if (highscore_value != 0){
			highscore.text = highscore_value.ToString();
			highscore.transform.parent.gameObject.SetActive(true);
		}
		Invoke("canClick", 0.5f);
	}

	void defineCanarinho(){
		if (PlayerData.powerups[(int)DataManager.powerUp.canarinho] > 0)
			canarinho_bar.SetActive(true);
	}

	void defineBonusForce(){
		int index = (int)DataManager.powerUp.force;
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
		max_force += bonus_force;
	}

	void canClick(){
		started = true;
	}

	void changeForce(){
		force_intensity.fillAmount = force_curve.Evaluate(Mathf.PingPong(timer,1f));
		timer += Time.deltaTime * curve_speed;
	}

	void clickActionArea(){
		if (state == game_state.start)
			setAngle();
		else if (state == game_state.set_angle){
			setForce();
			state = game_state.tixa;
			carrinho();
		}
	}


	void Update(){

		if (Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit();
		}

		if (state == game_state.reborn && can_reborn){
			impulseNeymar();
			return;
		}

		if (state == game_state.start)
			rotateArrow();
		else if (state == game_state.set_angle)
			changeForce();

		if (started){
			checkActionArea();			
		}

		if (game_state.game == state){
			Narration.instance.checkAudio();
			updateDistanceText();
			updateDeathTimer();
		}
	}

	void checkActionArea(){

		Vector3 origin = Vector3.zero;

		bool clicked = false;


		#if UNITY_EDITOR
		if (Input.GetMouseButtonDown(0)){
			origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			clicked = true;
		}

		#else

		foreach (Touch t in Input.touches){
			if (t.phase == TouchPhase.Began){
				origin = Camera.main.ScreenToWorldPoint(t.position);
				clicked = true;
			}
		}
		#endif


		if (!clicked)
			return;

		RaycastHit2D ui_hit = Physics2D.Raycast(origin, Vector2.zero, Mathf.Infinity, ui_layer);
		RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.zero, Mathf.Infinity, action_layer);

		if (ui_hit.collider == null  && hit.collider != null && hit.collider.gameObject.name == "ActionArea")
			clickActionArea();
	}

	void updateDeathTimer(){
		death_timer += Time.deltaTime;

		if (rb.velocity.magnitude > min_speed)
			death_timer = 0;
		if (death_timer > 1)
			gameOver();		
	}

	public void returnToMenu(){
		SceneManager.LoadScene("Menu");
	}
	
	void clickedToImpulseNeymar(){
		neymarquezine_clicked++;
		float max_y = -2;
		if (head.position.y > max_y){
			head.velocity = Vector2.zero;
			head.position = new Vector2(head.position.x, max_y);
		}
		else{
			head.AddForce(Vector2.up * impulse_force);
		}
	}

	void impulseNeymar(){
		if (left_click){
			if (Input.GetMouseButtonDown(0)){
				left_click = false;
				clickedToImpulseNeymar();
			}
		}
		else {
			if (Input.GetMouseButtonDown(1)){
				left_click = true;
				clickedToImpulseNeymar();
			}
		}

		marquezine_timer += Time.deltaTime;

		if (marquezine_timer > 3.5f){
			marquezine_timer = 0;
			state = game_state.over;
			gameOver();
			return;
		}

		if (neymarquezine_clicked > 20){
			state = game_state.freeze_ney;
			zidaneReborn();
			neymarquezine_clicked = 0;
		}
		
	}

	void zidaneReborn(){
		neymarquezine.SetActive(false);
		GameObject go = Instantiate(zidane_reborn, canvas);
		Destroy(go, 2.5f);
	}

	public void freezeNey(bool b){
		foreach (Transform t in player){
			t.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			t.GetComponent<Rigidbody2D>().isKinematic = b;
		}
	}

	public void throwPlayerAgain(){
		Invoke("enableMissel", 1);
		floor.resetFloor();
		state = game_state.game;
		can_collide_with_enemy = true;
	}

	void carrinho(){
		foot_particle.SetActive(true);
		foreach (Transform member in carinha)
			member.GetComponent<Rigidbody2D>().isKinematic = false;

		pe_carinha.AddForce(3000 * Vector2.right);
	}


	void enableReborn(){
		can_reborn = true;
	}

	void reduceVelocity(){
		foreach (Transform t in player){
			t.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		}
		floor.zeroFloor();
	}

	void gameOver(){

		can_collide_with_enemy = false;

		if (!reborn){
			state = game_state.reborn;
			reborn = true;
			neymarquezine.SetActive(true);
			reduceVelocity();

			Invoke("enableReborn", 2f);
		}
		else{
			state = game_state.over;
			neymarquezine.SetActive(false);
		}

		disableMissel();
		death_timer = 0;

		if (state == game_state.over){
			PlayerData.money += score;
			
			if (highscore_value < score){
				PlayerData.highscore = score;
				new_score.SetActive(true);
				Narration.instance.newRecord();
				new_highscore_text.text = score.ToString();
			}
			else{
				game_over.SetActive(true);
			}
			DataManager.instance.saveLocalData();
			game_shop.enableShop();
		}
	}

	void enableGravity(){
		foreach (Transform child in player){
			Rigidbody2D rb2d = child.GetComponent<Rigidbody2D>();
			rb2d.gravityScale = gravity;
			rb2d.velocity = Vector2.zero;
			rb2d.isKinematic = false;
		}
	}

	void updateDistanceText(){
		score = (int) ((player_torso.position.x - started_x) / 3);

		if (score >= last_meta + n_between_meta){
			last_meta = score;
			distance_text.GetComponent<Animator>().Play("DistanceText", -1, 0);
		}

		distance_text.text = "m: " + score;
	}	

	void setForce(){

		if (debug)
			force = min_force + (max_force - min_force) * debug_value;
		else
			force = min_force + (max_force - min_force) * force_intensity.fillAmount;

		force_intensity.transform.parent.gameObject.SetActive(false);
	}

	void setAngle(){
		arrow_angle = arrow.GetChild(0).eulerAngles.z;
		arrow.gameObject.SetActive(false);
		force_intensity.transform.parent.gameObject.SetActive(true);
		force_intensity.transform.parent.rotation = arrow.GetChild(0).rotation;
		state = game_state.set_angle;
	}

	public void throwPlayer(){

		Narration.instance.falta();

		enableGravity();
		Vector3 dir = Vector3.zero;

		if (debug)
			dir = Quaternion.AngleAxis(35, Vector3.forward) * Vector3.right;

		else
			dir = Quaternion.AngleAxis(arrow_angle + 5, Vector3.forward) * Vector3.right;

		dir = new Vector2(dir.x * player_x_multiplier, dir.y * 0.8f);

		rb.AddForce(dir * force);
		state = game_state.game;
		
		Invoke("enableMissel", 2f);

		arrow.gameObject.SetActive(false);

		distance_text.gameObject.SetActive(true);
	}

	public void resetGame(){
		SceneManager.LoadScene("DistanceGame");
	}

	void enableMissel(){
		MisselUI.instance.checkIfItsActive();
		ZidaneUI.instance.checkIfItsActive();
	}

	void disableMissel(){
		MisselUI.instance.gameObject.SetActive(false);
		ZidaneUI.instance.gameObject.SetActive(false);
	}

	void rotateArrow(){
		if (arrow.position.z < 0)
			left_rotation = false;
		else if (arrow.position.z > 90)
			left_rotation = true;
	}

}
