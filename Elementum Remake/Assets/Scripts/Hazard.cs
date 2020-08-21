using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    public delegate void hazardDelegate(GameObject collision);
    public static event hazardDelegate hazardEvent;

    public AudioClip death;
    public AudioClip earthBreak;
    public float playerCooldown;

    public BoxCollider2D hitBox;
    public SpriteRenderer render;

    public void Start()
    {
        hitBox.size = new Vector2(render.size.x-0.3f, render.size.y/2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hazardEvent?.Invoke(collision.gameObject);
    }

    private void Update()
    {
        playerCooldown -= Time.deltaTime;
    }
}
