using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Element : MonoBehaviour
{
    protected Color color;      //Color of element that slot will change to when it is occupied
    protected string name;      //Allows elements to be identified by name when loading from GameData into new scene

    public Sprite sprite;

    public string Name
    {
        get
        {
            return name;
        }
    }

    public Color Color
    {
        get
        {
            return color;
        }
    }

    //Performs the ability
    public abstract void Activate(GameObject player);

    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
