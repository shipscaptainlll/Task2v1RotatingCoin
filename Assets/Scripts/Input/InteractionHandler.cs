using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    [SerializeField] UserInputListener userInputListener;
    [SerializeField] LayerMask targetLayer;

    [SerializeField] SoundManager soundManager;
    //AudioSource crystalHit;
    bool coinHeld;
    public bool CoinHeld { get { return coinHeld; } }

    public event Action CoinClicked = delegate { };

    // Start is called before the first frame update
    void Awake()
    {
        userInputListener.LMBClicked += InspectObject;
        userInputListener.LMBHeld += InspectObjectHeld;
        userInputListener.LMBUp += CoinUnclicked;
    }

    void Start()
    {
        //crystalHit = soundManager.FindSound("CrystalHit");
    }

    void InspectObject()
    {
        Transform foundObject = FindObjectUnderMouse();
        if (foundObject == null)
        {
            Debug.Log("No object found");
            return;
        }

        if (foundObject.GetComponent<CoinRotation>() != null)
        {
            CoinClicked?.Invoke();
        } else if (foundObject.GetComponent<Crystal>() != null)
        {
            //crystalHit.time = 0.5f;
            //crystalHit.Play();
            foundObject.GetComponent<CrystalBehavior>().DealDamageCrystal();
        }

    }

    void InspectObjectHeld()
    {
        Transform foundObject = FindObjectUnderMouse();
        if (foundObject == null)
        {
            Debug.Log("No object found");
            coinHeld = false;
            return;
        }

        if (foundObject.GetComponent<CoinRotation>() != null)
        {
            coinHeld = true;
        } else
        {
            coinHeld = false;
        }

    }

    void CoinUnclicked()
    {
        coinHeld = false;
    }

    Transform FindObjectUnderMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit[] hit = Physics.RaycastAll(ray);
        foreach (RaycastHit hitElement in hit)
        {
            if (targetLayer == (targetLayer | (1 << hitElement.collider.gameObject.layer)))
            {
                return hitElement.transform;
            }
            
        }
        return null;
    }
}
