using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {

    Transform playerTransform;
    //new Camera camera;

	// Use this for initialization
	void Start () {
        playerTransform = GameObject.FindGameObjectWithTag(Player.Tag).transform;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y,transform.position.z);
	}

    private void FixedUpdate()
    {
        //Camera.main.orthographicSize += 0.1f * Time.fixedDeltaTime;
    }
}
