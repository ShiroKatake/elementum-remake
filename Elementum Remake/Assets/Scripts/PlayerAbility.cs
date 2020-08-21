using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    public AbilityQueue queue;
    public bool active;
    public GameObject earthCube;
    public bool disabled;

    // Start is called before the first frame update
    void Start()
    {
        PlayerController.playerFalling += Reset;
        VoidBarrier.voidEvent += queue.ClearAll;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Activate(Vector2 dir)
    {
        if (!disabled)
        {
            active = queue.Activate(gameObject, dir);
        }
    }

    private void Reset()
    {
        active = false;
    }
}
