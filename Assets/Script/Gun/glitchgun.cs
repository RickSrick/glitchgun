using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class glitchgun : MonoBehaviour
{
    [Header("bullets type")]
    [SerializeField] GameObject redBullet;
    [SerializeField] GameObject greenBullet;

    [Header("number of bullets")]
    [SerializeField] int redLoader;
    [SerializeField] int greenLoader;


    [Space]
    [SerializeField] Vector2 offset;

    private SpriteRenderer gunRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        gunRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        gameObject.transform.GetChild(0).localPosition = offset; 
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        targetDir.Normalize();

        float rotationZ = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;

        if (Mathf.Abs(rotationZ) > 95) gunRenderer.flipY = true;
        else if (Mathf.Abs(rotationZ) < 85) gunRenderer.flipY = false;

        transform.rotation = Quaternion.Euler(0, 0, rotationZ);

        if (Input.GetButtonDown("Fire1") && redLoader > 0)
        {
            Instantiate(redBullet, gameObject.transform.GetChild(0).position, transform.rotation);
            redLoader--;
        }
        if (Input.GetButtonDown("Fire2") && greenLoader > 0)
        {
            Instantiate(greenBullet, gameObject.transform.GetChild(0).position, transform.rotation);
            greenLoader--;
        }
    }
}
