using UnityEngine;

public class SongController : MonoBehaviour {
    
    public static SongController instance;

    public AudioSource song;

    private void Start() {

        if (instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
            song.Play();
        }
        else
            Destroy(gameObject);
    }

}