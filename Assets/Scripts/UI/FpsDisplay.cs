using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FpsDisplay : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private bool enabled;
    [SerializeField] private CanvasGroup canvasGroup;
    private int count;

    public bool EnabledFPS
    {

        get => enabled;

        set
        {
            enabled = value;
            ActivateVisibility();
        }
    }

    void Start()
    {
        ActivateVisibility();
    }

    // Update is called once per frame
    void Update()
    {
        if (enabled)
        {
            count = (int)(1f / Time.unscaledDeltaTime);
            text.text = count.ToString();
        }
    }

    void ActivateVisibility()
    {
        if (enabled == true)
        {
            canvasGroup.alpha = 1;
        }
        else
        {
            canvasGroup.alpha = 0;
        }
    }

    public void ToggleVisibility()
    {
        if (enabled)
        {
            enabled = false;
            ActivateVisibility();
        } else
        {
            enabled = true;
            ActivateVisibility();
        }
    }
}
