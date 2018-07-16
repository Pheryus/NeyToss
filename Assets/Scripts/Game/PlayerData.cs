using UnityEngine;

public class PlayerData : MonoBehaviour {
    

    public static int money = 0, highscore = 0;

    public static int[] powerups;

    public static float jesus, jesus_bonus;
    
    const int max_powerups = 10;

    public static bool[] checked_news;

    public static void definePlayerData(){
        powerups = new int[max_powerups];
        checked_news = new bool[max_powerups];
        for (int i = 0; i < max_powerups; i++){
            powerups[i] = 0;
            checked_news[i] = true;
        }

        powerups[0] = 10;
        powerups[1] = 1;
        jesus = 0;
        money = 0;
        highscore = 0;
        defineJesusBonus();
    }

    public static void loadData(JSONObject json){
        money = (int)json.GetField("money").n;
        jesus = json.GetField("jesus").n;
        highscore = (int)json.GetField("highscore").n;
        
        JSONObject powerups_json = json.GetField("powerups");
        for (int i = 0; i < powerups_json.Count; i++){
            powerups[i] = (int) powerups_json[i].n;
        }
        JSONObject news_json = json.GetField("news");
        if (news_json != null)
            for (int i = 0; i < news_json.Count; i++)
                checked_news[i] = news_json[i].b;

        defineJesusBonus();
    }

    static void defineJesusBonus(){
        int jesus_id = (int)DataManager.powerUp.canarinho;
        jesus_bonus = powerups[jesus_id] * DataManager.instance.powerups[jesus_id].bonus_force.x;
    }

    public static string saveToString(){
        JSONObject save_json = new JSONObject();
        save_json.AddField("money", money);
        save_json.AddField("jesus", jesus);
        save_json.AddField("highscore", highscore);
        save_json.AddField("news", getCheckNewsJSON());

        save_json.AddField("powerups", getPowerupsToJSON());

        return save_json.ToString();
    }

    static JSONObject getCheckNewsJSON(){
        JSONObject json = new JSONObject(JSONObject.Type.ARRAY);
        foreach (bool b in checked_news)
            json.Add(b);
        return json;
    }

    static JSONObject getPowerupsToJSON(){
        JSONObject json = new JSONObject(JSONObject.Type.ARRAY);
        foreach (int n in powerups)
            json.Add(n);
        return json;
    }

}