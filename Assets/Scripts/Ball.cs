using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	BoxCollider2D bc;

	public bool collide = false;

	void Awake(){
		bc = GetComponent<BoxCollider2D>();
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Player"){
			//bc.isTrigger = true;
			collide = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == "Player"){
			collide = false;
		}
	}
}
