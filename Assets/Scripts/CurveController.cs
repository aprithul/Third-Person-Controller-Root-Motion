using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveController : MonoBehaviour {

	private static CurveController _instance; 
	private static float smoothing_value = 0f;
	private static Quaternion smooth_target_rotation = Quaternion.identity;

	// Use this for initialization
	void Awake () {
		_instance = this;
		smooth_target_rotation = transform.rotation;
	}
	
	// Update is called once per frame
	void LateUpdate () {

		_instance.transform.rotation = Quaternion.Lerp (_instance.transform.rotation, smooth_target_rotation, smoothing_value * Time.deltaTime);
	}

	/*
	public static void Rotate(float x, float y, float z)
	{
		if (_instance != null) {

			_instance.transform.Rotate (x, y, z);
		} else
			print ("_instance not initated");
	}*/


	public static void Rotate(float x, float y, float z, float smoothing_value)
	{
		if (_instance != null) {
			CurveController.smoothing_value = smoothing_value;
			smooth_target_rotation = smooth_target_rotation * Quaternion.Euler (x, y, z);

		}
		else
			print ("_instance not initated");
	}

	public static void Rotate(float x, float y, float z)
	{
		if (_instance != null) {
			smooth_target_rotation = smooth_target_rotation * Quaternion.Euler (x,y,z);

		}
		else
			print ("_instance not initated");
	}
}
