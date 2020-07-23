using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static Vector2 spawnLocation;
    public static List<string> queue = new List<string>();
    

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
