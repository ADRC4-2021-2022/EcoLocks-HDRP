using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class UIManager 
{
    private static UIManager instance;

    public GameObject canvasObj;
    public Stack<BasePanel> stack_ui;
    public Dictionary<string, GameObject> dict_uiObject;
    public static UIManager GetInstance() 
    { 
        if (instance == null)
        {
            Debug.LogError("UIManager instance is not exist");
            return instance;
        }
        else
        {
            return instance;
        }
    }

    public UIManager()
    {
        instance = this;
        stack_ui = new Stack<BasePanel>();
        dict_uiObject = new Dictionary<string, GameObject>();


    }

    public GameObject GetSingleObject(UIType uiType)
    {
        if (dict_uiObject.ContainsKey(uiType.Name))
        {
            return dict_uiObject[uiType.Name];
        }

        if (canvasObj == null)
        {
            canvasObj = UIFunction.GetInstance().FindCanvas();
        }

        GameObject gameObject = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(uiType.Path), canvasObj.transform);
        return gameObject;
    }
    /// <summary>
    /// in stack
    /// </summary>
    /// <param name="basePanel"></param>
    public void Push(BasePanel basePanel)
    {
        Debug.Log($"{basePanel.uiType.Name} in stack");

        if (stack_ui.Count > 0)
        {
            stack_ui.Peek().OnDisable();
        }

        GameObject ui_object = GetSingleObject(basePanel.uiType);
        dict_uiObject.Add(basePanel.uiType.Name, ui_object);
        basePanel.activeObj = ui_object;

        if (stack_ui.Count == 0)
        {
            stack_ui.Push(basePanel);
        }
        else
        {
            if (stack_ui.Peek().uiType.Name == basePanel.uiType.Name)
            {
                stack_ui.Push(basePanel);
            }
        }

        basePanel.OnStart();
    }

    /// <summary>
    /// out stack
    /// </summary>
    /// <param name="isLoad">pop top => false, pop all => true</param>
    public void Pop(bool isLoad)
    {
        if (isLoad == true)
        {
            if (stack_ui.Count>0)
            {
                stack_ui.Peek().OnDisable();
                stack_ui.Peek().OnDestroy();
                GameObject.Destroy(dict_uiObject[stack_ui.Peek().uiType.Name]);
                dict_uiObject.Remove(stack_ui.Peek().uiType.Name);
                stack_ui.Pop();
                Pop(true);
            }
        }

        else
        {
            if (stack_ui.Count > 0)
            {
                stack_ui.Peek().OnDisable();
                stack_ui.Peek().OnDestroy();
                GameObject.Destroy(dict_uiObject[stack_ui.Peek().uiType.Name]);
                dict_uiObject.Remove(stack_ui.Peek().uiType.Name);
                stack_ui.Pop();

                if (stack_ui.Count > 0)
                {
                    stack_ui.Peek().OnEnable();
                }
            }
        }






    }






}

