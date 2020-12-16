using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCape : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Interaction.eventTrigger += Activate;
    }

    private void Activate(int id)
    {
        if (id == 1000)
        {
            gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
