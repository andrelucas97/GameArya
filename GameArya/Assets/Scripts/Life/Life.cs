using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    public string itemID;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            if (collision.GetComponent<PlayerController>().life <10)
            {
                collision.GetComponent<PlayerController>().life++;
                Destroy(this.gameObject);
            }
            
        }
    }
}
