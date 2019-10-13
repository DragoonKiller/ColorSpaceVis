using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMouseOperation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IScrollHandler
{
    public CameraFocus cam;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left) cam.BeginMouseControl();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left) cam.EndMouseControl();
    }

    public void OnScroll(PointerEventData eventData)
    {
        var dy = eventData.scrollDelta.y;
        cam.dist *= Mathf.Pow(0.95f, dy);
    }
}
