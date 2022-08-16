using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class UIType 
{
    private string path;
    private string name;

    public string Path { get => path; }
    public string Name { get => name; }

    /// <summary>
    /// get ui info
    /// </summary>
    /// <param name="ui_path"></param>
    /// <param name="ui_name"></param>
    public UIType(string ui_path, string ui_name)
    {
        path = ui_path;
        name = ui_name;



    }



}

