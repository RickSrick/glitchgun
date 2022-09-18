using UnityEngine;

/*
 * @author: RickSrick
 * 
 * the red platforms are destroyable and unmovable
 * a red platform can only be destroyed with red bullet 
 */

public class red_platform : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("redBullet"))
        {
            //TODO: start destroy animation
            Destroy(this.gameObject);
        }
    }
}