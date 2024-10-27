using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    [SerializeField] private Button play_Button;
    [SerializeField] private Button help_Button;
    [SerializeField] private Button exit_Button;
    [SerializeField] private Button back_button;
    [SerializeField] private GameObject Info_Panel;

    private void Start()
    {
        play_Button.onClick.RemoveAllListeners();
        play_Button.onClick.AddListener(() => { SceneManager.LoadScene("MainScene"); });

        help_Button.onClick.RemoveAllListeners();
        help_Button.onClick.AddListener(() => { if (!Info_Panel.activeSelf) { Info_Panel.SetActive(true); } });

        back_button.onClick.RemoveAllListeners();
        back_button.onClick.AddListener(() => { if (Info_Panel.activeSelf) { Info_Panel.SetActive(false); } });

        exit_Button.onClick.RemoveAllListeners();
        exit_Button.onClick.AddListener(() => { Application.Quit(); });

        ButtonHoverDetection(play_Button);
        ButtonHoverDetection(exit_Button);
        ButtonHoverDetection(help_Button);
        ButtonHoverDetection(back_button);
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
