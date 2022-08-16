using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class BasePanel 
{
    public UIType uiType;
    public GameObject activeObj;

    public BasePanel (UIType ui_type)
    {
        uiType = ui_type;
    }

    public virtual void OnStart()
    {
        Debug.Log($"{uiType.Name} Go! ");
        if (activeObj.GetComponent<CanvasGroup>() == null)
        {
            activeObj.AddComponent<CanvasGroup>();
        }
    }

    public virtual void OnEnable()
    {
        UIFunction.GetInstance().GetOrAddComponent<CanvasGroup>(activeObj).interactable = true;
    }

    public virtual void OnDisable()
    {
        UIFunction.GetInstance().GetOrAddComponent<CanvasGroup>(activeObj).interactable = false;
    }

    public virtual void OnDestroy()
    {
        UIFunction.GetInstance().GetOrAddComponent<CanvasGroup>(activeObj).interactable = false;
    }


}

