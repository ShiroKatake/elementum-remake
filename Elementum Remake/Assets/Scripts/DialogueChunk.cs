using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialogueChunk : ScriptableObject
{
    public bool locked;
    public int loopback;

    [TextArea(3,10)]
    public List<string> lines;
    public DialougeResponse response;
    public int currentLine;

    public string CurrentLine()
    {
        return lines[currentLine];
    }

    public void Reset()
    {
        currentLine = 0;
    }
}
