using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Use the Selectable class as a base class to access the IsHighlighted method
public class MenuButton : Selectable
{
    //Use this to check what Events are happening
    BaseEventData m_BaseEvent;

    // Public Variables
    public GameObject imageIcon;

    void Update()
    {
        //Check if the GameObject is being highlighted
        if (IsHighlighted(m_BaseEvent) == true)
        {
            if (imageIcon && imageIcon.activeSelf == false)
            {
                imageIcon.SetActive(true);
            }
        }
        else
        {
            if (imageIcon && imageIcon.activeSelf == true)
            {
                imageIcon.SetActive(false);
            }
        }
    }
}