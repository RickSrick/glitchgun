using System;
using UnityEngine;

/*
 * @author: RickSrick
 * 
 * the green platforms follow a path in loop, 
 * if you hit a green platform with green bullet, the platform change its state:
 * 
 *  - from moving to stop
 *  - from stop to moving
 */

public class green_platform : MonoBehaviour
{
    enum moveSet { rotate, move }

    [SerializeField] bool isMoving;
    [SerializeField] moveSet currentMovesSet;
    [Space][SerializeField][Range(0, 500)] float speed;
    [Space][SerializeField] bool onLoop;
    [SerializeField] Vector2[] steps;

    private int stepsCounter = 0;
    private bool block = false;
    private Array localSteps;

    private void Awake()
    {
        localSteps = Array.CreateInstance(typeof(Vector2), steps.Length);
        localSteps = steps;
    }
    void Update()
    {
        if (!isMoving) return;

        switch (currentMovesSet)
        {
            case moveSet.rotate:
                transform.Rotate(Vector3.forward * speed * Time.deltaTime, Space.World);
            break;

            case moveSet.move:
                Move();
            break;
        }
    }

    private void Move()
    {
        if (steps.Length <= 0 || block) return;

        transform.localPosition = Vector3.MoveTowards(transform.position, steps[stepsCounter+1], speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, steps[stepsCounter + 1]) <= 0.1)
        {
            if(stepsCounter +1 >= steps.Length -1)
            {
                if (onLoop)
                {
                    Array.Reverse(localSteps);
                    stepsCounter = 0;
                }
                else block = true;
                return;
            }

            stepsCounter++;

        }

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("greenBullet"))
        {
            //add block Animation
            isMoving = !isMoving;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(null);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && currentMovesSet != moveSet.rotate) collision.gameObject.transform.SetParent(transform);
    }
}