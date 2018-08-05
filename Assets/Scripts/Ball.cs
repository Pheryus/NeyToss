using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	public BoxCollider2D bc;

	public bool collide = false;

	void Awake(){
		bc = GetComponent<BoxCollider2D>();
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player"){
			collide = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == "Player"){
			collide = false;
		}
	}
}
