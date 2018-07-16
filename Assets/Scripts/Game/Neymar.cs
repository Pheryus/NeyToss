using UnityEngine;

public class Neymar : MonoBehaviour {
    
    public static Neymar instance;

    Rigidbody2D[] rbs;


    void Awake(){
        instance = this;
        getRigidbody2D();
    }

    void getRigidbody2D(){
        
        rbs = new Rigidbody2D[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
            rbs[i] = transform.GetChild(i).GetComponent<Rigidbody2D>();

    }


    public float getSpeed(){
        float speed = 0;
        foreach (Rigidbody2D rb in rbs)
            speed += rb.velocity.magnitude;

        return speed / rbs.Length;
    }


}