using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Key : DestroyOnLoad
{
    public string doorName;
    public GameObject door;

    private void Awake()
    {
        scene = SceneManager.GetActiveScene().buildIndex;
        spawn = transform.position;
        DestroyCollected();
    }

    // Start is called before the first frame update
    void Update()
    {
        door = GameObject.Find(doorName);
        if (door != null)
        {
            door.GetComponent<ExitInteraction>().locked = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovement>().holding = gameObject;
            gameObject.transform.parent = collision.transform;
        }
    }

    public void Activate()
    {
        door.GetComponent<ExitInteraction>().locked = false;
        Collect();
    }
}
