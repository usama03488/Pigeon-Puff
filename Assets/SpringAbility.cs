using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringAbility : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
       // Debug.Log(collision.transform.name);
        if (collision.transform.tag == "Player")
        {
            GetComponent<SpriteRenderer>().enabled = false;
            collision.transform.root.GetComponent<PlayerController>().SpringAbility();

            Destroy(gameObject);
        }
    }


 /*   public IEnumerator FlyPlayer(GameObject Player)
    {
        yield return new WaitForSeconds(4f);
        Player.transform.GetComponent<Rigidbody2D>().gravityScale = 3;
        Player.GetComponent<PlayerController>().isJumpBlock = false;
    }*/
}
