using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static Vector2 spawnLocation;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
