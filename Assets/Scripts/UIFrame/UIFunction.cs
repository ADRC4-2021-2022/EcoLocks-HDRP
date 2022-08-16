using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class UIFunction 
{
    private static UIFunction instance;

    public static UIFunction GetInstance()
    {
        if (instance == null)
        {
            instance = new UIFunction();         
        }
        return instance;
    }

    public GameObject FindCanvas()
    {
        GameObject gameObject = GameObject.FindObjectOfType<Canvas>().gameObject;
        if (gameObject == null)
        {
            Debug.LogError("No canvas");
            return gameObject;
        }

        return gameObject;
    }

    public GameObject FindObjectInChild(GameObject parent, string child_name)
    {
        Transform[] transforms = parent.GetComponentsInChildren<Transform>();

        foreach (var item in transforms)
        {
            if (item.gameObject.name == child_name)
            {
                return item.gameObject;
            }
        }
        Debug.LogWarning("It is not here");
        return null;



    }

    public T GetOrAddSingleComponentInChild<T>(GameObject panel, string ComponentName) where T : Component
    {
        Transform[] transforms = panel.GetComponentsInChildren<Transform>();

        foreach (Transform item in transforms)
        {
            if (item.gameObject.name == ComponentName)
            {
                return item.gameObject.GetComponent<T>();                
            }
        }
        return null;
    }


    public T GetOrAddComponent<T>(GameObject Get_Obj) where T : Component
    {
        if (Get_Obj.GetComponent<T>() != null)
        {
            return Get_Obj.GetComponent<T>();
        }

        
        return null;
    }



}

