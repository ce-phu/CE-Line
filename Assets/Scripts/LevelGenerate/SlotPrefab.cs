using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SlotPrefab : MonoBehaviour, IPointerEnterHandler
{
    private int mouseValue = 0;

    private bool isWriting = false;
    
    public UnityEvent<TMP_InputField, int> action;
    
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            isWriting = true;
            mouseValue = 1;
        }
        else if (Input.GetMouseButton(1))
        {
            isWriting = true;
            mouseValue = 2;
        }
        else if (Input.GetMouseButton(2))
        {
            isWriting = true;
            mouseValue = 0;
        }
        else
        {
            isWriting = false;
            mouseValue = -1;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isWriting) return;
        action.Invoke(GetComponent<TMP_InputField>(), mouseValue);
    }
}
