using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour
{
    public static bool spawned = false;

    public static Vector2 spawnLocation = new Vector2(-14, -7);
    public static List<string> queue = new List<string>();
    public static int bits;
    public static GameObject holding;
    public GameObject player;
    public GameObject playerCamera;
    

    private void Awake()
    {


        
        if (!spawned)
        {
            spawned = true;
            DontDestroyOnLoad(this);

            player = GameObject.Find("Player");
            playerCamera = GameObject.Find("Player Camera");
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                player.SetActive(false);
                playerCamera.SetActive(false);
            }
        }
        else
        {
            DestroyImmediate(gameObject);
        }
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        player.SetActive(true);
        playerCamera.SetActive(true);
    }

    public void LoadMenu()
    {
        spawnLocation = player.transform.position;
        playerCamera.GetComponent<CameraController>().ButtonResume();
        player.SetActive(false);
        playerCamera.SetActive(false);
        SceneManager.LoadScene(0);

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
