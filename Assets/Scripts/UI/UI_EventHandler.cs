using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    PointerEventData _pointEventData;
    bool _isPressed = false;
    public Action<PointerEventData> OnDownAction;
    public Action<PointerEventData> OnUpAction;
    public Action<PointerEventData> OnPressedAction;
    public void OnPointerDown(PointerEventData eventData)
    {
        _pointEventData = eventData;
        if (OnDownAction != null)
        {
            OnDownAction.Invoke(eventData);
            
        }
        _isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (OnUpAction != null)
        {
            OnUpAction.Invoke(eventData);
           
        }
        _isPressed = false;
    }
    public void OnPressed()
    {
        if (OnPressedAction != null)
        {
            OnPressedAction.Invoke(_pointEventData);
        }
    }



    // Update is called once per frame
    void Update()
    {
        if(_isPressed)
        {
            OnPressed();
        }
    }

    
}
