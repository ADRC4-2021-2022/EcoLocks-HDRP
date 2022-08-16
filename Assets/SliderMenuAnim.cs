using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class SliderMenuAnim : MonoBehaviour
{
    public GameObject Menu;

    public void ShowAndHideMenu()
    {
        if (Menu != null)
        {
            Animator animator = Menu.GetComponent<Animator>();
            if (animator != null)
            {
                bool isOpen = animator.GetBool("Show");
                animator.SetBool("Show", !isOpen);
            }
        }



    }

}

