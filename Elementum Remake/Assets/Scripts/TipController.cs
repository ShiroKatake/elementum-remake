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

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!inputDisabled)
        {
            if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Use") || Input.GetButtonDown("Use2"))
            {
                FadeInTip("", false);
            }
        }
    }

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
