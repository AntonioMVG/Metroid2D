using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public int damage = 1;

    public Vector3 startPosition;
    public Vector3 endPosition;

    private bool movingToEnd;
    private bool upDown;
    private SpriteRenderer sprite;

    private void Start()
    {
        startPosition = transform.position;
        movingToEnd = true;
        if (gameObject.name == "Crab")
        {
            sprite = gameObject.transform.Find("crab-idle-1").GetComponent<SpriteRenderer>();
        }
        else if (gameObject.name == "Octopus")
        {
            sprite = gameObject.transform.Find("octopus-1").GetComponent<SpriteRenderer>();
        }

        // Moving to right
        if (startPosition.x < endPosition.x)
        {
            sprite.flipX = true;
        }
        else if (startPosition.x > endPosition.x)
        {
            sprite.flipX = false;
        }
        else
        {
            sprite.flipX = true;
        }
    }

    private void Update()
    {
        EnemyMove();
    }

    void EnemyMove()
    {
        // Calculate the destination in order to movingToEnd
        Vector3 targetPosition = (movingToEnd) ? endPosition : startPosition;

        // Enemy movement
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            movingToEnd = !movingToEnd;
            if(!upDown) sprite.flipX = !sprite.flipX;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }
}
