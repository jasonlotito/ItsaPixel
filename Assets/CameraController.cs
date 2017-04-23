using UnityEngine;

public class CameraController : MonoBehaviour {

    Transform playerTransform;
   
	void Start () {
        try
        {
            playerTransform = GameObject.FindGameObjectWithTag(Player.Tag).transform;
        } catch
        {

        }
	}
	
	void LateUpdate () {
        if(playerTransform)
        {
            transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
        }
	}
}
