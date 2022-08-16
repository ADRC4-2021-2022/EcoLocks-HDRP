using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

///<summary>
///
///</summary>
public class ButtonListener : MonoBehaviour
{
    public Button[] buttons;
    ColorBlock cbGray = new ColorBlock();
    ColorBlock cb1 = new ColorBlock();
    ColorBlock cb2 = new ColorBlock();
    void Start()
    {
        
        

    }


    int index = 0;
    public void OnPointerDown(PointerEventData eventData)
    {
        print("°´ÏÂ£¡£¡£¡£¡");
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        print("Ì§Æð£¡£¡£¡£¡");
    }

    public void OnClick()
    {

        //first time press
        if (index == 0)
        {
            buttons[0].GetComponent<Image>().color = Color.red;
            
            Debug.Log("0");
            index = 1;
        }

        //release
        else 
        {
            buttons[0].GetComponent<Image>().color = Color.gray;
            Debug.Log("1");
            index = 0;
        }










    }
}


