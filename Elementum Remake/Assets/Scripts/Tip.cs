using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tip : MonoBehaviour
{
    public delegate void CollisionDelegate();
    public static event CollisionDelegate tipActivate;

    private TipController overlay;
    private bool activated;

    public string text;

    private void Awake()
    {
        overlay = GameObject.Find("Player Camera/Tips Overlay").GetComponent<TipController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!activated)
        {
            activated = true;
            overlay.FadeInTip(text, true);
            tipActivate?.Invoke();
        }
    }
}
