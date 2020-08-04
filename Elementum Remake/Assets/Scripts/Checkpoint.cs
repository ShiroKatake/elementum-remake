using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject spawn;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameData.spawnLocation = spawn.transform.position;
    }
}
