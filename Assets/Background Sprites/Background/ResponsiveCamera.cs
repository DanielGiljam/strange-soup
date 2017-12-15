using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponsiveCamera : MonoBehaviour
{

    public Vector2 EmulatedResolution = new Vector2(17, 10);
	
	void Awake ()
	{
        Camera cam  = GetComponent<Camera>();
        float screenAspectRatio = (float) Screen.width / Screen.height;
        Debug.Log("Screeen width: " + Screen.width);
        Debug.Log("Screeen height: " + Screen.height);
        Debug.Log("Screen aspect ratio --> " + screenAspectRatio);
        Debug.Log("True 16:9 --> " + 16.0f / 9.0f);

        if (Mathf.Ceil(screenAspectRatio * 100) > 177)
        {
            cam.orthographicSize = (EmulatedResolution.x / (16.0f / 9.0f)) / 2;
        }
	    else
	    {
	        cam.orthographicSize = EmulatedResolution.y / 2;
        }
	}

}
