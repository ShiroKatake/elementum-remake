using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class endMenuScript : MonoBehaviour
{

    private Button menu;
    private Button exit;
    private SceneController sceneController;
    // Start is called before the first frame update
    void Start()
    {
        menu = GameObject.Find("Menu Camera/Pause Menu/Menu").GetComponent<Button>();
        exit = GameObject.Find("Menu Camera/Pause Menu/Exit").GetComponent<Button>();
        sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
    }

    // Update is called once per frame
    void Update()
    {
        menu.onClick.AddListener(sceneController.LoadMenu);
        exit.onClick.AddListener(sceneController.ExitGame);
    }
}
