using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    private Button play;
    private Button options;
    private Button exit;

    private GameData dataController;

    // Start is called before the first frame update
    void Start()
    {
        play = GameObject.Find("Menu Camera/Pause Menu/PlayButton").GetComponent<Button>();
        options = GameObject.Find("Menu Camera/Pause Menu/Options").GetComponent<Button>();
        exit = GameObject.Find("Menu Camera/Pause Menu/Exit").GetComponent<Button>();
        dataController = GameObject.Find("DataController").GetComponent<GameData>();


        play.onClick.AddListener(dataController.StartGame);
        exit.onClick.AddListener(dataController.ExitGame);
        Debug.Log("listeners added");
    }
}
