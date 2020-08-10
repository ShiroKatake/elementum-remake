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
            
        }
        else
        {
            DestroyImmediate(gameObject);
        }
        
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 1)
        {
            player.SetActive(false);
            playerCamera.SetActive(false);
        }
    }
}
