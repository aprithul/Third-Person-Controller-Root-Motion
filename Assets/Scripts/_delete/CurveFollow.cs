using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveFollow : MonoBehaviour {

	public BezierCurve curve;
	private float t=0;
	private int direction = 1;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = curve.GetPoint (t);

		t += direction * Time.deltaTime;
		if (t > 1) {
			t = 1;
			direction = -direction;
		} else if (t < 0) {
			t = 0;
			direction = -direction;
		}


	}
}
