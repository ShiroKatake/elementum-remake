﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public enum ScenePhase
{
    Loading,
    Open,
    Title,
    Game,
    Cinematic,
    Close,
    Save
}


public class SceneController : MonoBehaviour
{
    private static bool spawned = false;

    public static Scene currentScene;
    public ScenePhase phase;
    
    private PlayerMovement player;
    private CameraController playerCamera;
    public TypeWriterEffect typer;

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
            player = GameObject.Find("Player").GetComponent<PlayerMovement>();
            playerCamera = GameObject.Find("Player Camera").GetComponent<CameraController>();
            sceneFadeIn = GameObject.Find("Player Camera/UI").GetComponent<Animator>();
            deathFade = GameObject.Find("Player Camera/UI/Death Fade").GetComponent<Animator>();
            typer.display = GameObject.Find("Player Camera/UI/Scene Intro").GetComponent<TMP_Text>();
        }
        else
        {
            DestroyImmediate(gameObject);
        }
        
    }

    private void UpdateScene(Scene current, Scene next)
    {
        Debug.Log("scene changed to " + next.name);
        typer.text = next.name;
        currentScene = next;

    }

    private void Start()
    {
        player.cinematicOverride = true;
        player.disabled = true;
        StartCoroutine(FadeIn());
    }

    private void Update()
    {
        if (!player.alive)
        {
            playerCamera.freeze = true;
            deathFade.SetBool("fadeIn", true);
            StartCoroutine(Respawn());
            player.alive = true;
        }
    }

    IEnumerator Respawn()
    {
        player.disabled = true;
        yield return new WaitForSeconds(1f);
        player.Respawn();
        playerCamera.JumpToTarget();
        playerCamera.freeze = false;
        deathFade.SetBool("fadeIn", false);
        StartCoroutine(EnableMovement(0.3f));
    }

    IEnumerator EnableMovement(float delay)
    {
        yield return new WaitForSeconds(delay);
        player.disabled = false;
    }

    IEnumerator FadeIn()
    {
        typer.Type();

        yield return new WaitForSeconds(2);
        player.disabled = false;
        player.cinematicOverride = false;
        sceneFadeIn.SetBool("Open", true);
    }

    private void Load()
    {

    }
}
