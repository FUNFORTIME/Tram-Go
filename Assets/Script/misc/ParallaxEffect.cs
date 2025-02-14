using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBagckground : MonoBehaviour
{
    private GameObject cam;

    [SerializeField] private float parallaxEffect;

    private float xPosition;
    public float length=0;

    void Start()
    {
        cam = GameObject.Find("Main Camera");

        if(length==0)length = GetComponent<SpriteRenderer>().bounds.size.x;
        xPosition = transform.position.x;
    }

    void Update()
    {
        float distanceMoved = cam.transform.position.x * (1 - parallaxEffect);
        float distanceToMove = cam.transform.position.x * parallaxEffect;

        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);

        float len = 1.0f*length;
        if (distanceMoved > xPosition + len)
            xPosition = xPosition + len;
        else if (distanceMoved < xPosition - len)
            xPosition = xPosition - len;
    }
}
