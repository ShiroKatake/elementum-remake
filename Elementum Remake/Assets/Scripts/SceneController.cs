using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public enum ScenePhase
{
    Loading,
    Open,
    Game,
    Paused,
    Cinematic,
    Close,
    Save
}


public class SceneController : MonoBehaviour
{
    private static bool spawned = false;

    public static Scene currentScene;
    public static ScenePhase phase;
    private bool paused;


    private PlayerController player;
    private CameraController playerCamera;
    public TypeWriterEffect typer;

    private SpriteRenderer sceneVeil;
    private TMP_Text bits;
    private Animator sceneFadeIn;
    private Animator deathFade;

    // Start is called before the first frame update
    private void Awake()
    {
        if (!spawned)
        {
            spawned = true;
            DontDestroyOnLoad(gameObject);


            SceneManager.activeSceneChanged += UpdateScene;
            player = GameObject.Find("Player").GetComponent<PlayerController>();
            playerCamera = GameObject.Find("Player Camera").GetComponent<CameraController>();
            sceneFadeIn = GameObject.Find("Player Camera/UI").GetComponent<Animator>();
            sceneVeil = GameObject.Find("Player Camera/UI/Scene Fade In").GetComponent<SpriteRenderer>();
            deathFade = GameObject.Find("Player Camera/UI/Death Fade").GetComponent<Animator>();
            typer.display = GameObject.Find("Player Camera/UI/Scene Intro").GetComponent<TMP_Text>();
            bits = GameObject.Find("Player Camera/UI/Bits").GetComponent<TMP_Text>();
            
        }
        else
        {
            DestroyImmediate(gameObject);
        }
        
    }

    private void UpdateScene(Scene current, Scene next)
    {
        phase = ScenePhase.Loading;
        typer.text = next.name;
        currentScene = next;
    }

    private void Start()
    {
        SceneManager.activeSceneChanged += UpdateScene;
        PlayerController.deathEvent += OnPlayerDeath;
        ExitInteraction.doorEvent += TransitionToNextScene;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 1)
        {
            player.gameObject.SetActive(false);
            playerCamera.gameObject.SetActive(false);
        }
        if (phase == ScenePhase.Loading)
        {
            //load the level

            //reset some transition variables
            sceneVeil.color = new Color(sceneVeil.color.r, sceneVeil.color.g, sceneVeil.color.b, 255);
            bits.color = new Color(bits.color.r, bits.color.g, bits.color.b, 0);

            phase = ScenePhase.Open;
        }
        if (phase == ScenePhase.Open)
        {
            if (currentScene.buildIndex != 0)
            {
                player.cinematicOverride = true;
                player.disabled = true;
                StartCoroutine(FadeIn());
            }
            phase = ScenePhase.Cinematic;
        }
        else if (phase == ScenePhase.Game)
        {
            if (paused)
            {
                if (Input.GetButtonDown("ESC"))
                {
                    paused = false;
                    Resume();
                }
            }
            else
            {
                if (Input.GetButtonDown("ESC"))
                {
                    paused = true;
                    Pause();
                }
            }
        }
    }

    public void OnPlayerDeath()
    {
        playerCamera.freeze = true;
        deathFade.SetBool("fadeIn", true);
        ChangeScenePhase(ScenePhase.Cinematic);
        StartCoroutine(Respawn());
    }

    public void Pause()
    {
        Time.timeScale = 0;
        playerCamera.pauseMenu.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        playerCamera.pauseMenu.SetActive(false);
    }

    public void ButtonResume()
    {
        Resume();
        phase = ScenePhase.Game;
    }

    public void LoadMenu()
    {
        ButtonResume();
        SceneManager.LoadScene(0);
    }

    public void LoadMenuFromGame()
    {
        GameData.spawnLocation = player.transform.position;
        ButtonResume();
        player.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(false);
        LoadMenu();
    }

    public void StartGame()
    {
        Animator animator = GameObject.Find("Menu Camera/Pause Menu/Veil").GetComponent<Animator>();
        animator.gameObject.SetActive(true);
        animator.SetTrigger("fadeOut");
        StartCoroutine(LoadScene(2, 2));
    }

    public IEnumerator LoadScene(int scene, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(scene);
        player.gameObject.SetActive(true);
        playerCamera.gameObject.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1f);
        player.Respawn();
        playerCamera.JumpToTarget();
        deathFade.SetBool("fadeIn", false);
        StartCoroutine(ChangeScenePhaseOnDelay(ScenePhase.Game, 0.3f));
    }

    IEnumerator ChangeScenePhaseOnDelay(ScenePhase scenePhase, float delay)
    {
        yield return new WaitForSeconds(delay);
        ChangeScenePhase(scenePhase);
    }



    private void ChangeScenePhase(ScenePhase scenePhase)
    {
        phase = scenePhase;
        if (phase == ScenePhase.Game)
        {
            player.disabled = false;
            player.cinematicOverride = false;
            playerCamera.freeze = false;
        }
        else
        {
            player.disabled = true;
            player.cinematicOverride = true;
        }
    }

    private void TransitionToNextScene(Vector2 destination, string scene)
    {
        sceneFadeIn.SetBool("Open", false);
        StartCoroutine(Transition(destination, scene));

        
    }

    IEnumerator Transition(Vector2 destination, string scene)
    {
        yield return new WaitForSeconds(1);

        //Saves Scene Data to GameData
        GameData.queue.Clear();
        player.GetComponent<PlayerAbility>().queue.Save();

        GameData.spawnLocation = destination;
        //Change Scene
        SceneManager.LoadScene(scene);
    }

    IEnumerator FadeIn()
    {
        typer.Type();
        sceneFadeIn.SetBool("Open", true);
        yield return new WaitForSeconds(2);

        ChangeScenePhase(ScenePhase.Game);
    }
}
