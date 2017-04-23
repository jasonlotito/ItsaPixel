using UnityEngine;

public class CameraController : MonoBehaviour {

    Transform playerTransform;
   
	void Start () {
            playerTransform = GameObject.FindGameObjectWithTag(Player.Tag).transform;
  
	}
	
	void LateUpdate () {
        if(playerTransform)
        {
            transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
        }
	}
}
