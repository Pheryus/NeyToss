using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour {


    float speed;

    public Transform camera_transform;

    public float background_size;

    private Transform[] layers;

    public float parallax_speed;

    public float view_zone;

    float last_camera_x;

    private int left_index, right_index;

    float started_y;

    private void Start() {
        layers = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
            layers[i] = transform.GetChild(i);

        started_y = layers[0].position.y;
        
        left_index = 0;
        right_index = transform.childCount - 1;

        last_camera_x = camera_transform.position.x;
    }

    void scrollLeft(){
        layers[right_index].position = Vector3.right * (layers[left_index].position.x - background_size);
        left_index = right_index;
        layers[right_index].position = new Vector2(layers[right_index].position.x, started_y);
        right_index --;
        
        if (right_index < 0)
            right_index = layers.Length - 1;
    }

    void scrollRight(){
        layers[left_index].position = Vector3.right * (layers[right_index].position.x + background_size);

        right_index = left_index;
        layers[left_index].position = new Vector2(layers[left_index].position.x, started_y);
        left_index++;
        if (left_index == layers.Length)
            left_index = 0;
    }

    private void FixedUpdate() {

        float delta_x = camera_transform.position.x - last_camera_x; 
        transform.position += Vector3.right * (delta_x * parallax_speed);

        last_camera_x = camera_transform.position.x;

        if (camera_transform.position.x < (layers[left_index].transform.position.x + view_zone))
            scrollLeft();

        if (camera_transform.position.x > (layers[right_index].transform.position.x - view_zone)){
            scrollRight();
        }        
    }

}
