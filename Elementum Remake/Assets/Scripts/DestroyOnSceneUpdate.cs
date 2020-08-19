using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyOnSceneUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.activeSceneChanged += SceneChange;
    }

    void SceneChange(Scene current, Scene next)
    {
        Destroy(gameObject);
    }
}
