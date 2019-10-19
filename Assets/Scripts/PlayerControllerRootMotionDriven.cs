using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerRootMotionDriven : MonoBehaviour {


	public float player_move_speed;
	public Transform move_direction_calculator;
	public Transform mesh_transform;

	[HideInInspector]
	private Vector3 camera_direction;


	private Animator animation_controller;
	private Vector3 player_move_vector;
	private Vector3 user_input;
	private Camera player_camera;
	private Vector3 player_last_position;
	private Vector3 camera_last_position;


	// Use this for initialization
	void Awake () {
		animation_controller = mesh_transform.GetComponent<Animator> ();
		player_camera = Camera.main;
	}

	void Start()
	{

	}


	// Update is called once per frame
	void Update () {

		// set camera direction to use as reference for player's forward direction
		set_camera_direction(player_camera.transform.forward);

		Debug.DrawRay (transform.position, camera_direction*5, Color.blue);

		// get user input
		user_input = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));
		// transform user input relative to the direction of camera, use a extra transform for convinience
		move_direction_calculator.position = transform.position;
		move_direction_calculator.forward = camera_direction;
		user_input = move_direction_calculator.TransformDirection (user_input);
		user_input.Normalize ();

		Debug.DrawRay (transform.position, user_input*5, Color.green);

		// set the player_move_vector to appropriate direction and value
		player_move_vector = user_input;
		player_move_vector *= player_move_speed;

		// 
		player_last_position = transform.position;
		camera_last_position = player_camera.transform.position;
		camera_last_position.y = player_last_position.y;

		// apply movement
		CurveController.Rotate(0, Mathf.Abs(Vector3.Angle (camera_last_position - player_last_position, camera_last_position - transform.position)) * (Input.GetAxisRaw("Horizontal")),0);


		// set animations
		// must be changed


		//if(player_move_vector.sqrMagnitude > 0.1f)
		//	mesh_transform.rotation = Quaternion.LookRotation( player_move_vector, mesh_transform.up);


		animation_controller.SetFloat("speed", user_input.sqrMagnitude);
		animation_controller.SetFloat ("direction", Input.GetAxis ("Horizontal"));
	}


	void set_camera_direction (Vector3 direction)
	{
		camera_direction = direction;
		camera_direction.y = 0f;
		camera_direction.Normalize ();
	}
}
