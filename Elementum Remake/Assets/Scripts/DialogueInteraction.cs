using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteraction : MonoBehaviour
{
    public DialougeEvent dialogue;

    private void Start()
    {
        dialogue.currentLine = 0;
    }

    

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.GetComponent<PlayerController>().input.Gameplay.Interact.phase == UnityEngine.InputSystem.InputActionPhase.Started)
            {
                SceneController.ChangeScenePhase(ScenePhase.Dialogue);
                DialogueController.currentDialogue = dialogue;
            }
        }
    }
}
