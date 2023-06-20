using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScroller : MonoBehaviour
{
    [SerializeField] Animator creditsAnimator;


    public void StartScrollingView()
    {
        creditsAnimator.Play("CreditsScrolling", -1, 0f);
    }
}
