using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundConroller : MonoBehaviour
{
    private float startPos, length;
    public GameObject cam;
    public float parallaxEffect;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void LateUpdate()
    {
        float distance = cam.transform.position.x * parallaxEffect; //0 = bouge avec la cam, 1 = reste fixe, 0.5 = moitié de la vitesse de la cam
        float movement = cam.transform.position.x * ( 1 - parallaxEffect );

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        //ajustement de position pour scrolling infini du bg
        if (movement > startPos + length)
        {
            startPos += length;
        }
        else if (movement < startPos - length)
        {
            startPos -= length;
        }
    }
}
