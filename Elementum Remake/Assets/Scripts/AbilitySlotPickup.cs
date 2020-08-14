using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AbilitySlotPickup : MonoBehaviour
{
    public static List<string> collectedAbilitySlotPickups = new List<string>();

    public void Awake()
    {
        foreach (string b in collectedAbilitySlotPickups)
        {
            if (b == SceneManager.GetActiveScene().buildIndex + transform.position.x.ToString() + transform.position.y.ToString())
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Colliding");
        if (collision.gameObject.tag == "Player")
        {
            var pos = transform.position;
            collectedAbilitySlotPickups.Add(SceneManager.GetActiveScene().buildIndex + pos.x.ToString() + pos.y.ToString());
            Destroy(this.gameObject);
            collision.gameObject.GetComponent<PlayerAbility>().queue.Add();
        }
    }
}
