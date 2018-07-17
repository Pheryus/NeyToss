using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour {
    
    public static DataManager instance;

    static string data_path;

    public PowerUp[] powerups;

    public enum powerUp {force = 0, zidane,canarinho, missel, fzidane, fcanarinho, fmissel, players, ball, jesus };

    private void Awake() {
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }  
        else{
            Destroy(gameObject);
            return;
        }
        data_path = Application.persistentDataPath + "/playerInfo.json";
        PlayerData.definePlayerData();
        loadLocalData();
    }

    
    private void deleteSave(){
        PlayerPrefs.DeleteAll();
        File.Delete(data_path);
    }

    private void loadLocalData() {

            
        if (File.Exists(data_path)) {
            Debug.Log("ja existia arquivo");
            string file = File.ReadAllText(data_path);
            PlayerData.loadData(new JSONObject(file.ToString()));
        }
        else{
            //Debug.Log("não existia porra nenhuma de arquivo");
            System.IO.FileInfo file = new System.IO.FileInfo(data_path);
            file.Directory.Create();
            saveLocalData();
        }
        
    }

    public bool hasBalloonUpdate(){

        int money = PlayerData.money;
        int highscore = PlayerData.highscore;

        bool has_update = false;

        for (int i = 0; i < powerups.Length; i++){
            int cost = powerups[i].getPrice(i);
            int high = powerups[i].getHighscore(i);
            
            if (!PlayerPrefs.HasKey("saw" + i) || (PlayerPrefs.HasKey("saw" + i) && PlayerPrefs.GetInt("saw" + i) == 0)){
                if ((money >= cost || highscore >= high) && (PlayerData.checked_news[i]) 
                && (i == 0 || (i > 0 && PlayerData.powerups[i-1] > 0))){
                    PlayerPrefs.SetInt("saw" + i, 1);
                    PlayerData.checked_news[i] = false;
                    has_update = true;
                }
            }

        }
           
        return has_update;
    }

    public void closeAllBalloons(){
        for (int i = 0; i < PlayerData.checked_news.Length; i++){
            PlayerData.checked_news[i] = true;
            PlayerPrefs.SetInt("saw" + i, 1);
        }
    }

    public void saveLocalData(){
        if (!File.Exists(data_path)){
            File.Create(data_path).Dispose();
        }

        File.WriteAllText(data_path, PlayerData.saveToString());
    }

}