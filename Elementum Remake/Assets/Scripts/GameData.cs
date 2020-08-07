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

    public void StartGame()
    {
        Animator animator = GameObject.Find("Menu Camera/Pause Menu/Veil").GetComponent<Animator>();
        animator.gameObject.SetActive(true);
        animator.SetBool("fadeOut", true);


        StartCoroutine(LoadScene(2, 2));
    }

    public IEnumerator LoadScene(int scene, float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(scene);
        player.SetActive(true);
        playerCamera.SetActive(true);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadMenuFromGame()
    {
        spawnLocation = player.transform.position;
        playerCamera.GetComponent<CameraController>().ButtonResume();
        player.SetActive(false);
        playerCamera.SetActive(false);
        LoadMenu();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
