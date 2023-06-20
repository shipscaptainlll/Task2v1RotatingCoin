using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRotation : MonoBehaviour
{
    [SerializeField] float coinNormalSpeed;
    [SerializeField] float coinMaxSpeed;
    [SerializeField] AnimationCurve acceleratedRotationCurve;
    private float coinSpeed;
    private bool isCoinAccelerated;
    private Coroutine coinAccelerationCoroutine;
    private Animator rotationAnimation;

    public float CoinSpeedAnimation { set { rotationAnimation.speed = value; } }

    public float CoinNormalSpeed { get { return coinNormalSpeed; } }
    public float CoinMaxSpeed { get { return coinMaxSpeed; } }
    public float CoinSpeed { set { coinSpeed = value; } }

    [SerializeField] PanelsOpener panelsOpener;
    [SerializeField] SoundManager soundManager;


    void Awake()
    {
        rotationAnimation = transform.GetComponent<Animator>();
        coinSpeed = coinNormalSpeed;
    }

    public void InitiateCoinAccelaration()
    {
        if (panelsOpener.CurrentlyOpened != null) return;

        if (isCoinAccelerated) { StopCoroutine(coinAccelerationCoroutine); }

        coinAccelerationCoroutine = StartCoroutine(AccelarateCoinSpin(0.8f));
    }

    private IEnumerator AccelarateCoinSpin(float duration)
    {
        //soundManager.Stop("CoinRotation");
        soundManager.Play("CoinFlip");
        isCoinAccelerated = true;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            coinSpeed = Mathf.Lerp(coinNormalSpeed, coinMaxSpeed, acceleratedRotationCurve.Evaluate(elapsed/duration));
            rotationAnimation.speed = coinSpeed;
            yield return null;
        }

        yield return null;

        coinSpeed = coinNormalSpeed;
        rotationAnimation.speed = coinSpeed;
        isCoinAccelerated = false;
    }

    //played by coin rotation animation
    private void PlayRotationSound()
    {
        soundManager.Play("CoinRotation");
    }
}
