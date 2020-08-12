using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLock : MonoBehaviour
{
    public bool locked;

    // Start is called before the first frame update
    void Start()
    {
        locked = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (locked)
        {
            GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 1);
        }
        else
        {
            Debug.Log("Unlocked");
            GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
        }
        
    }
}
