
using UnityEngine;

public class NeymarCollider : MonoBehaviour {
    
    public static bool trigger = false;

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player" && !trigger){
            GameControl.instance.throwPlayer();
            trigger = true;
        }
    }
}