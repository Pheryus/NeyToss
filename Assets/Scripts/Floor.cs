using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour {

	public Transform player;

	public float bounce, max_bounce;

	float start_bounce;

	public Camera cam;

	public GameObject[] neys;

	public GameObject ney_pos, canvas;

	float timer = 0;

	
	bool created_neymar = false;

	float min_bounce;

	const float const_min_bounce = 3;

	bool bounced = false;

	private void Start() {
		min_bounce = const_min_bounce;
		start_bounce = bounce;
	}

	void Update(){
		transform.position = new Vector2(player.position.x, transform.position.y);

		if (created_neymar && timer > 0.8f){
			timer = 0; 
			created_neymar = false;
		}
		timer += Time.deltaTime;

	}


	public void resetFloor(){
		bounce = start_bounce;
		min_bounce = const_min_bounce;
	}

	public void zeroFloor(){
		bounce = 0.01f;
		min_bounce = 0.01f;
	}

	private void OnCollisionEnter2D(Collision2D other) {
		GameObject go = other.collider.gameObject;
		if (go.tag == "Player"){
			Rigidbody2D rb = go.GetComponent<Rigidbody2D>();

			if (!bounced){
				rb.velocity = new Vector2(rb.velocity.x, Mathf.Min(rb.velocity.y + bounce, max_bounce));
				bounced = true;
				bounce -= 0.5f;
				if (bounce < min_bounce)
					bounce = min_bounce;
				Invoke("enableBounce", 0.1f);
			}

			fakeFriction(rb, 0.98f);

			if (rb.velocity.magnitude > 25)
				CameraLimit.instance.defineShake(new Vector2(0.08f, 0.08f), 0.25f);
			
			if (rb.velocity.magnitude > 36)
				createNey();
		}
	}

	void enableBounce(){
		bounced = false;
	}

	private void OnCollisionStay2D(Collision2D other) {
		GameObject go = other.collider.gameObject;

		if (go.tag == "Player"){
			Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
			fakeFriction(rb, 0.98f);
		}
	}

	void fakeFriction(Rigidbody2D rb, float f){
		rb.velocity = new Vector2(rb.velocity.x * f, rb.velocity.y);
	}

	void createNey(){
		if (!created_neymar){
			int n = Random.Range(0, neys.Length);
			Vector2 pos = ney_pos.transform.GetChild(Random.Range(0, ney_pos.transform.childCount)).position;
			GameObject ney = Instantiate(neys[n], pos, Quaternion.identity, canvas.transform);
			Destroy(ney, 1);
			timer = 0;
			created_neymar = true;
		}
	}
}
