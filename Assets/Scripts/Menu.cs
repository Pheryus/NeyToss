using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour {
    
    public AudioSource galvao;

    bool clicked = false;

    public GameObject shop, ui_canvas, balloon_update;

    private void Start() {
        updateBalloon();
    }


    public void updateBalloon(){
        balloon_update.SetActive(DataManager.instance.hasBalloonUpdate()); 
    }

    public void closeBalloon(){
        balloon_update.SetActive(false);
        DataManager.instance.closeAllBalloons();
    }

    public void startGame(){

        if (clicked)
            return;
        clicked = true;

        galvao.Play();
        Invoke("startGame2", 3.2f);
    }

    void startGame2(){
        SceneManager.LoadScene("DistanceGame");
    }

    public void openShop(){
        Instantiate(shop, ui_canvas.transform);
    }

    private void Update() {
        if (clicked){

            #if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
                SceneManager.LoadScene("DistanceGame");

            #else
            foreach (Touch t in Input.touches){

                Vector2 pos = Camera.main.ScreenToWorldPoint(t.position);

                RaycastHit2D ui_hit = Physics2D.Raycast(pos, Vector2.zero, Mathf.Infinity);

                if (ui_hit != null && ui_hit.collider.gameObject.name == "SkipNarrator"){
                    SceneManager.LoadScene("DistanceGame");
                    break;
                }
            }

            #endif
        }
    }

    public void quit(){
        Application.Quit();
    }

    
}