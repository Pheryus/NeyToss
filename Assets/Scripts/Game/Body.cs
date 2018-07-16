using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour {

	Rigidbody2D rb;

	public float limit;

	private void Start() {
		rb = GetComponent<Rigidbody2D>();	
	}

	private void Update() {
		if (PlayerData.powerups[(int)DataManager.powerUp.canarinho] == 0)
			return;
			
		if (rb.velocity.magnitude > limit / 3){
			PlayerData.jesus += rb.velocity.magnitude / limit * Time.deltaTime * ((100 + PlayerData.jesus_bonus) / 100);
			if (PlayerData.jesus > 100)
				PlayerData.jesus = 100;
		}
	}

}
