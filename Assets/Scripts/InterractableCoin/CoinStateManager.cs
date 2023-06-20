using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinStateManager : MonoBehaviour
{
    [SerializeField] InteractionHandler interactionHandler;
    [SerializeField] CoinRotation coinRotation;
    [SerializeField] CoinColorChanger coinColorChanger;

    [SerializeField] CanvasGroup coinStateDefault;
    [SerializeField] CanvasGroup coinStateWater;
    [SerializeField] CanvasGroup coinStateFire;
    [SerializeField] CanvasGroup coinStateNature;
    [SerializeField] CanvasGroup coinStateLight;

    [SerializeField] AreaSpawner areaSpawner;
    [SerializeField] PanelsOpener panelsOpener;

    bool enteredGodMode;
    bool disabled;
    int crystalsDestroyedGodMode;

    [SerializeField] ParticleSystem natureParticles;
    [SerializeField] ParticleSystem fireParticles;
    [SerializeField] ParticleSystem waterParticles;
    [SerializeField] ParticleSystem lightParticles;

    [SerializeField] ParticleSystem godModEffects;

    [SerializeField] SoundManager soundManager;

    AudioSource waterSplashSound;
    AudioSource waterSmallDropsSound;
    AudioSource fireSound;
    AudioSource treeBushesSound;
    AudioSource wooshSound;
    AudioSource gustSound;
    AudioSource crystalHit;

    Coroutine ConstantDamagingCoroutine;


    public enum CoinStateEnum { waterState, fireState, natureState, defaultState, lightState };
    private CoinStateEnum currentType;
    System.Random random;

    public bool EnteredGodMode { get { return enteredGodMode; } set { enteredGodMode = value; EnterGodMode(); } }
    public CoinStateEnum CurrentType { get { return currentType; } set { currentType = value; } }

    void Awake()
    {
        currentType = CoinStateEnum.defaultState;

        random = new System.Random(transform.GetHashCode() + DateTime.Now.Millisecond);
        interactionHandler.CoinClicked += ChangeCoinState;
        UpdateCoinStateUI();
    }

    void Start()
    {
        waterSplashSound = soundManager.FindSound("WaterSplash");
        waterSmallDropsSound = soundManager.FindSound("WaterSmallDrops");
        fireSound = soundManager.FindSound("Fire");
        treeBushesSound = soundManager.FindSound("TreeBushes");
        wooshSound = soundManager.FindSound("Woosh");
        gustSound = soundManager.FindSound("Gust");
        crystalHit = soundManager.FindSound("CrystalHit");
    }

    void Update()
    {
        if (enteredGodMode)
        {
            if (interactionHandler.CoinHeld && !disabled)
            {
                coinRotation.CoinSpeedAnimation = coinRotation.CoinMaxSpeed;
                if (!godModEffects.isPlaying) { godModEffects.Play(); }
                if (ConstantDamagingCoroutine == null) { ConstantDamagingCoroutine = StartCoroutine(ConstantDamaging()); }
            } else
            {
                coinRotation.CoinSpeedAnimation = coinRotation.CoinNormalSpeed;
                if (godModEffects.isPlaying) { godModEffects.Stop(); }
                if (ConstantDamagingCoroutine != null) { StopCoroutine(ConstantDamagingCoroutine); ConstantDamagingCoroutine = null; }
            }
        }
    }

    void ChangeCoinState()
    {
        if (!enteredGodMode)
        {
            UpdateRandomState();
            UpdateCoinStateUI();
            coinRotation.InitiateCoinAccelaration();
            coinColorChanger.InitiateColorChange();
            PlayStateVFX();
            PlaySounds();
        }
    }

    public void EnterGodMode()
    {
        interactionHandler.CoinClicked -= ChangeCoinState;
        
        currentType = CoinStateEnum.lightState; 
        coinColorChanger.InitiateColorChange(); 
        PlayStateVFX(); 
        UpdateCoinStateUI();
    }



    void UpdateRandomState()
    {
        int randomNumber = random.Next(0, 3);
        
        while (randomNumber == (int)currentType)
        {
            randomNumber = random.Next(0, 3);
        }

        currentType = (CoinStateEnum)randomNumber;
    }


    void UpdateCoinStateUI()
    {
        Debug.Log("updated ui its " + (int)currentType);
        if ((int)currentType == 0)
        {
            HideCoinState();
            coinStateWater.alpha = 1;
        } else if ((int)currentType == 1)
        {
            HideCoinState();
            coinStateFire.alpha = 1;
        } else if ((int)currentType == 2)
        {
            HideCoinState();
            coinStateNature.alpha = 1;
        } else if ((int)currentType == 3)
        {
            HideCoinState();
            coinStateDefault.alpha = 1;
        } else if ((int)currentType == 4)
        {
            HideCoinState();
            coinStateLight.alpha = 1;
        }
        
    }

    void HideCoinState()
    {
        coinStateDefault.alpha = 0;
        coinStateWater.alpha = 0;
        coinStateFire.alpha = 0;
        coinStateNature.alpha = 0;
        coinStateLight.alpha = 0;
    }

    void PlaySounds()
    {
        if ((int)currentType == 0)
        {
            waterSplashSound.time = 0.2f;
            waterSplashSound.Play();
            waterSmallDropsSound.Play();
            
        }
        else if ((int)currentType == 1)
        {
            fireSound.Play();

        }
        else if ((int)currentType == 2)
        {
            treeBushesSound.Play();
            wooshSound.Play();
            gustSound.Play();
        }
    }

    void PlayStateVFX()
    {
        if ((int)currentType == 0)
        {
            waterParticles.Play();
            
        }
        else if ((int)currentType == 1)
        {
            fireParticles.Play();

        }
        else if ((int)currentType == 2)
        {
            natureParticles.Play();
        }
        else if ((int)currentType == 4)
        {
            lightParticles.Play();
        }
    }

    IEnumerator ConstantDamaging()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            foreach (CrystalBehavior element in areaSpawner.activeCrystals)
            {
                element.DealDamageCrystal();
                crystalsDestroyedGodMode++;
                if (crystalsDestroyedGodMode > 150)
                {
                    StopGodMode();
                }
                //crystalHit.Play();
            }
        }
        
        
    }

    public void StopGodMode()
    {
        areaSpawner.StopSpawning();
        panelsOpener.OpenFinishPanel();
        disabled = true;
    }
    
}
