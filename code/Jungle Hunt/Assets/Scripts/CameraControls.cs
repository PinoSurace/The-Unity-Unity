using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour {
    private Vector3 cameraTargetPosition;
	
	void FixedUpdate()
    {
        Vector3 playerOffset = new Vector3(-5.0f, 0, 0);

        // This defines how quickly the camera responds to player movement
        const float cameraSpeed = 1.0f;

        Vector3 playerPosition = GameObject.Find("Tarzan").transform.position;
        Vector3 cameraCurrentPosition = this.transform.position;

        // Only change the target position if it's moving the camera forwards from where it was before
        if (playerPosition.x + playerOffset.x < cameraTargetPosition.x)
        {
            cameraTargetPosition = playerPosition + playerOffset;
        }
        
        // Only change camera x coordinate
        float dx = (cameraTargetPosition.x - cameraCurrentPosition.x) * Time.deltaTime * cameraSpeed;
        
        // Move the camera towards its target position
        this.transform.position += new Vector3(dx, 0.0f, 0.0f);
    }
}
