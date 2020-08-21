using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidBarrier : MonoBehaviour
{
    public delegate void voidDelegate();
    public static event voidDelegate voidEvent;

    public BoxCollider2D hitBox;
    public SpriteRenderer render;

    public void Start()
    {
        hitBox.size = new Vector2(0.1f, render.size.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            voidEvent?.Invoke();
        }
    }
}
