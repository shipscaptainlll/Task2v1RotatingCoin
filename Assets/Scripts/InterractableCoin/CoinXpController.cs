using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinXpController : MonoBehaviour
{
    float minimalWidth = 0;
    [SerializeField] float maximumWidth;
    [SerializeField] float maximumXp;
    [SerializeField] float currentXp;
    [SerializeField] RectTransform xpTransform;
    [SerializeField] Animator xpNotification;
    [SerializeField] Animator coinXpAnimator;
    [SerializeField] CoinStateManager coinStateManager;
    [SerializeField] AreaSpawner areaSpawner;



    public float CurrentXp { get { return currentXp; } set { currentXp = value; } }


    void Awake()
    {

    }

    public void GainXp()
    {
        //Debug.Log(currentDamage);
        int xp = 60;
        xpNotification.Play("XpNotification", -1, 0.0f);
        
        currentXp += xp;
        float xpPercent = ((currentXp) / maximumXp) * 100;
        //Debug.Log("hello there");

        xpPercent = Mathf.Clamp(xpPercent, 0, 100);
        Debug.Log("left percent is " + xpPercent);
        int updatedWidth = (int)(xpPercent * maximumWidth / 100);
        Debug.Log("updated width is " + updatedWidth);
        StartCoroutine(SmoothXpIncrease(updatedWidth));
    }

    IEnumerator SmoothXpIncrease(float updatedWidth)
    {
        float counter = 0;
        float smoothingDuration = 0.15f;
        float initialWidth = xpTransform.rect.width;
        float currentWidth = initialWidth;
        while (counter < smoothingDuration)
        {
            counter += Time.deltaTime;
            currentWidth = Mathf.Lerp(initialWidth, updatedWidth, counter / smoothingDuration);
            //Debug.Log(currentWidth);
            xpTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentWidth);
            yield return null;
        }
        xpTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, updatedWidth);
        if (currentXp >= maximumXp)
        {
            EnterGodMode();
            //crystalBehavior.DestroyCrystal();
            //if (HealthReachedZero != null) { HealthReachedZero(); }
        }
        yield return null;
    }

    public void UpdateOreHealth()
    {
        float xpPercent = ((currentXp) / maximumXp) * 100;
        //Debug.Log(currentHealth);
        //Debug.Log(leftHealthPercent);
        xpPercent = Mathf.Clamp(xpPercent, 0, 100);
        int updatedWidth = (int)(xpPercent * maximumWidth / 100);
        xpTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, updatedWidth);
    }


    public void RefillHP()
    {
        currentXp = maximumXp;

        xpTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maximumWidth);
    }

    void EnterGodMode()
    {
        Debug.Log("reached maximum");
        coinXpAnimator.Play("Dissolve");
        areaSpawner.SpawnRate = 120;
        coinStateManager.EnteredGodMode = true;
    }

}
