using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float xSpeed;
    public float ySpeed;
    public Vector3 offset;
    public Vector3 UIoffset;
    public Vector3 smoothedPosition;
    public Vector3 desiredPosition;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = target.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target.position.x < transform.position.x - 10 || target.position.x > transform.position.x + 10)
        {
            xSpeed = Mathf.Lerp(xSpeed, xSpeed * 3, 0.001f);
        }
        else
        {
            xSpeed = 0.02f;
        }
        if (target.position.y < transform.position.y - 5 || target.position.y > target.position.y + 5)
        {
            ySpeed = Mathf.Lerp(ySpeed, ySpeed * 3, 0.001f);
        }
        else
        {
            ySpeed = 0.01f;
        }

        desiredPosition = target.position + offset;
        smoothedPosition.x = Mathf.Lerp(transform.position.x, desiredPosition.x, xSpeed);
        smoothedPosition.y = Mathf.Lerp(transform.position.y, desiredPosition.y, ySpeed);
        smoothedPosition.z = -1;

        transform.position = smoothedPosition;

        transform.GetChild(0).transform.position = transform.position + UIoffset;
        transform.GetChild(1).transform.position = transform.position;

    }
}
