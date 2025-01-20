using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingLine : MonoBehaviour
{
    public int Direction;
    public Camera mainCamera;
    public float MovingSpeed;
    // Start is called before the first frame update
    void Start()
    {
       Direction = Random.Range(0, 2);
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(Direction==0)
        {
            transform.Translate(new Vector3(MovingSpeed, 0, 1) * Time.deltaTime);
        }
        else
        {
            transform.Translate(new Vector3(-MovingSpeed, 0, 1) * Time.deltaTime);
        }
        ClampPlayerPosition();
    }
    void ClampPlayerPosition()
    {

        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);
        viewportPosition.x = Mathf.Clamp01(viewportPosition.x); // Clamp between 0 and 1

        // If the player reaches the left edge, wrap around to the right edge
        if (viewportPosition.x <= 0f)
        {

            viewportPosition.x = 1f;
            //rb.AddForce(new Vector2(-2, rb.velocity.y));
            //rb.velocity = new Vector2(-2, rb.velocity.y);
        }
        // If the player reaches the right edge, wrap around to the left edge
        else if (viewportPosition.x >= 1f)
        {

            viewportPosition.x = 0f;
          //  rb.AddForce(new Vector2(2, rb.velocity.y));
            // rb.velocity = new Vector2(2, rb.velocity.y);
        }

        transform.position = mainCamera.ViewportToWorldPoint(viewportPosition);


    }
}
