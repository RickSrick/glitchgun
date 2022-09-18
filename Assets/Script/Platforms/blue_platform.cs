using UnityEngine;

/*
 * @author: RickSrick
 * 
 * the blue platforms allow player to jump, are unmovable and unbreakable
 * you don't need any bullet type to active or deactive.
 */

public class blue_platform : MonoBehaviour
{
    [SerializeField][Range(0, 100)] float jumpForce;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D collisionRb = collision.gameObject.GetComponent<Rigidbody2D>();

        if(collisionRb != null)
        {
            collisionRb.velocity = Vector2.zero;
            //the force is relative to angle of the up vector of platform
            collisionRb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}