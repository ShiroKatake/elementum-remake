using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static Vector2 spawnLocation = new Vector2(7, 10);
    public static List<string> queue = new List<string>();
    

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
