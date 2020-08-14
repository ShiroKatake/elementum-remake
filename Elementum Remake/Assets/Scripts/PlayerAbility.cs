using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    public AbilityQueue queue;
    public bool active;
    public GameObject earthCube;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Use") || Input.GetButtonDown("Use2"))
        {
            active = queue.Activate(gameObject);
        }
    }
}
