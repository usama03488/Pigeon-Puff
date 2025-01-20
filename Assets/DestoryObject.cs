using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryObject : MonoBehaviour
{

    public Transform LineTransform; // Reference to player's transform
    public Transform cameraTransform; // Reference to the main camera


    public Collider2D LineCollider;
    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        float bottomLimitY = cameraTransform.position.y - cameraTransform.GetComponent<Camera>().orthographicSize; // Account for orthographic camera
        float playerBottomY = LineTransform.position.y - LineCollider.bounds.extents.y; // Adjust for collider

      

        if (playerBottomY < bottomLimitY)
        {
            DestoryGameObject();
        }
    }
    void DestoryGameObject()
    {
        Destroy(gameObject);
    }

}
