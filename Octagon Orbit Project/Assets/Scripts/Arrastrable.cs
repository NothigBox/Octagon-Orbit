using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Arrastrable : MonoBehaviour,IPointerUpHandler, IPointerDownHandler, IDragHandler, IBeginDragHandler,IEndDragHandler
{
    Image image;
    public bool Pressed;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;

    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.color = Color.green;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.color = Color.white;
    }
}
