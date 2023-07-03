using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalBehavior : MonoBehaviour
{
    [SerializeField] CrystalHealthController crystalHealthController;
    public CoinStateManager coinStateManager;
    [SerializeField] Material waterMaterial;
    [SerializeField] Material fireMaterial;
    [SerializeField] Material natureMaterial;
    [SerializeField] Animator ineffectiveNotification;
    [SerializeField] Animator damageNotification;
    private SoundManager soundManager;
    public CoinXpController coinXpController;
    public AreaSpawner areaSpawner;

    AudioSource crystalHit;
    AudioSource crystalHitFail;

    public SoundManager SoundManager { set { soundManager = value; crystalHit = soundManager.FindSound("CrystalHit"); crystalHitFail = soundManager.FindSound("CrystalFailHit"); } }
    public enum CrystalStateEnum { waterState, fireState, natureState };
    private CrystalStateEnum currentType;

    public CrystalStateEnum CurrentType { get { return currentType; } set { currentType = value; } }


    void Awake()
    {
        //crystalHealthController.HealthReachedZero += DestroyCrystall;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DealDamageCrystal()
    {

        if (coinStateManager.CurrentType == CoinStateManager.CoinStateEnum.waterState
            && currentType == CrystalStateEnum.waterState)
        {
            crystalHealthController.DealDamage();
            crystalHit.Play();
            Debug.Log("water");
            damageNotification.Play("CrystalNotification", -1, 0.0f);

        } else if (coinStateManager.CurrentType == CoinStateManager.CoinStateEnum.fireState
            && currentType == CrystalStateEnum.fireState)
        {
            crystalHealthController.DealDamage();
            crystalHit.Play();
            Debug.Log("fire");
            damageNotification.Play("CrystalNotification", -1, 0.0f);
        } else if (coinStateManager.CurrentType == CoinStateManager.CoinStateEnum.natureState
           && currentType == CrystalStateEnum.natureState)
        {
            crystalHealthController.DealDamage();
            crystalHit.Play();
            Debug.Log("neture");
            damageNotification.Play("CrystalNotification", -1, 0.0f);
        } else if (coinStateManager.CurrentType == CoinStateManager.CoinStateEnum.lightState)
        {
            crystalHealthController.DealDamage();
            crystalHit.Play();
            Debug.Log("light");
            damageNotification.Play("CrystalNotification", -1, 0.0f);
        } else
        {
            ineffectiveNotification.Play("CrystalNotification", -1, 0.0f);
            crystalHitFail.Play();
            Debug.Log("ineffective");
        }
    }

    public void RandomizeValues()
    {
        int randomInt = Random.Range(0, 3);
        Debug.Log("values were randomized");
        if (randomInt == 0)
        {
            currentType = CrystalStateEnum.waterState;
            transform.GetComponent<MeshRenderer>().material = waterMaterial;
        } else if (randomInt == 1)
        {
            currentType = CrystalStateEnum.fireState;
            transform.GetComponent<MeshRenderer>().material = fireMaterial;
        } else if (randomInt == 2)
        {
            currentType = CrystalStateEnum.natureState;
            transform.GetComponent<MeshRenderer>().material = natureMaterial;
        }
    }

    public void DestroyCrystal()
    {
        ResetNotifications();
        coinXpController.GainXp();
        areaSpawner.SpawnRate = (areaSpawner.SpawnRate + 2);
        areaSpawner.DestroyCrystal(this.transform.parent.gameObject);
    }


    void ResetNotifications()
    {
        damageNotification.Play("CrystalNotification", -1, 0.98f);
        ineffectiveNotification.Play("CrystalNotification", -1, 0.98f);
    }

}
