using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialougeEvent : ScriptableObject
{
    public string characterName;
    public int currentChunk;

    public List<DialogueChunk> characterLines;

    public void ResetCurrentChunk()
    {
        characterLines[currentChunk].currentLine = 0;
    }

    public void ProgressChunk()
    {
        characterLines[currentChunk].currentLine++;
    }

    public int CurrentLine()
    {
        return characterLines[currentChunk].currentLine;
    }

    public DialougeResponse CurrentResponse()
    {
        return characterLines[currentChunk].response;
    }

    public int Loopback()
    {
        return characterLines[currentChunk].loopback;
    }

    public void ChangeChunk(int index)
    {
        currentChunk = index;
    }
}
