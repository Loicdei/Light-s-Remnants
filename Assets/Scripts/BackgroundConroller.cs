using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundConroller : MonoBehaviour
{
    private float startPos;
    public GameObject cam;
    public float parallaxEffect;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distance = cam.transform.position.x * parallaxEffect; //0 = bouge avec la cam, 1 = reste fixe, 0.5 = moitié de la vitesse de la cam

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
    }
}
