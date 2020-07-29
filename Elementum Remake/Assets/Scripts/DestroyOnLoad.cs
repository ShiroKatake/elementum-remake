using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyOnLoad : MonoBehaviour
{
    public static List<string> collected = new List<string>();
    public Vector2 spawn;
    public int scene;

    protected void DestroyCollected()
    {
        foreach (string b in collected)
        {
            if (b == SceneManager.GetActiveScene().buildIndex + transform.position.x.ToString() + transform.position.y.ToString())
            {
                Destroy(this.gameObject);
            }
        }
    }

    protected void Collect()
    {
        collected.Add(scene.ToString() + spawn.x.ToString() + spawn.y.ToString());
        Debug.Log("Collected");
        Destroy(this.gameObject);
    }
}
