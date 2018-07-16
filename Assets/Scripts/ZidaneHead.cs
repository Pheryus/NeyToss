using UnityEngine;

public class ZidaneHead : MonoBehaviour {
    
    public Vector2 force;

    public GameObject particle;

    bool collide = false;

    public bool zidaneReborn;

    Vector2 bonus_force;

    void Start(){
        defineBonusForce();
    }

	void defineBonusForce(){
		int index = (int)DataManager.powerUp.fzidane;
		bonus_force = DataManager.instance.powerups[index].bonus_force  * PlayerData.powerups[index];
	}

    private void OnTriggerEnter2D(Collider2D other) {
        if (!collide && other.gameObject.tag == "Player"){
            
            if (zidaneReborn){
                GameControl.instance.throwPlayerAgain();
            }

            GameObject p = Instantiate(particle, other.gameObject.transform.position, Quaternion.identity, transform.parent);
            Destroy(p, 2.5f);
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(force + bonus_force);
            collide = true;
        }
    }

}