using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomController : MonoBehaviour
{
    public delegate void OnZoomEndsEvent();
    bool startZoomingIn;
    float zoomSpeed;
    private OnZoomEndsEvent callback;
    float lerp;
    private bool startZoomingOut;

    public void StartZoomingIn(OnZoomEndsEvent _callback)
    {
        StartZooming(true, _callback);
    }

    public void StartZoomingOut(OnZoomEndsEvent _callback)
    {
        StartZooming(false, _callback);
    }

    public void StartZooming(bool zoomIn,  OnZoomEndsEvent _callback)
    {
        Camera.main.GetComponent<AudioSource>().Play();
        startZoomingIn = zoomIn;
        startZoomingOut = !zoomIn;
        callback = _callback;
        lerp = 0;
    }

    public void StopZooming()
    {
        lerp = 0;
        startZoomingIn = false;
        startZoomingOut = false;

    }

    public void Update()
    {
        if (startZoomingIn)
        {
            if ( lerp < 1 )
            {
                lerp += Time.deltaTime / 13f;
                Camera.main.orthographicSize = Mathf.Lerp(310f, 2f, lerp);
            } else
            {
                Camera.main.orthographicSize = 2f;
                StopZooming();

                if (callback != null)
                {
                    callback();
                }

                callback = null;
            }
        }

        if (startZoomingOut)
        {
            if (lerp < 1)
            {
                lerp += Time.deltaTime / 13f;
                Camera.main.orthographicSize = Mathf.Lerp(2f, 310f, lerp);
            }
            else
            {
                Camera.main.orthographicSize = 310f;
                StopZooming();

                if (callback != null)
                {
                    callback();
                }

                callback = null;
            }
        }
    }
}
