using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missel : MonoBehaviour {

	GameObject target;

	Rigidbody2D rb, player_rb;

	public Vector2 force;

	public GameObject explosion;

	bool explode;

	public float rotationSpeed, moveSpeed;

	public float life_time = 0, max_time = 2.5f;


	public Vector2 position;

	float t = 0, secs = 4f;

	Vector2 bonus_force;

	Camera main;

	void Start () {
		main = Camera.main;
		rb = GetComponent<Rigidbody2D>();

		target = GameObject.FindGameObjectWithTag("Player");	
		player_rb = target.GetComponent<Rigidbody2D>();
		defineBonusForce();
	}

	void defineBonusForce(){
		int index = (int)DataManager.powerUp.fmissel;
		bonus_force = DataManager.instance.powerups[index].bonus_force * PlayerData.powerups[index];
	}
	
	private void Update() {
		life_time += Time.deltaTime;

		if (life_time >= max_time){
			Destroy(gameObject);
			return;
		}

		Vector2 place = main.ScreenToWorldPoint(position);

		t += Time.deltaTime/secs;

		if (transform.position.x > place.x){
			transform.position += Vector3.right * 10f * Time.deltaTime;
		}
		else
			transform.position = Vector3.Lerp(transform.position, place, t);

	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (!explode && other.gameObject.tag == "Player"){
			
			GameObject perna = GameObject.Find("LegAL");
			GameObject body = GameObject.Find("Body");

			
			GameObject neymar = GameObject.Find("Neymar");

			zeroNeymarVelocity(neymar);

			perna.GetComponent<Rigidbody2D>().AddForce(force + bonus_force);
			createExplosion(body);
			explode = true;
		}
		else if (other.gameObject.tag == "Floor"){
			createExplosion();
			explode = true;
		}
	}

	void zeroNeymarVelocity(GameObject go){
		foreach (Transform t in go.transform){
			Rigidbody2D rb = t.GetComponent<Rigidbody2D>();
			if (rb.velocity.x < 0)
				rb.velocity = new Vector2(rb.velocity.x, 0);
		}
	}


	void createExplosion(GameObject p = null){
		GameObject go;
		if (p == null)
			go = Instantiate(explosion, transform.position, Quaternion.identity);
		else
			go = Instantiate(explosion, p.transform);

		Destroy(go, 1f);
		Destroy(gameObject);
	}
}
