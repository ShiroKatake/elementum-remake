using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tip : MonoBehaviour
{
    public Animator animator;
    public bool active;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetButtonDown("TipTest"))
        {
            FadeInTip("boob");
        }
    }

    public void FadeInTip(string tip)
    {
        if (!active)
        {
            animator.SetBool("fadeIn", true);
            active = true;
            GetComponentInChildren<TMP_Text>().text = tip;
        }
        else
        {
            animator.SetBool("fadeIn", false);
            active = false;
            GetComponentInChildren<TMP_Text>().text = "";

        }
    }
}
