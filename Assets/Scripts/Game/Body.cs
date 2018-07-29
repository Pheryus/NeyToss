using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour {

	Rigidbody2D rb;

	public float limit;

	public int times_used = 0;


	float percent_speed = 1;

	[Range(0,1)]
	public float reduce_percent;



	public float max_speed;

	private void Start() {
		rb = GetComponent<Rigidbody2D>();	
		times_used = 0;
		percent_speed = 1;
	}

	public void useCanarinho(){
		times_used++;
		percent_speed *= 1 - reduce_percent;
		Debug.Log("percent speed: " + percent_speed);
	}

	private void Update() {
		if (PlayerData.powerups[(int)DataManager.powerUp.canarinho] == 0)
			return;
			
		if (rb.velocity.magnitude > limit / 3){

			float speed = rb.velocity.magnitude;

			if (speed > 100)
				speed = 100;
				
			PlayerData.jesus += (speed / limit * Time.deltaTime * ((100 + PlayerData.jesus_bonus) / 100)) * percent_speed;
			if (PlayerData.jesus > 100)
				PlayerData.jesus = 100;
		}

		checkMaxSpeed();
	}

	void checkMaxSpeed(){
		if (rb.velocity.magnitude > max_speed){
			rb.velocity *= 0.9f * 30 * Time.deltaTime;
		}
	}

}
