using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour
{
    public static bool spawned = false;

    public static Vector2 spawnLocation = new Vector2(-248, 27);
    public static Vector2 cameraTargetSpawn;
    public static int sceneIndex;
    public static List<string> queue = new List<string>();
    public static int deaths;
    public static int bits;
    public static GameObject holding;
    

    private void Awake()
    {
        if (!spawned)
        {
            sceneIndex = 2;
            spawned = true;
            DontDestroyOnLoad(this);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    void Start()
    {
        PlayerController.deathEvent += OnPlayerDeath;
        Bit.collectEvent += OnBitCollect;
    }

    public void OnPlayerDeath()
    {
        deaths++;
    }
    
    public void OnBitCollect()
    {
        bits++;
    }

}
