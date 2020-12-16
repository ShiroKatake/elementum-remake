using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResponseListButton : MonoBehaviour
{
    public TMP_Text buttonText;
    public int buttonValue;

    public void SetText(string text)
    {
        buttonText.text = text;
    }

    public void OnClick()
    {
        Debug.Log("This is running");
        DialogueController.ChangeChunk(buttonValue);
        GameObject.Find("SceneController").GetComponent<SceneController>().RegisterResponse();
    }
}
