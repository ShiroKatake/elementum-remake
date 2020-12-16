using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseListController : MonoBehaviour
{
    public GameObject buttonTemplate;
    public GameObject buttonList;

    public void GenerateButtons(List<string> responses, List<int> values)
    {
        int i = 0;
        foreach (string s in responses)
        {
            GameObject button = Instantiate(buttonTemplate);
            button.SetActive(true);
            button.GetComponent<ResponseListButton>().SetText(s);
            button.GetComponent<ResponseListButton>().buttonValue = values[i];
            button.transform.SetParent(buttonList.transform, false);
            i++;
        }
    }
}
