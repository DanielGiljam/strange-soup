using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponsiveCamera : MonoBehaviour
{

    Vector2 emulatedResolution = new Vector2(17, 10);
    Vector2 screenAspectRatio;
    Camera cam;
    float emuResFloat;
    float screenARFloat;
	
	void Awake ()
	{
        cam  = GetComponent<Camera>();
        screenAspectRatio = new Vector2(Screen.width, Screen.height);
        emuResFloat = emulatedResolution.x / emulatedResolution.y;
        screenARFloat = screenAspectRatio.x / screenAspectRatio.y;
        if (screenARFloat > emuResFloat)
        {
            cam.orthographicSize = (emulatedResolution.x / screenARFloat) / 2;
        }
	    else
	    {
	        cam.orthographicSize = emulatedResolution.y / 2;
        }
        Debug.Log("Screen width: " + Screen.width + ", Screen height: " + Screen.height + "\nScreen aspect ratio: " + screenARFloat + ", Emulated resolution aspect ratio: " + emuResFloat);
	}

}
