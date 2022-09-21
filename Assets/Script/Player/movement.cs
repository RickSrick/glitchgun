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
    public float linearDrag = 10;

    [Space]
    [Header("Conditions")]
    public bool canMove;
    public bool wallGrab;
    public bool wallJumped;
    private bool changingDirection;

    public int side = 1;


    void Awake()
    {
        collision = GetComponent<collision>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");

        Vector2 dir = new Vector2(x, 0);
        
        changingDirection = (rb.velocity.x > 0f && dir.x < 0f) || (rb.velocity.x < 0f && dir.x > 0f);

        Walk(dir);

        if (collision.onGround)
        {
            wallJumped = false;
            ApplyGroundLinearDrag(dir);
        }
        else { rb.drag = 0f; }

        if (Input.GetButtonDown("Jump"))
        {
            rb.drag = 0f;
            if (collision.onGround) Jump(Vector2.up, false);
            if (collision.onWall && !collision.onGround) WallJump();
        }

    }
    private void ApplyGroundLinearDrag(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) < 0.4f || changingDirection)
        {
            rb.drag = linearDrag;
        }
        else
        {
            rb.drag = 0f;
        }
    }

    private void Walk(Vector2 dir)
    {
        if (!canMove || wallGrab) return;
        if (!wallJumped)
        {
            //rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);

            rb.AddForce(new Vector2(dir.x, 0) * speed * 2);

            if(Mathf.Abs(rb.velocity.x) > speed) { rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * speed, rb.velocity.y); }

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

        Jump((Vector2.up + wallDir), true);

        wallJumped = true;
    }

    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }
}