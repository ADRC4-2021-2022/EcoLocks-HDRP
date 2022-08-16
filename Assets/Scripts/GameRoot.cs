using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class GameRoot : MonoBehaviour
{
    private static GameRoot instance;

    private SceneControl sceneControl;
    public SceneControl SceneControl_Root { get => sceneControl; }

    private UIManager uiManager;
    public UIManager UIManager_Root { get => uiManager; }

    public static GameRoot GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("Fail");
            return null;
        }
        return instance;



    }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        uiManager = new UIManager();
        sceneControl = new SceneControl();
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        UIManager_Root.canvasObj = UIFunction.GetInstance().FindCanvas();


        Scene1 scene1 = new Scene1();
        SceneControl_Root.dict_scene.Add(scene1.sceneName,scene1);


        UIManager_Root.Push(new StartPanel());



    }






}

