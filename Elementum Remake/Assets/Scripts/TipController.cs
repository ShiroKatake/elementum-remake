using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TipController : MonoBehaviour
{
    public delegate void TipDelegate(ScenePhase targetPhase);
    public static event TipDelegate tipDisplaying;

    public Animator animator;
    public bool inputDisabled;

    public void FadeInTip(string tip, bool active)
    {
        GetComponentInChildren<TMP_Text>().text = tip;
        if (active)
        {
            tipDisplaying?.Invoke(ScenePhase.Cinematic);
            StartCoroutine(InputDisable());
            animator.SetBool("fadeIn", true);
        }
        else
        {
            tipDisplaying?.Invoke(ScenePhase.Game);
            animator.SetBool("fadeIn", false);
        }
    }

    private IEnumerator InputDisable()
    {
        inputDisabled = true;

        yield return new WaitForSeconds(1.5f);
        inputDisabled = false;
    }
}
