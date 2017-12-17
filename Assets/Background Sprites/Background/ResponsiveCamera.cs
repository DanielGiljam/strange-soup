using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponsiveCamera : MonoBehaviour
{

    Vector2 optimalAspectRatio = new Vector2(17, 10);
    Vector2 screenAspectRatio;
    Camera cam;
    float optimalARFloat;
    float screenARFloat;
	
	void Awake ()
	{
        cam  = GetComponent<Camera>();
        screenAspectRatio = new Vector2(Screen.width, Screen.height);
        optimalARFloat = optimalAspectRatio.x / optimalAspectRatio.y;
        screenARFloat = screenAspectRatio.x / screenAspectRatio.y;
        if (screenARFloat > optimalARFloat)
        {
            cam.orthographicSize = (optimalAspectRatio.x / screenARFloat) / 2;
        }
	    else
	    {
	        cam.orthographicSize = optimalAspectRatio.y / 2;
        }
        Debug.Log("Screen width: " + Screen.width + ", Screen height: " + Screen.height + "\nScreen aspect ratio: " + screenARFloat + ", Optimal aspect ratio: " + optimalARFloat);
	}

}
