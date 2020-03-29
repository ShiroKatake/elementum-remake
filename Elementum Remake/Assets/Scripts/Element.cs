using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Element : MonoBehaviour
{
    protected Color color;

    public Color Color
    {
        get
        {
            return color;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Element created");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void Activate(PlayerMovement player);
}
