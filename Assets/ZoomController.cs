using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomController : MonoBehaviour
{
    public delegate void OnZoomEndsEvent();
    bool startZoomingIn;
    float zoomSpeed;
    private OnZoomEndsEvent callback;


    public void StartZoomingIn(OnZoomEndsEvent _callback)
    {
        StartZooming(10f, true, _callback);
    }

    public void StartZoomingOut(OnZoomEndsEvent _callback)
    {
        StartZooming(10f, false, _callback);
    }

    public void StartZooming(float _zoomSpeed, bool _zoomIn, OnZoomEndsEvent _callback)
    {
        startZoomingIn = true;
        zoomSpeed = _zoomSpeed * (_zoomIn ? 1 : -1);
        callback = _callback;
    }

    public void StartZooming(float _zoomSpeed = 10f, bool _zoomIn = true)
    {
        startZoomingIn = true;
        zoomSpeed = _zoomSpeed * (_zoomIn ? 1 : -1);
    }

    public void StopZooming()
    {
        startZoomingIn = false;
    }

    public void Zoom()
    {
        if (startZoomingIn)
        {
            Camera.main.orthographicSize -= zoomSpeed * Time.fixedDeltaTime;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 2.0f, 1000.0f);

            if (Camera.main.orthographicSize == 2.0f)
            {
                startZoomingIn = false;

                if (callback != null)
                {
                    callback();
                }

                callback = null;
            } else if (Camera.main.orthographicSize == 1000.0f) {
                startZoomingIn = false;

                if (callback != null)
                {
                    callback();
                }

                callback = null;
            }
        }
    }
}
