using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeWriterEffect : MonoBehaviour
{
    public float delay;
    public string text;
    private string currentText;

    public TMP_Text display;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator ShowText()
    {
        for (int i = 0; i < text.Length +1; i++)
        {
            currentText = text.Substring(0, i);
            display.text = currentText;
            yield return new WaitForSeconds(delay);
        }
    }

    public void Type()
    {
        StartCoroutine(ShowText());
    }
}
