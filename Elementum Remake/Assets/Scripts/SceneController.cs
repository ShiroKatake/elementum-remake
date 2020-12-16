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


    [Header("DebugTools")]
    public bool disableIntro;


    [Header("Scene Objects")]
    public PlayerController player;
    public CameraController playerCamera;
    public TypeWriterEffect typer;
    public ChaseTrigger introTrigger;

    public bool awaitingResponse;

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
            introTrigger = GameObject.Find("Cinematic Triggers/Intro").GetComponent<ChaseTrigger>();
            typer.display = GameObject.Find("Player Camera/UI/Scene Intro").GetComponent<TMP_Text>();

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
            playerCamera.sceneVeil.color = new Color(playerCamera.sceneVeil.color.r, playerCamera.sceneVeil.color.g, playerCamera.sceneVeil.color.b, 255);

            ChangeScenePhase(ScenePhase.Open);
        }
        if (phase == ScenePhase.Open)
        {
            if (currentScene.buildIndex != 0)
            {
                if(!disableIntro)
                {
                    if (!introTrigger.triggered)
                    {
                        StartCoroutine(FadeIn());
                        introTrigger.triggered = true;
                        introTrigger.ManualPlay();
                    }
                    if (introTrigger.ended)
                    {
                        ExitCinematic();
                    }
                }
                else
                {
                    StartCoroutine(FadeIn());
                    ChangeScenePhase(ScenePhase.Game);
                }
            }
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
        Debug.Log("Exiting Cinematic");
        if ((phase == ScenePhase.Cinematic || phase == ScenePhase.Open) && !playerCamera.tipController.inputDisabled)
        {
            playerCamera.tipController.FadeInTip("", false);
            ChangeScenePhase(ScenePhase.Game);
        }
    }

    public void ContinueDialogue()
    {
        if (phase == ScenePhase.Dialogue)
        {
            if (DialogueController.currentDialogue.CurrentLine() == DialogueController.currentDialogue.characterLines.Count - 1)
            {
                if (!awaitingResponse)
                {
                    Response(DialogueController.currentDialogue.CurrentResponse());
                    awaitingResponse = true;
                }
            }
            else
            {
                playerCamera.dialogueController.IncrementDialogue();

            }
        }
    }

    public void RegisterResponse()
    {
        playerCamera.responseController.gameObject.SetActive(false);
        foreach (Transform child in playerCamera.responseController.buttonList.transform)
        {
            Destroy(child.gameObject);
        }
        awaitingResponse = false;
    }

    public void Response(DialougeResponse response)
    {
        if (response == null)
        {
            ExitDialogue();
            return;
        }
        playerCamera.responseController.GenerateButtons(response.Responses, response.values);
        playerCamera.responseController.gameObject.SetActive(true);
    }

    public void ExitDialogue()
    {
        ChangeScenePhase(ScenePhase.Game);
    }

    public void OnPlayerDeath()
    {
        playerCamera.deathFade.SetBool("fadeIn", true);
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
        playerCamera.deathFade.SetBool("fadeIn", false);
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
        playerCamera.deathFade.SetBool("fadeIn", true);
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
        
        playerCamera.deathFade.SetBool("fadeIn", false);
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
        playerCamera.sceneFadeIn.SetBool("Open", true);
        yield return new WaitForSeconds(2);

        //ChangeScenePhase(ScenePhase.Game);
    }
}
