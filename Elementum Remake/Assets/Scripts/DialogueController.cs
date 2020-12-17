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
    public static bool textDisabled;

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
            dialogueText.text = currentDialogue.characterLines[currentDialogue.currentChunk].CurrentLine();
            titleText.text = currentDialogue.characterName;
        }
        if (textDisabled)
        {
            dialogueText.alpha = 0;
        }
        else
        {
            dialogueText.alpha = 255;
        }
    }

    public static void ResetDialogue()
    {
        currentDialogue.ResetCurrentChunk();
    }

    public void IncrementDialogue()
    {
        currentDialogue.ProgressChunk();
    }

    public static void ChangeChunk(int chunkIndex)
    {
        currentDialogue.currentChunk = chunkIndex;
        ResetDialogue();
    }
}
