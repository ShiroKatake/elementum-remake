using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraController : MonoBehaviour
{
    private static bool spawned = false;

    public bool freeze;

    public static Transform target;
    private static bool playerMode;
    public float minimumSpeed;
    public float xSpeed;
    public float ySpeed;
    public float limitX;
    public float limitY;
    private float timeFactor;
    public static float cameraSize;
    public Camera camera;

    public Vector3 offset;
    public Vector3 UIoffset;
    public Vector3 smoothedPosition;

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
        freeze = true;
        SceneController.ScenePhaseChanged += ChangeCameraState;
        PlayerController.playerRespawn += JumpToTarget;
        cameraSize = 12.1f;

        if (SceneController.currentScene.buildIndex != 0 && SceneController.currentScene.buildIndex != 1)
        {
            Initialize();
        }
        
    }

    private void ChangeCameraState(ScenePhase current, ScenePhase next)
    {
        switch (next)
        {
            case ScenePhase.Paused:
                freeze = true;
                break;
            case ScenePhase.Game:
                freeze = false;
                break;
        }
    }

    public void Initialize()
    {
        Debug.Log("initializing camera");
        target = GameObject.Find("Start").transform;
        JumpToTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (!freeze)
        {
            camera.orthographicSize = cameraSize;

            if (playerMode)
            {
                PlayerFollow();
            }
            else
            {
                if (OutOfRange(transform.position.x - limitX, transform.position.x + limitX, target.position.x ))
                {
                    xSpeed *= 1.05f;
                }
                else
                {
                    xSpeed = DampSpeed(xSpeed);
                }
                if (OutOfRange(transform.position.y - limitY, transform.position.y + limitY, target.position.y))
                {
                    ySpeed *= 1.05f;
                }
                else
                {
                    ySpeed = DampSpeed(ySpeed);
                }
            }

            if (Vector3.Distance(smoothedPosition, target.position) > 7.5)
            {
                timeFactor = Time.deltaTime;
            }
            else
            {
                timeFactor = 0.03f;
            }

            smoothedPosition.x = Mathf.Lerp(transform.position.x, target.position.x, timeFactor*xSpeed);
            smoothedPosition.y = Mathf.Lerp(transform.position.y, target.position.y, timeFactor*ySpeed);
            smoothedPosition.z = -1;

            transform.position = smoothedPosition;

            //transform.GetChild(0).transform.position = transform.position + UIoffset;
            //transform.GetChild(1).transform.position = transform.position;
        }
    }

    private bool OutOfRange(float negativeLimit, float positiveLimit, float point)
    {
        if (point < negativeLimit || point > positiveLimit)
        {
            return true;
        }
        return false;
    }

    private float DampSpeed(float speed)
    {
        if (speed > minimumSpeed)
        {
            return speed *= 0.97f;
        }
        else
        {
            return speed = minimumSpeed;
        }
    }
    private void PlayerFollow()
    {
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
    }

    public static void ChangeTarget(Transform newTarget, float distance)
    {
        target = newTarget;
        if (newTarget.gameObject.CompareTag("Player"))
        {
            playerMode = true;
        }
        else
        {
            playerMode = false;
        }

        if (distance == 0)
        {
            cameraSize = 12.1f;
        }
        else
        {
            cameraSize = distance;
        }
    }
    public void Respawn()
    {
        transform.position = GameData.cameraTargetSpawn;
        smoothedPosition = transform.position;
    }

    public void JumpToTarget()
    {
        if (target != null)
        {
            transform.position = new Vector3(target.position.x, target.position.y, -10);
            camera.orthographicSize = cameraSize;
        }
    }

    private void Shake()
    {
        //select a static point to shake around, give the camera two points to oscillate between. alter the speed of the lerp to change insensity
    }
}
