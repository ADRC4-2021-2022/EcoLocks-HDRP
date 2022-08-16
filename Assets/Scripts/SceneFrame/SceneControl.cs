using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

///<summary>
///
///</summary>
public class SceneControl
{
    private static SceneControl instance;

    public int scene_number = 1;
    public string[] string_scene;

    /// <summary>
    /// key => scene name£¬val => scene info
    /// </summary>
    public Dictionary<string, SceneBase> dict_scene;


    public static SceneControl GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("SceneControl is not exist");
            return instance;
        }

        return instance;
    }

    

    public SceneControl()
    {
        instance = this;

        dict_scene = new Dictionary<string, SceneBase>();
        //dict_scene.Add();
    }

    
    public void LoadScene(string sceneName, SceneBase sceneBase)
    {
        if (scene_number >= 2)
        {
            foreach (string scenename in string_scene)
            {
                if (scenename == sceneName)
                {
                    Debug.Log($"{sceneName} load");
                    break;
                }
                scene_number++;
                string_scene[scene_number] = sceneName;
            }
        }

        if (!dict_scene.ContainsKey(sceneName))
        {
            dict_scene.Add(sceneName, sceneBase);
        }
    
        if (scene_number >= 2)
        {
            dict_scene[SceneManager.GetActiveScene().name].ExitScene();
        }
        SceneManager.LoadScene(sceneName);
        sceneBase.EnterScene();
        GameRoot.GetInstance().UIManager_Root.Pop(true);
        

    }

}
