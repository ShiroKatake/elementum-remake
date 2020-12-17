using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(DialougeEvent))]
public class DialogueEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DialougeEvent dialogue = (DialougeEvent)target;

        if(GUILayout.Button("Reset"))
        {
            dialogue.Reset();
        }
    }
}
