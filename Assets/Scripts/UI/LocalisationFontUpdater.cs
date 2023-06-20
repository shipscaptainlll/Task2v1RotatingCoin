using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LocalisationFontUpdater : MonoBehaviour
{
    [SerializeField] Font defaultFont;
    [SerializeField] Font reserveFont;
    


    public void UpdateText(Text textComponent)
    {
        //Debug.Log("Current localization is " + LocalizationSettings.SelectedLocale.name);
        if (LocalizationSettings.SelectedLocale.name == "Russian (ru)")
        {
            //Debug.Log("current localization is Russian font is " + reserveFont);
            textComponent.font = reserveFont;
            
        } else
        {
            //Debug.Log("current localization is Other font is " + defaultFont);
            textComponent.font = defaultFont;
        }
        
    }
}
