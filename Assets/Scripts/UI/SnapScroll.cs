using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapScroll : MonoBehaviour {

	public GameObject pan_prefab, pan_prefab2;

	[Range(0,50)]
	[Header("Controllers")]
	public int pan_count;

	[Range(0,500)]
	public int  pan_offset;

	[Range(0, 20)]
	public float snap_speed;

	[Range(0, 5f)]
	public float scale_offset;

	[Range(0, 20)]
	public float scale_speed;

	public GameObject[] pan_instances;

	public Vector2[] pan_position;

	public Vector2[] pan_scale;

	RectTransform rect_transform;

	public ScrollRect scroll_rect;

	[Range(0, 1f)]
	public float offset; 

	public int 	pan_selected_id;

	bool is_scrolling = false;
	
	Vector2 content_vector;

	public float x_offset;

	public bool zoom_on = true;

	public Canvas canvas;


	public void defineSnapScroll(int size){
		pan_count = size;
		rect_transform = GetComponent<RectTransform>(); 
		pan_instances = new GameObject[pan_count];
		pan_position = new Vector2[pan_count];
		pan_scale = new Vector2[pan_count];

		pan_selected_id = 0;

		for (int i=0; i < pan_count; i++){
			if (i == 0 || i == 7)
				pan_instances[i] = Instantiate(pan_prefab, transform, false);
			else
				pan_instances[i] = Instantiate(pan_prefab2, transform, false);

			if (i == 0){
				pan_position[i] = -pan_instances[i].transform.localPosition;
				pan_position[i] = new Vector2(pan_position[i].x, pan_position[i].y);	
				continue;
			}

			float previous_x = pan_instances[i-1].transform.localPosition.x;
			float pan_width = pan_prefab.GetComponent<RectTransform>().rect.width + x_offset;

			pan_instances[i].transform.localPosition = new Vector2(previous_x + pan_width + pan_offset, 
			pan_instances[0].transform.localPosition.y);
			pan_position[i] = -pan_instances[i].transform.localPosition;
		}

	}

	void FixedUpdate(){
		checkBorder();
		defineSelectedPan();
	}

	void checkBorder(){
		if ((rect_transform.anchoredPosition.x <= pan_position[0].x || 
		rect_transform.anchoredPosition.x >= pan_position[pan_count - 1].x) 
		&& !is_scrolling){
			scroll_rect.inertia = false;
		}
	}

	void defineSelectedPan(){
		float nearest_position = float.MaxValue;

		int previous_id = pan_selected_id;

		for (int i=0; i < pan_count; i++){
			float distance = Mathf.Abs(rect_transform.anchoredPosition.x - pan_position[i].x);

			if (distance < nearest_position){
				nearest_position = distance;
				pan_selected_id = i;
			}

			if (zoom_on){
				float scale = Mathf.Clamp(1 /(distance / pan_offset) * scale_offset, 0.5f, 1f);
				pan_scale[i].x = Mathf.SmoothStep(pan_instances[i].transform.localScale.x + offset, scale, scale_speed * Time.fixedDeltaTime);
				pan_scale[i].y = Mathf.SmoothStep(pan_instances[i].transform.localScale.y + offset, scale, scale_speed * Time.fixedDeltaTime);
				pan_instances[i].transform.localScale = pan_scale[i];
			}
		}

		if (previous_id != pan_selected_id){
			updateBalloon(pan_selected_id);
			if (previous_id == 0)
				updateBalloon(0);
		}

		float scroll_velocity = Mathf.Abs(scroll_rect.velocity.x);

		if (scroll_velocity < 400 && !is_scrolling)
			scroll_rect.inertia = false;


		if (is_scrolling || scroll_velocity > 400) 
			return;

		setContentTransform();
	}


	void updateBalloon(int id){

		if (!PlayerData.checked_news[id]){
			PlayerData.checked_news[id] = true;
			pan_instances[id].GetComponent<PowerUpUI>().updateBalloon();
			DataManager.saveData();
		}
	}

	void setContentTransform(){
		content_vector.x = Mathf.SmoothStep(rect_transform.anchoredPosition.x, pan_position[pan_selected_id].x, snap_speed * Time.fixedDeltaTime);
		rect_transform.anchoredPosition = content_vector;
	}

	public void scrolling(bool scroll){
		is_scrolling = scroll;
		if (scroll)
			scroll_rect.inertia = true;
	}
}
