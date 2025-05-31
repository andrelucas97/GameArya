using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private Vector3 target;

    void Start()
    {
        target = pointB.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            target = target == pointA.position ? pointB.position : pointA.position;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Transform playerParent = collision.transform.parent;
            if (playerParent != null)
            {
                playerParent.SetParent(transform);
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Transform playerParent = collision.transform.parent;
            if (playerParent != null)
            {
                playerParent.SetParent(null);
            }
        }
    }

    void MovePlatform(Collision2D collision)
    {
        Transform playerParent = collision.transform.parent;
        if (playerParent != null)
        {
            playerParent.SetParent(null);
        }
    }
}
