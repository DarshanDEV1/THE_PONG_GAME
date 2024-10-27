using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameDataManager : MonoBehaviour
{
    internal string player_name;
    internal double percent;

    [SerializeField] TMP_Text m_message;
    [SerializeField] Button m_Restart;
    [SerializeField] Button m_Quit;

    //HACK: JSON is preferable to store these kind of data.
    //This is test implementation has to be remmoved and json has to be used.
    private void Start()
    {
        percent = 90.00;
        m_message.text = player_name + " Won With " + percent.ToString("F2") + " % ";

        m_Restart.onClick.RemoveAllListeners();
        m_Restart.onClick.AddListener(() => { SceneManager.LoadScene("IntroScene"); });
        m_Quit.onClick.RemoveAllListeners();
        m_Quit.onClick.AddListener(() => { Application.Quit(); });

        ButtonHoverDetection(m_Restart);
        ButtonHoverDetection(m_Quit);
    }

    private void ButtonHoverDetection(Button button)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = button.gameObject.AddComponent<EventTrigger>();
        }

        // Add PointerEnter event
        EventTrigger.Entry pointerEnter = new EventTrigger.Entry();
        pointerEnter.eventID = EventTriggerType.PointerEnter;
        pointerEnter.callback.AddListener((eventData) => 
        {
            DOTweenUIManager.Instance.ScaleValue(button.transform, Vector3.one * 1.2f, 1f);
        });
        trigger.triggers.Add(pointerEnter);

        // Add PointerExit event
        EventTrigger.Entry pointerExit = new EventTrigger.Entry();
        pointerExit.eventID = EventTriggerType.PointerExit;
        pointerExit.callback.AddListener((eventData) => 
        {  
            DOTweenUIManager.Instance.ScaleValue(button.transform, Vector3.one, 1f);
        });
        trigger.triggers.Add(pointerExit);
    }
}
