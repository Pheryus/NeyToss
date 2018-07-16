using UnityEngine;

public class Enemy : MonoBehaviour {
    

    bool collide = false;


    public Vector2 impulse;

    public float movespeed;

    public bool is_a_ball;

    public float min_speed; 

    Ball ball;

    private void Start() {
        if (transform.childCount > 0)
            ball = transform.GetChild(0).GetComponent<Ball>(); 
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player" && !collide && GameControl.instance.can_collide_with_enemy){
            if (is_a_ball && !ball.collide)
                return;

            if (Neymar.instance.getSpeed() < min_speed)
                return;

            impulsePlayer(other.gameObject);
            collide = true;
            Invoke("enableCollide", 0.5f);
        }
    }


    void impulsePlayer(GameObject go){
        go.transform.GetComponent<Rigidbody2D>().AddForce(impulse);
    }

    private void FixedUpdate() {
        transform.position = new Vector2(transform.position.x + movespeed * Time.deltaTime, transform.position.y); 
    }

    void enableCollide(){
        collide = false;
    }

}