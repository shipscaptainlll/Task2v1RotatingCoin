using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelsOpener : MonoBehaviour
{
    //[SerializeField] ContactManager contactManager;
    //[SerializeField] ClickManager clickManager;
    [SerializeField] Transform creditsPanel;
    [SerializeField] Transform escapeMenuPanel;
    [SerializeField] Transform bugReportPanel;
    [SerializeField] Transform exitGamePanel;
    //[SerializeField] Transform inactivePosition;
    //[SerializeField] Transform activePosition;
    [SerializeField] Transform finishGamePanel;

    [SerializeField] CreditsScroller creditsScroller;
    [SerializeField] BugReportSender bugReportSender;

    [Header("Sound Manager")]
    [SerializeField] SoundManager soundManager;


    AudioSource whooshFirstSound;
    AudioSource inventoryOpenSound;
    Transform currentlyOpened;
    public Transform CurrentlyOpened { get => currentlyOpened; }

    Transform nextToOpen;
    float updateSpeed;

    public event Action PanelsUpdated = delegate { };
    void Start()
    {
        //RelocateFarAway(creditsPanel);
        //RelocateFarAway(bugReportPanel);
        //RelocateFarAway(escapeMenuPanel);
        updateSpeed = 0.1f;
        

        //whooshFirstSound = soundManager.FindSound("...");
        //inventoryOpenSound = soundManager.FindSound("...");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChooseEscapeActions();
        }
    }

    void DecideNextState()
    {
        if (currentlyOpened != null && currentlyOpened != finishGamePanel)
        {
            ClosePanel(currentlyOpened);
        }
        openPanel(nextToOpen);
        ShowOnForeground(nextToOpen);
        currentlyOpened = nextToOpen;

    }

    public void ChooseEscapeActions()
    {
        if (currentlyOpened != null)
        {
            CloseCurrentPanel();
            nextToOpen = null;
            //cameraVolumeController.UnBlurScreen();
        }
        else
        {
            OpenEscapePanel();
        }
    }

    void ClosePanel(Transform panelToClose)
    {
        if (panelToClose != null)
        {
            StartCoroutine(SmoothCloseTransition(panelToClose, false));
        }
    }

    public void CloseCurrentPanel()
    {
        ClosePanel(currentlyOpened);
        currentlyOpened = null;
    }

    IEnumerator SmoothCloseTransition(Transform panelToClose, bool isSubpanel)
    {

        CanvasGroup panelCanvasGroup = panelToClose.GetComponent<CanvasGroup>();
        float elapsed = 0;
        float alphaMaxValue = 1;
        float alphaZeroValue = 0;
        if (panelCanvasGroup.alpha > 0)
        {
            while (elapsed < updateSpeed)
            {
                elapsed += Time.deltaTime;
                panelCanvasGroup.alpha = Mathf.Lerp(alphaMaxValue, alphaZeroValue, elapsed / updateSpeed);
                yield return null;
            }
        }

        panelToClose.gameObject.SetActive(false);
        if (!isSubpanel) {
            //RelocateFarAway(panelToClose);
            
        }

    }

    void openPanel(Transform panelToOpen)
    {
        //RelocateDefaultPosition(panelToOpen);
        panelToOpen.gameObject.SetActive(true);
        StartCoroutine(SmoothOpenTransition(panelToOpen));
    }

    IEnumerator SmoothOpenTransition(Transform panelToOpen)
    {
        CanvasGroup panelCanvasGroup = panelToOpen.GetComponent<CanvasGroup>();
        //RelocateDefaultPosition(panelToOpen);
        float elapsed = 0;
        float alphaMaxValue = 1;
        float alphaZeroValue = 0;
        if (panelCanvasGroup.alpha < 1)
        {
            while (elapsed < updateSpeed)
            {
                elapsed += Time.deltaTime;
                panelCanvasGroup.alpha = Mathf.Lerp(alphaZeroValue, alphaMaxValue, elapsed / updateSpeed);
                yield return null;
            }
        }
    }

    public void ShowOnForeground(Transform panelToForeground)
    {
        panelToForeground.SetAsLastSibling();
    }


    public void OpenCredits()
    {
        nextToOpen = creditsPanel;
        DecideNextState();
        creditsScroller.StartScrollingView();
    }

    public void OpenBugReportPanel()
    {
        nextToOpen = bugReportPanel;
        DecideNextState();
        bugReportSender.RefreshNotificationsVisibility();
    }

    public void OpenExitPanel()
    {
        nextToOpen = exitGamePanel;
        DecideNextState();
    }

    public void OpenEscapePanel()
    {
        nextToOpen = escapeMenuPanel;
        DecideNextState();
    }

    public void OpenFinishPanel()
    {
        nextToOpen = finishGamePanel;
        DecideNextState();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }



    /*
    

    void RelocateFarAway(Transform panelToMove)
    {
        panelToMove.position = inactivePosition.position;
    }

    void RelocateDefaultPosition(Transform panelToMove)
    {
        panelToMove.position = activePosition.position;
    }

    */

}
