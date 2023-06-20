using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinColorChanger : MonoBehaviour
{
    [SerializeField] InteractionHandler interactionHandler;
    private bool isColorChanging;
    private Coroutine colorChangeCoroutine;

    private MeshRenderer coinMeshRenderer;
    [SerializeField] Color[] myColors;
    [SerializeField] Material godModMaterial;

    [SerializeField] ParticleSystem particleSystem;


    [SerializeField] PanelsOpener panelsOpener;

    
    [SerializeField] CoinStateManager coinStateManager;


    

    void Awake()
    {
        coinMeshRenderer = GetComponent<MeshRenderer>();
    }

    public void InitiateColorChange()
    {
        if (panelsOpener.CurrentlyOpened != null) return;

        if (isColorChanging) { StopCoroutine(colorChangeCoroutine); }

        if ((int)coinStateManager.CurrentType == 4)
        {
            coinMeshRenderer.material = godModMaterial;
        }
        else
        {
            colorChangeCoroutine = StartCoroutine(ChangeCoinColor(0.8f));
        }

        

        //use in another class
        particleSystem.Play();
        
    }

    private IEnumerator ChangeCoinColor(float duration)
    {
        isColorChanging = true;
        float elapsed = 0;
        Color startColor = coinMeshRenderer.material.color;

        

        Color targetColor = myColors[(int)coinStateManager.CurrentType];

        

        Debug.Log(elapsed + " started ");
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            coinMeshRenderer.material.color = Color.Lerp(startColor, targetColor, elapsed/duration);
            yield return null;
        }
        Debug.Log(elapsed + " ready ");
        //yield return null;

        coinMeshRenderer.material.color = targetColor;
        isColorChanging = false;

        
    }

}
