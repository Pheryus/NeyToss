using UnityEngine;
using System.IO;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine.SocialPlatforms;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager : MonoBehaviour {
    
    public static DataManager instance;


    static bool is_cloud_data_loaded = false, is_local_data_loaded = false;

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
        Application.targetFrameRate = 30;
        QualitySettings.vSyncCount = 2;
        data_path = Application.persistentDataPath + "/playerInfo.json";
        PlayerData.definePlayerData();
    }

    private void Start() {
        connect();
    }


    public void connect() {
        PlayGamesClientConfiguration client_config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
        PlayGamesPlatform.InitializeInstance(client_config);
        //PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        signIn();
    }



    public void signIn() {
        Social.localUser.Authenticate((bool success) => {
            if (!is_cloud_data_loaded) {
                if (success && PlayerPrefs.HasKey("FirstTime")){
                    loadData();
                    return;
                }
            }
        });
        loadLocalData();
    }

    
    public void deleteSave(){
        File.Delete(data_path);
        PlayerData.definePlayerData();
    }


    #region CloudVariables
    void loadData() {
        //Debug.Log("Tem key firstttime");

        if (Social.localUser.authenticated) {
            //Debug.Log("autenticou. vai dar load no server");

            ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution("playerInfo",
                DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, onLoadGame);
        }
        else {
            //Debug.Log("não conseguiu dar load no server! Dará load local");
            loadLocalData();
        }
    }


    private void loadLocalData() {

        if (!PlayerPrefs.HasKey("FirstTime"))
            PlayerPrefs.SetInt("FirstTime", 1);

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

    public static void saveLocalData(){
        if (!File.Exists(data_path)){
            File.Create(data_path).Dispose();
        }

        File.WriteAllText(data_path, PlayerData.saveToString());
    }


    void onLoadGame(SavedGameRequestStatus status, ISavedGameMetadata game) {
        if (status == SavedGameRequestStatus.Success)
            loadGame(game);
        /*
        else
            loadLocalData();
        */
    }
    static void onSaveGame(SavedGameRequestStatus status, ISavedGameMetadata game) {
        if (status == SavedGameRequestStatus.Success)
            saveGame(game);
        else
            saveLocalData();
    }

    private void loadGame(ISavedGameMetadata game) {
        ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(game, onSavedGameDataRead);
        //((PlayGamesPlatform)Social.Active).SavedGame.Delete(game);
    }

    static private void saveGame(ISavedGameMetadata game) {
        BinaryFormatter bf = new BinaryFormatter();
        
        MemoryStream ms = new MemoryStream();
        //bf.Serialize(ms, GameInfo.saveData());

        byte[] data_to_save = ms.ToArray();

        SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder().Build();
        
        ((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(game, update, data_to_save, pedro);
        saveLocalData();
    }

     static void pedro(SavedGameRequestStatus status, ISavedGameMetadata tixa) {
    }


    private void onSavedGameDataRead(SavedGameRequestStatus status, byte[] saved_data) {
        
        if (status == SavedGameRequestStatus.Success) {
            if (saved_data.Length != 0) {
                Debug.Log("save data maior que 0");
                PlayerData.loadData(new JSONObject(saved_data.ToString()));
            }
            is_cloud_data_loaded = true;
        }
        else
            Debug.Log("falha ao dar load");
    }


    public static void saveData()  {
        if (is_cloud_data_loaded && Social.localUser.authenticated) {
            ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution("playerInfo",
                DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, onSaveGame);    
        }
        saveLocalData();
    }
    #endregion Achievements

}