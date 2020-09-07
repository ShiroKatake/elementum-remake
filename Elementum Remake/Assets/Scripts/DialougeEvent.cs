using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialougeEvent : ScriptableObject
{
    public string characterName;
    public List<string> characterLines;
    public int currentLine;
}
