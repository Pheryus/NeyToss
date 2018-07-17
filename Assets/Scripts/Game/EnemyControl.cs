using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyControl : MonoBehaviour {

    public static EnemyControl instance;

    public GameObject neymar;

    public List<Pool> pools;

    public Dictionary<string, Queue<GameObject>> pool_dictionary;

    const int ball_offset = 13, enemy_offset = 13;

    public float interval, ball_interval;

    public float min_interval, min_ball_interval;

    public float start_offset;


    const float min_x_variance = 50;

    float default_x_variance = 100, x_enemy_variance = 0, x_ball_variance = 0;

    void Awake(){
        instance = this;
        defineInterval();
        createObjectPool();

        x_enemy_variance = neymar.transform.position.x + start_offset;
        x_ball_variance = neymar.transform.position.x + start_offset;
    }


    void defineInterval(){
        int player_id = (int)DataManager.powerUp.players;
        int ball_id = (int)DataManager.powerUp.ball;

        interval -= PlayerData.powerups[player_id] * 10;
        ball_interval -= PlayerData.powerups[ball_id] * 10;

        if (interval < min_interval)
            interval = min_interval;

        if (ball_interval < min_ball_interval)
            ball_interval= min_ball_interval;
    }

    void createObjectPool(){
        pool_dictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool p in pools){
            Queue<GameObject> object_pool = new Queue<GameObject>();
            for (int i=0; i < p.size; i++){
                GameObject go = Instantiate(p.prefab);
                go.SetActive(false);
                object_pool.Enqueue(go);
            }
            pool_dictionary.Add(p.tag, object_pool);
        }
    }

    public GameObject spawnFromPool(string tag){

        if (!pool_dictionary.ContainsKey(tag))
            return null;
        
        GameObject object_to_spawn = pool_dictionary[tag].Dequeue();
        object_to_spawn.SetActive(true);
        
        pool_dictionary[tag].Enqueue(object_to_spawn);
        return object_to_spawn;
    }

    private void Update() {
        float enemy_delta = (neymar.transform.position.x - x_enemy_variance) / 3;
        float ball_delta = (neymar.transform.position.x - x_ball_variance) / 3;

        if (enemy_delta > interval){
            x_enemy_variance = neymar.transform.position.x;
            createEnemy();
        }
        if (ball_delta > ball_interval){
            x_ball_variance = neymar.transform.position.x;
            createBall();
        }
    }

    void createBall(){
        GameObject ball = spawnFromPool("Ball");
        ball.transform.position = new Vector2(neymar.transform.position.x + ball_offset, ball.transform.position.y);
    }
    
    void createEnemy(){
        GameObject player = spawnFromPool("Player");
        player.transform.position = new Vector2(neymar.transform.position.x + enemy_offset, player.transform.position.y);
    }
}