using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraController : MonoBehaviour
{
    private static bool spawned = false;
    public static CameraController playerCamera;

    public bool freeze;

    public Transform target;
    public float xSpeed;
    public float ySpeed;
    public float limitX;
    public float limitY;

    public Vector3 offset;
    public Vector3 UIoffset;
    public Vector3 smoothedPosition;
    public Vector3 desiredPosition;

    public bool paused;
    public GameObject pauseMenu;

    [Header("Camera Shake")]
    public float xIntensity;
    public float yIntensity;

    private void Awake()
    {
        if (!spawned)
        {
            spawned = true;
            DontDestroyOnLoad(gameObject);
            playerCamera = this;

            pauseMenu.SetActive(false);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = target.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!freeze)
        {
            if (paused)
            {
                if (Input.GetButtonDown("ESC"))
                {
                    paused = false;
                }
                Pause();

            }
            else
            {
                if (Input.GetButtonDown("ESC"))
                {
                    paused = true;
                }
                Resume();
            }

            if (target.position.x < transform.position.x - limitX || target.position.x > transform.position.x + limitX)
            {
                xSpeed *= 1.005f;
            }
            else if (target.position.x < transform.position.x - (limitX * 2) || target.position.x > transform.position.x + (limitX * 2))
            {
                xSpeed *= 1.5f;
            }
            else
            {
                if (xSpeed > 1f)
                {
                    xSpeed *= 0.9f;
                }
                else
                {
                    xSpeed = 1f;
                }
            }
            if (target.position.y < transform.position.y - limitY || target.position.y > target.position.y + limitY)
            {
                ySpeed *= 1.05f;
            }
            else if (target.position.y < transform.position.y - (limitY + 2) || target.position.y > transform.position.y + (limitY + 2))
            {
                ySpeed *= 1.5f;
            }
            else
            {
                if (ySpeed > 2f)
                {
                    ySpeed *= 0.9f;
                }
                else
                {
                    ySpeed = 2f;
                }
            }

            desiredPosition = target.position + offset;
            smoothedPosition.x = Mathf.Lerp(transform.position.x, desiredPosition.x, Time.deltaTime * xSpeed);
            smoothedPosition.y = Mathf.Lerp(transform.position.y, desiredPosition.y, Time.deltaTime * ySpeed);
            smoothedPosition.z = -1;

            transform.position = smoothedPosition;

            transform.GetChild(0).transform.position = transform.position + UIoffset;
            transform.GetChild(1).transform.position = transform.position;
        }
        

    }

    private void Pause()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    private void Resume()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void ButtonResume()
    {
        Resume();
        paused = false;
    }

    public void JumpToTarget()
    {
        transform.position = target.position;
        desiredPosition = target.position;
        smoothedPosition = target.position;
    }

    private void Shake()
    {
        //select a static point to shake around, give the camera two points to oscillate between. alter the speed of the lerp to change insensity
    }
}
