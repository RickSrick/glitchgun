using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    private collision collision;
    [HideInInspector] public Rigidbody2D rb;

    [Space]
    [Header("Values")]
    public float speed = 10;
    public float wallJumpLerp = 10;
    public float jumpForce = 10;

    [Space]
    [Header("Conditions")]
    public bool canMove;
    public bool wallGrab;
    public bool wallJumped;

    public int side = 1;


    void Awake()
    {
        collision = GetComponent<collision>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");

        Vector2 dir = new Vector2(x, y);
        Walk(dir);

        if (Input.GetButtonDown("Jump"))
        {
            if (collision.onGround) Jump(Vector2.up, false);
            if (collision.onWall && !collision.onGround) WallJump();
        }

    }

    private void Walk(Vector2 dir)
    {
        if (!canMove || wallGrab) return;
        if (!wallJumped)
        {
            rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * speed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
        }
    }

    private void Jump(Vector2 dir, bool wall)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpForce;
    }

    private void WallJump()
    {
        if ((side == 1 && collision.onRightWall) || side == -1 && !collision.onRightWall)
        {
            side *= -1;
        }

        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));

        Vector2 wallDir = collision.onRightWall ? Vector2.left : Vector2.right;

        Jump((Vector2.up / 1.5f + wallDir / 1.5f), true);

        wallJumped = true;
    }

    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }
}