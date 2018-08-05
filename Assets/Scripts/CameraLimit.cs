using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLimit : MonoBehaviour {

	
	public float max_y;

	public Transform player, body;

	float camera_distance;


	public static CameraLimit instance;


	float x_offset, y_offset;

	public float camera_speed;


	public float n1, n2;

	public Vector2 camera_start;


	Vector2 shake;

	float duration, timer = 0;


	Rigidbody2D rb;

	void Start(){
		instance = this;
		camera_start = transform.position + new Vector3(6, 0, 0);
		rb = player.GetComponent<Rigidbody2D>();
		x_offset = player.position.x - camera_start.x;
		y_offset = player.position.y - camera_start.y;
		
	}

	void Update() {
		
		if (Time.timeScale == 0)
			return;

		if (GameControl.instance.state != GameControl.game_state.game)
			return;

		
	
		Vector3 position = Vector3.zero;

		float playerVel = 0;

		foreach (Transform member in body){
			position += member.position;
			playerVel += member.GetComponent<Rigidbody2D>().velocity.x;
		}

		playerVel /= body.childCount;
		position /= body.childCount;

		float velOffset = Mathf.Lerp( 0f, n1, Mathf.InverseLerp(0, n2, playerVel));

		//if (Mathf.Abs(playerVel) > 0.5f)
		transform.position = new Vector3(position.x - x_offset + velOffset, position.y - y_offset, transform.position.z);
		
		checkVerticalLimit();
		shakeScreen();
	}


	public void shakeScreen(){
		if (timer < duration){
			transform.position += new Vector3(Random.Range(-shake.x, shake.x), Random.Range(-shake.y, shake.y), 0);
			timer += Time.deltaTime;
		}
		else{
			timer = 0;
			duration = 0;
		}
	}

	public void defineShake(Vector2 _shake, float _duration){
		shake = _shake;
		duration = _duration;
	}


	void checkVerticalLimit(){
		if (transform.position.y > max_y){
			transform.position = new Vector3(transform.position.x, max_y, transform.position.z);
		}
		else if (transform.position.y < camera_start.y)
			transform.position = new Vector3 (transform.position.x, camera_start.y, transform.position.z);

		//transform.position = new Vector3(Mathf.Max(transform.position.x, camera_start.x), transform.position.y, transform.position.z);
		//transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
	}
}
