using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySlot : MonoBehaviour
{
    public bool occupied;
    public Element element;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!occupied)
        {
            transform.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
        }
        else
        {
            switch (element.name)
            {
                case "Air":
                    transform.GetComponent<SpriteRenderer>().color = new Color(165, 176, 206, 1);
                    break;
                case "Fire":
                    transform.GetComponent<SpriteRenderer>().color = new Color(148, 36, 0, 1);
                    break;
                case "Earth":
                    transform.GetComponent<SpriteRenderer>().color = new Color(74, 43, 27, 1);
                    break;
            }
        }
        
    }
}
