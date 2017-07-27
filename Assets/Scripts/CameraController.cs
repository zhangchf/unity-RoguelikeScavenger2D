using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	void Start() {
		SetCameraSize ();
	}

	void Update() {
		SetCameraSize ();
	}

	void SetCameraSize () {
//		DeviceOrientation deviceOrientation = Input.deviceOrientation;
//		if (deviceOrientation == DeviceOrientation.LandscapeLeft
//			|| deviceOrientation == DeviceOrientation.LandscapeRight) {
//			Camera.main.orthographicSize = 5f;
//		}
//		else if (deviceOrientation == DeviceOrientation.Portrait
//			|| deviceOrientation == DeviceOrientation.PortraitUpsideDown) {
//			Camera.main.orthographicSize = 8.35f;
//		}

		ScreenOrientation screenOrientation = Screen.orientation;
		if (screenOrientation == ScreenOrientation.Portrait) {
			Camera.main.orthographicSize = 8.35f;
		} else if (screenOrientation == ScreenOrientation.Landscape) {
			Camera.main.orthographicSize = 5f;
		}
	}
}
