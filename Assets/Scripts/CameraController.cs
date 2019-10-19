using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {


	public Transform target;
	public BezierCurve camera_curve;
	public float mouse_vertical_speed;
	public float mouse_horizontal_speed;
	public float vertical_slowdown_limit;
	public float smoothing_value;
	public bool smoothing;
	public float max_field_of_view;
	public float min_field_of_view;

	private float follow_speed;
	private float current_camera_distance_from_target;
	private float t_value;	// distance in curve
	private Vector3 camera_target_position;	
	private Camera this_camera_reference;
	private float last_vertical_mouse_value;
	private float mouse_vertical_speed_factor;

	private float smooth_target_t_value;
	private float target_field_of_view;

	void Awake(){

		this_camera_reference = GetComponent<Camera> ();
		//Cursor.lockState = CursorLockMode.Locked;
	}

	// Use this for initialization
	void Start () {
		mouse_vertical_speed_factor = 1f;
		t_value = 0.4f;
		smooth_target_t_value = t_value;
		last_vertical_mouse_value = 0f;
	}
	
	// Update is called once per frame
	void LateUpdate () {

		// direct mapping to user input, maybe jerky but accurate
		if (!smoothing) {		
			// horizontal movement
			CurveController.Rotate (0, Input.GetAxisRaw ("Mouse X") * mouse_horizontal_speed * Time.deltaTime, 0,smoothing_value);
			// vertical movment
			// calculate new t_value
			t_value += Input.GetAxisRaw("Mouse Y") * Time.deltaTime * mouse_vertical_speed;
			t_value = Mathf.Clamp (t_value, 0f, 1f);
			camera_target_position = camera_curve.GetPoint (t_value);
		}
		else // smoothened camera movement
		{
			// horizontal movement
			// calculates proper rotation values, and updates in during the next update
			CurveController.Rotate(0,Input.GetAxisRaw("Mouse X") * mouse_horizontal_speed * Time.deltaTime, 0, smoothing_value);
			// vertical movment
			// calculate new target t value and lerp towards it
			smooth_target_t_value += Input.GetAxisRaw("Mouse Y") * Time.deltaTime * mouse_vertical_speed;
			smooth_target_t_value = Mathf.Clamp (smooth_target_t_value, 0f, 1f);

			t_value = Mathf.Lerp (t_value, smooth_target_t_value, smoothing_value*Time.deltaTime);
			camera_target_position = camera_curve.GetPoint (t_value);
		
		}

		// change filed of view depending on pitch
		target_field_of_view = min_field_of_view + (max_field_of_view - min_field_of_view) * t_value;
		this_camera_reference.fieldOfView = target_field_of_view;

		// place camera at target
		transform.position = camera_target_position;
		// camera normally looks at the direction of just above the player head
		transform.LookAt (target);	

	}



}	
