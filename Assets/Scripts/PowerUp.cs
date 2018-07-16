using UnityEngine;


[CreateAssetMenu(fileName = "New PowerUp", menuName = "PowerUp")]  
public class    PowerUp : ScriptableObject {
    
    public string name;

    public int start_cost;

    public Vector2 bonus_force, min_bonus_force;


    public int unlock_highscore, scalling;

    public float scalling_percentage, cost_exponential_percentage, reduce_percentage;

    public int max_level;

    public bool has_max_level;

    public Sprite sprite;

    public int getPrice(int i){
        return start_cost + (int)((start_cost * cost_exponential_percentage) * PlayerData.powerups[i] * PlayerData.powerups[i]);
    }

    public int getHighscore(int i){

        if (has_max_level && i > max_level)
            return 0;

        if (i == 0)
            return unlock_highscore;

        return (int)((unlock_highscore + scalling * PlayerData.powerups[i]) * (100 + scalling_percentage * i-1) / 100);
    }

}