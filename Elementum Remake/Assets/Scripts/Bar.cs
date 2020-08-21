using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{
    public GameObject player;
    public Vector2 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y-0.5f > transform.position.y)
        {
            transform.position = new Vector2(transform.position.x, player.transform.position.y-0.5f);
        }
        if (Input.GetKey(KeyCode.F))
        {
            transform.position = initialPosition;
        }
    }
}
