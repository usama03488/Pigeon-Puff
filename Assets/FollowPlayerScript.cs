using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerScript : MonoBehaviour
{
    public Transform target; // Player's transform
    public float followSpeed = 10f; // Adjust this value to control the speed of camera movement
    public Vector3 offset; // Offset of the camera from the player

    private bool isGoingUp = false;
    public float lastYPosition;

    void Update()
    {
        if (target != null)
        {
            if (target.position.y > lastYPosition)
            {
                isGoingUp = true;
                lastYPosition = target.position.y;
            }
            else
            {
                isGoingUp = false;
            }

          

            if (isGoingUp)
            {
                Vector3 desiredPosition = target.position + offset;
                desiredPosition = new Vector3(-4f, target.position.y + offset.y, 0);
                transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
            }
        }
    }
}
