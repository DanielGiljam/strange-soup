using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponsiveCamera : MonoBehaviour
{

    public Vector2 EmulatedResolution = new Vector2(272, 160);
	
	void Awake ()
	{
	    var camera  = GetComponent<Camera>();
        var screenAspectRatio = (float) Screen.width / Screen.height;
        Debug.Log(screenAspectRatio);

        if (screenAspectRatio > 16.0f / 9.0f)
        {

            camera.orthographicSize = (EmulatedResolution.x / screenAspectRatio) / 2;

        }
	    else
	    {
	        camera.orthographicSize = EmulatedResolution.y / 2;
        }
	}
}
