using UnityEngine;  
using System.Collections;  
using UnityEngine.EventSystems;  
using UnityEngine.UI;
using TMPro;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public Image buttonImage; // Reference to the button's Image component
    public TextMeshProUGUI theText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        theText.color = Color.red; //Or however you do your color
        if (buttonImage != null)
        {
            buttonImage.color = Color.white; // Change the button color on hover
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        theText.color = Color.white; //Or however you do your color
        if (buttonImage != null)
        {
            buttonImage.color = Color.red; // Change the button color on hover
        }
    }
}