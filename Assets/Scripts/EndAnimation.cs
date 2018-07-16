using UnityEngine;

public class EndAnimation : MonoBehaviour {
    
    public void endAnimation(){
        GetComponent<Animator>().enabled = false;
    }
}