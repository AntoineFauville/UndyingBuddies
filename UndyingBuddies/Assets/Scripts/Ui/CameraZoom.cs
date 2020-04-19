using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private GameObject Camera;

    [SerializeField] private int maxZoomOut = 65;
    [SerializeField] private int maxZoomIn = 40;

    int actualZoom;

    [SerializeField] private float smooth = 5;
    [SerializeField] private int speed = 5;

    // Start is called before the first frame update
    void Start()
    {
        actualZoom = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (actualZoom >= maxZoomOut)
            {
                actualZoom = maxZoomOut;
            }
            else
            {
                actualZoom += speed;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (actualZoom <= maxZoomIn)
            {
                actualZoom = maxZoomIn;
            }
            else
            {
                actualZoom -= speed;
            }
        }
        
        Camera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(Camera.GetComponent<Camera>().fieldOfView, actualZoom, Time.deltaTime * smooth);
    }
}
