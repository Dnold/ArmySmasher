using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    public float speed;

    public float zoomSensitivity = 100f;
    public float maxZoom = 16f;
    public float minZoom = 4.1f;
    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        transform.Translate(new Vector3(x, y, 0) * speed * Time.deltaTime);

        float scroll = Input.mouseScrollDelta.y * zoomSensitivity * Time.deltaTime;
        if (scroll != 0f)
        {
            Camera.main.orthographicSize += scroll;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);
        }
        KeepInSquare();
    }
    public void KeepInSquare()
    {
        if(transform.position.x > 30)
        {
            transform.position = new Vector3(-30,transform.position.y, -10);

        }
        if(transform.position.x < -30)
        {
            transform.position = new Vector3(30, transform.position.y, -10);
        }
        if (transform.position.y > 25)
        {
            transform.position = new Vector3(transform.position.x, -25, -10);
        }
        if (transform.position.y < -25)
        {
            transform.position = new Vector3(transform.position.x, 25, -10);
        }
    }
}
