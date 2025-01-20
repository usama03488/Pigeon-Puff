using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f; // Adjust this value to control the jump height
    private Rigidbody2D rb;
    public Camera mainCamera;


    public Collider2D playerCollider;
  
    private bool isFalling = false;
    public bool isCollide;

    public bool isJumpBlock;
    private Vector2 touchStartPos;
    public AudioSource JumpSound;

    void Start()
    {
        Time.timeScale = 1;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isCollide) // Change KeyCode.Space to the desired key
        {
            Jump();
         // Disable collider while jumping
        }


        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Get the first touch

            // Check if touch phase is moving
            if (touch.phase == TouchPhase.Moved)
            {
                // Calculate the direction of the touch movement
                float direction = Mathf.Sign(touch.deltaPosition.x);

                if(touch.deltaPosition.x<0)
                {
                    transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
                }
                else
                {
                   transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                }

                // Move the player horizontally
             //     transform.Translate(new Vector2(direction * moveSpeed, rb.velocity.y));
                rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
            }
        }


        if (rb.velocity.y < 0)
        {
            playerCollider.enabled = true;
        }
        else
        {
            playerCollider.enabled = false;
        }

        if(isJumpBlock)
        {
            FlyPlayerUpword();
        }
        else
        {
         //  Debug.Log("transform position x");
            RigidBodyxPosition = transform.position.x;
        }

        ClampPlayerPosition();
    }
    float RigidBodyxPosition;
    private void FixedUpdate()
    {

    




        // Moving left
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            MoveLeft();
        }

        // Moving right
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            MoveRight();
        }
    }
    void Jump()
    {
        if (!isJumpBlock)
        {

            rb.velocity = Vector2.up * jumpForce;
            isCollide = false;
        }
    }
 public   void MoveLeft()
    {
        //  rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
      
        transform.Translate(new Vector2(-moveSpeed, rb.velocity.y));
    }

    public void MoveRight()
    {
     
        // rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        transform.Translate(new Vector2(-moveSpeed, rb.velocity.y));

    }
    void ClampPlayerPosition()
    {
        
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);
        viewportPosition.x = Mathf.Clamp01(viewportPosition.x); // Clamp between 0 and 1

        // If the player reaches the left edge, wrap around to the right edge
        if (viewportPosition.x <= 0f)
        {
           
            viewportPosition.x = 0.8f;
            rb.AddForce(new Vector2(-2, rb.velocity.y));
            //rb.velocity = new Vector2(-2, rb.velocity.y);
        }
        // If the player reaches the right edge, wrap around to the left edge
        else if (viewportPosition.x >= 1f)
        {
           //transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            viewportPosition.x = 0.2f;
            rb.AddForce(new Vector2(2, rb.velocity.y));
            // rb.velocity = new Vector2(2, rb.velocity.y);
        }

        transform.position = mainCamera.ViewportToWorldPoint(viewportPosition);

      //  Debug.Log(viewportPosition + " View Poirt");
      }

    public void FlyPlayerUpword()
    {

        //  Debug.Log("transform position x Update"+ RigidBodyxPosition);
        rb.gravityScale = 0;
        transform.Translate(new Vector3(rb.velocity.x, 8) * Time.deltaTime);
       // transform.Translate(new Vector3(rb.velocity.x, 8) * Time.deltaTime);
      //  rb.AddForce(new Vector3(rb.velocity.x, 4)*Time.deltaTime );
    }
    public void SpringAbility()
    {

        rb.velocity = Vector2.up * 25f;
    }

    public void FlyPlayerCorutineCallback()
    {
        StartCoroutine(FlyPlayerAfterSeconds());
    }
    IEnumerator FlyPlayerAfterSeconds()
    {
        yield return new WaitForSeconds(4f);
       GetComponent<Rigidbody2D>().gravityScale = 3;
        GetComponent<PlayerController>().isJumpBlock = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag=="SimpleObstacle")
        {
            JumpSound.Play();
            isCollide = true;
        }
        if (collision.collider.tag == "OneJump")
        {
            JumpSound.Play();
            isCollide = true;
            Destroy(collision.collider.gameObject);
        }
        if (collision.collider.tag == "NoJump")
        {
            JumpSound.Play();
            Destroy(collision.collider.gameObject);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "SimpleObstacle")
        {

        }
    }

}
