
using UnityEngine;

public class NeymarCollider : MonoBehaviour {
    
    public static bool trigger = false;

    Rigidbody2D rb;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player" && !trigger){
            GameControl.instance.throwPlayer();
            trigger = true;
        }
    }
}