using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Button playButton;
    public Button optionsButton;
    public Button exitButton;

    public GameData dataController;

    // Start is called before the first frame update
    void Start()
    {
        dataController = GameObject.Find("DataController").GetComponent<GameData>();

        playButton.onClick.AddListener(dataController.StartGame);
        exitButton.onClick.AddListener(dataController.ExitGame);
    }
}
