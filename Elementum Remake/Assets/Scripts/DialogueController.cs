using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
    public Animator anim;
    public TMP_Text dialogueText;
    public TMP_Text titleText;
    public static DialougeEvent currentDialogue;

    // Start is called before the first frame update
    void Start()
    {
        SceneController.ScenePhaseChanged += ChangeState;
    }

    public void ChangeState(ScenePhase current, ScenePhase next)
    {
        if (next == ScenePhase.Dialogue)
        {
            anim.SetBool("Active", true);
        }
        else
        {
            anim.SetBool("Active", false);
        }
    }

    public void Update()
    {
        if (currentDialogue != null)
        {
            dialogueText.text = currentDialogue.characterLines[currentDialogue.currentLine];
            titleText.text = currentDialogue.characterName;
        }
    }

    public void ResetDialogue()
    {
        currentDialogue.currentLine = 0;
    }

    public void IncrementDialogue()
    {
        currentDialogue.currentLine++;
    }
}
