using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterAbility : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
     //   Debug.Log(collision.transform.name);
        if (collision.transform.tag == "Detector")
        {
            GetComponent<SpriteRenderer>().enabled = false;
            collision.transform.root.GetComponent<PlayerController>().isJumpBlock = true;
            collision.transform.root.GetComponent<Rigidbody2D>().gravityScale = 0;
            //StartCoroutine(FlyPlayer(collision.transform.root.gameObject));
            collision.transform.root.GetComponent<PlayerController>().FlyPlayerCorutineCallback();
            Debug.Log("Trigger");
        }
    }
   
 /*
    public IEnumerator FlyPlayer(GameObject Player)
    {
        Debug.Log("Fly player");
        yield return new WaitForSeconds(4f);
        Debug.Log("Fly player After");
        Player.transform.GetComponent<Rigidbody2D>().gravityScale = 3;
        Player.GetComponent<PlayerController>().isJumpBlock = false;
    }*/
}
