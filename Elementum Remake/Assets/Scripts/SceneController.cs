using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public enum ScenePhase
{
    NewGameMenu,
    Loading,
    Open,
    Game,
    Paused,
    Cinematic,
    Dialogue,
    Close,
    Save
}


public class SceneController : MonoBehaviour
{
    public delegate void SceneDelegate(ScenePhase current, ScenePhase next);
    public static event SceneDelegate ScenePhaseChanged;

    public delegate void SceneChange(Scene current);
    public static event SceneChange SceneUpdate;

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
    private TipController tipController;
    private DialogueController dialogueController;

    // Start is called before the first frame update
    private void Awake()
    {
        if (!spawned)
        {
            spawned = true;
            DontDestroyOnLoad(gameObject);
            
        }
        else
        {
            DestroyImmediate(gameObject);
        }
        
    }

    private void UpdateScene(Scene next, LoadSceneMode what)
    {
        SceneUpdate?.Invoke(next);

        if (SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 1)
        {
        }
        else
        {
            Debug.Log("Scene Updating");

            player = GameObject.Find("Player").GetComponent<PlayerController>();
            playerCamera = GameObject.Find("Player Camera").GetComponent<CameraController>();
            sceneFadeIn = GameObject.Find("Player Camera/UI").GetComponent<Animator>();
            sceneVeil = GameObject.Find("Player Camera/UI/Scene Fade In").GetComponent<SpriteRenderer>();
            deathFade = GameObject.Find("Player Camera/UI/Death Fade").GetComponent<Animator>();
            typer.display = GameObject.Find("Player Camera/UI/Scene Intro").GetComponent<TMP_Text>();
            bits = GameObject.Find("Player Camera/HUD/Bits").GetComponent<TMP_Text>();
            tipController = GameObject.Find("Player Camera/Tips Overlay").GetComponent<TipController>();
            dialogueController = GameObject.Find("Player Camera/Dialogue Overlay").GetComponent<DialogueController>();

            player.input.Gameplay.Pause.performed += ctx => Pause();
            //player.input.Gameplay.ExitCinematic.performed += ctx => ExitCinematic();
            player.input.Gameplay.ContinueDialogue.performed += ctx => ContinueDialogue();
            player.input.Gameplay.ExitDialogue.performed += ctx => ExitDialogue();

            typer.text = next.name;
            currentScene = next;
            player.Respawn();
            playerCamera.Initialize();
            ChangeScenePhase(ScenePhase.Loading);
        }
        
    }

    private void Start()
    {
        SceneManager.sceneLoaded += UpdateScene;
        PlayerController.deathEvent += OnPlayerDeath;
        ExitInteraction.doorEvent += TransitionToNextScene;
        Interaction.eventTrigger += NewEvent;
        

        currentScene = SceneManager.GetActiveScene();
        UpdateScene(currentScene, new LoadSceneMode());
    }

    private void Update()
    {
        
        if (phase == ScenePhase.NewGameMenu)
        {
            
        }
        if (phase == ScenePhase.Loading)
        {
            //load the level
            //reset some transition variables
            sceneVeil.color = new Color(sceneVeil.color.r, sceneVeil.color.g, sceneVeil.color.b, 255);

            ChangeScenePhase(ScenePhase.Open);
        }
        if (phase == ScenePhase.Open)
        {
            if (currentScene.buildIndex != 0)
            {
                StartCoroutine(FadeIn());
            }
            ChangeScenePhase(ScenePhase.Cinematic);
        }
        if (phase == ScenePhase.Dialogue)
        {

        }
    }

    public void NewEvent(int id)
    {
        switch(id)
        {
            case 1:
                player.cape.SetActive(true);
                break;
        }
    }

    public void ExitCinematic()
    {
        if (phase == ScenePhase.Cinematic && !tipController.inputDisabled)
        {
            tipController.FadeInTip("", false);
            ChangeScenePhase(ScenePhase.Game);
        }
    }

    public void ContinueDialogue()
    {
        if (phase == ScenePhase.Dialogue)
        {
            if (DialogueController.currentDialogue.currentLine == DialogueController.currentDialogue.characterLines.Count - 1)
            {
                ExitDialogue();
            }
            else
            {
                dialogueController.IncrementDialogue();

            }
        }
        
        
    }

    public void ExitDialogue()
    {
        ChangeScenePhase(ScenePhase.Game);
    }

    public void OnPlayerDeath()
    {
        deathFade.SetBool("fadeIn", true);
        ChangeScenePhase(ScenePhase.Cinematic);
        StartCoroutine(Respawn());
    }

    public void Pause()
    {
        if (paused)
        {
            Time.timeScale = 1;
            playerCamera.pauseMenu.SetActive(false);
            paused = false;
        }
        else
        {
            paused = true;
            Time.timeScale = 0;
            playerCamera.pauseMenu.SetActive(true);
        }
    }

    public void ButtonResume()
    {
        Pause();
        ChangeScenePhase(ScenePhase.Game);
    }

    public void LoadMenu()
    {
        ButtonResume();
        SceneManager.LoadScene(0);
    }

    public void LoadMenuFromGame()
    {
        GameData.spawnLocation = player.transform.position;
        GameData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
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
        StartCoroutine(LoadScene(GameData.sceneIndex, 2));
    }

    public IEnumerator LoadScene(int scene, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(scene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1f);
        player.Respawn();
        playerCamera.Respawn();
        deathFade.SetBool("fadeIn", false);
        StartCoroutine(ChangeScenePhaseOnDelay(ScenePhase.Game, 0.3f));
    }

    IEnumerator ChangeScenePhaseOnDelay(ScenePhase scenePhase, float delay)
    {
        yield return new WaitForSeconds(delay);
        ChangeScenePhase(scenePhase);
    }



    public static void ChangeScenePhase(ScenePhase scenePhase)
    {
        ScenePhaseChanged?.Invoke(phase, scenePhase);
        phase = scenePhase;
    }

    public void EnterCinematic()
    {
        //ChangeScenePhase(ScenePhase.Cinematic);
    }

    private void TransitionToNextScene(Vector2 destination, string scene)
    {
        ChangeScenePhase(ScenePhase.Cinematic);
        deathFade.SetBool("fadeIn", true);
        StartCoroutine(Transition(destination, scene));
        StartCoroutine(MoveCamera());

        
    }

    IEnumerator Transition(Vector2 destination, string scene)
    {
        yield return new WaitForSeconds(1);

        //Saves Scene Data to GameData
        GameData.queue.Clear();
        player.GetComponent<PlayerAbility>().queue.Save();

        GameData.spawnLocation = destination;
        //Change Scene
        //SceneManager.LoadScene(scene);
        player.Respawn();
        
        deathFade.SetBool("fadeIn", false);
        ChangeScenePhase(ScenePhase.Game);
        
    }

    IEnumerator MoveCamera()
    {
        yield return new WaitForSeconds(1.1f);
        playerCamera.JumpToTarget();
    }

    IEnumerator FadeIn()
    {
        typer.Type();
        sceneFadeIn.SetBool("Open", true);
        yield return new WaitForSeconds(2);

        //ChangeScenePhase(ScenePhase.Game);
    }
}
