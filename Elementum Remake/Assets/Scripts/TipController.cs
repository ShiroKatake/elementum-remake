using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TipController : MonoBehaviour
{
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
            StartCoroutine(InputDisable());
            animator.SetBool("fadeIn", true);
            PlayerController.player.cinematicOverride = true;
        }
        else
        {
            animator.SetBool("fadeIn", false);
            PlayerController.player.cinematicOverride = false;
        }
    }

    private IEnumerator InputDisable()
    {
        inputDisabled = true;

        yield return new WaitForSeconds(1.5f);
        inputDisabled = false;
    }
}
