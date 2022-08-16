using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

//using System;


public class PatternCreator : MonoBehaviour
{
    private int index0 = 0;
    private int index1 = 0;
    private int index2 = 0;
    private int index3 = 0;
    private int index4 = 0;
    private int index5 = 0;
    private int index6 = 0;
    private int index7 = 0;
    private int index8 = 0;

    Color purple;
    Color pink;
    private GameObject[] _prefabs;
    private bool[] _selected;
    public Button[] buttons;
    public Texture[] aTexture;
    public GameObject toogle;

    [SerializeField]
    private readonly float _voxelSize = 0.3f;
    
    public void Start()
    {
        LoadPrefabs();
        purple.r = 255;
        purple.g = 0;
        purple.b = 221;
        purple.a = 255;

        pink.r = 1;
        pink.g = 1;
        pink.b = 200;
        pink.a = 255;

    }
    private void LoadPrefabs()
    {
        _prefabs = Resources.LoadAll<GameObject>("Prefabs/Parts");
        _selected = new bool[_prefabs.Length];
        
        for (int i = 0; i < _prefabs.Length; i++)
        {
            _selected[i] = false;
        }
    }

    #region CLICK
    public void OnClick0()
    {
        //first time press
        if (index0 == 0)
        {
            buttons[0].GetComponent<Image>().color = Color.red;
            _selected[0] = true;
            index0 = 1;
        }
        //release
        else if (index0 == 1)
        {
            buttons[0].GetComponent<Image>().color = Color.gray;
            _selected[0] = false;
            index0 = 0;
        }
    }

    public void OnClick1()
    {
        //first time press
        if (index1 == 0)
        {
            buttons[1].GetComponent<Image>().color = Color.red;
            _selected[1] = true;
            index1 = 1;
        }
        //release
        else if (index1 == 1)
        {
            buttons[1].GetComponent<Image>().color = Color.gray;
            _selected[1] = false;
            index1 = 0;
        }
    }

    public void OnClick2()
    {
        //first time press
        if (index2 == 0)
        {
            buttons[2].GetComponent<Image>().color = Color.red;
            _selected[2] = true;
            index2 = 1;
        }
        //release
        else if (index2 == 1)
        {
            buttons[2].GetComponent<Image>().color = Color.gray;
            _selected[2] = false;
            index2 = 0;
        }
    }

    public void OnClick3()
    {
        //first time press
        if (index3 == 0)
        {
            buttons[3].GetComponent<Image>().color = Color.red;
            _selected[3] = true;
            index3 = 1;
        }
        //release
        else if (index3 == 1)
        {
            buttons[3].GetComponent<Image>().color = Color.gray;
            _selected[3] = false;
            index3 = 0;
        }
    }

    public void OnClick4()
    {
        //first time press
        if (index4 == 0)
        {
            buttons[4].GetComponent<Image>().color = Color.red;
            _selected[4] = true;
            index4 = 1;
        }
        //release
        else if (index4 == 1)
        {
            buttons[4].GetComponent<Image>().color = Color.gray;
            _selected[4] = false;
            index4 = 0;
        }
    }

    public void OnClick5()
    {
        //first time press
        if (index5 == 0)
        {
            buttons[5].GetComponent<Image>().color = Color.red;
            _selected[5] = true;
            index5 = 1;
        }
        //release
        else if (index5 == 1)
        {
            buttons[5].GetComponent<Image>().color = Color.gray;
            _selected[5] = false;
            index5 = 0;
        }
    }

    public void OnClick6()
    {
        //first time press
        if (index6 == 0)
        {
            buttons[6].GetComponent<Image>().color = Color.red;
            _selected[6] = true;
            index6 = 1;
        }
        //release
        else if (index6 == 1)
        {
            buttons[6].GetComponent<Image>().color = Color.gray;
            _selected[6] = false;
            index6 = 0;
        }
    }

    public void OnClick7()
    {
        //first time press
        if (index7 == 0)
        {
            buttons[7].GetComponent<Image>().color = Color.red;
            _selected[7] = true;
            index7 = 1;
        }
        //release
        else if (index7 == 1)
        {
            buttons[7].GetComponent<Image>().color = Color.gray;
            _selected[7] = false;
            index7 = 0;
        }
    }

    public void OnClick8()
    {
        //first time press
        if (index8 == 0)
        {
            buttons[8].GetComponent<Image>().color = Color.red;
            _selected[8] = true;
            index8 = 1;
        }
        //release
        else if (index8 == 1)
        {
            buttons[8].GetComponent<Image>().color = Color.gray;
            _selected[8] = false;
            index8 = 0;
        }
    }

#endregion

    public void OnGUI()
    {
        int counter = 0;
        int height = 100;
        for (int i = 0; i < _selected.Length; i++)
        {     
            _selected[i] = GUI.Toggle(new Rect(10, 10 + height * counter++, 100, 30), _selected[i], $"{_prefabs[i].name}");
            if (_selected[i])
            {
                if (i == 0)
                {
                    buttons[i].GetComponent<Image>().color = Color.red;
                }
                else if (i == 1)
                {
                    buttons[i].GetComponent<Image>().color = Color.green;
                }
                else if (i == 2)
                {
                    buttons[i].GetComponent<Image>().color = Color.blue;
                }
                else if (i == 3)
                {
                    buttons[i].GetComponent<Image>().color = Color.white;
                }
                else if (i == 4)
                {
                    buttons[i].GetComponent<Image>().color = Color.yellow;
                }
                else if (i == 5)
                {
                    buttons[i].GetComponent<Image>().color = Color.cyan;
                }
                else if (i == 6)
                {
                    buttons[i].GetComponent<Image>().color = purple;
                }
                else if (i == 7)
                {
                    buttons[i].GetComponent<Image>().color = Color.black;
                }
                else if (i == 8)
                {
                    buttons[i].GetComponent<Image>().color = pink;
                }
            }
            else if(_selected[i]==false)
            {
                buttons[i].GetComponent<Image>().color = Color.gray;
            }
        }

    }

    void AddTag(string tag, GameObject obj)
    {
        if (!isHasTag(tag))
        {
            SerializedObject tagManager = new SerializedObject(obj);
            SerializedProperty it = tagManager.GetIterator();
            while (it.NextVisible(true))
            {
                if (it.name == "m_TagString")
                {
                    Debug.Log(it.stringValue);
                    it.stringValue = tag;
                    tagManager.ApplyModifiedProperties();
                }
            }
        }
    }

    bool isHasTag(string tag)
    {
        for (int i = 0; i < UnityEditorInternal.InternalEditorUtility.tags.Length; i++)
        {
            if (UnityEditorInternal.InternalEditorUtility.tags[i].Equals(tag))
                return true;
        }
        return false;
    }

    public void CreatePatterns()
    {
        int count = 0;       
        for (int i = 0; i < _prefabs.Length; i++)
        {
            if (_selected[i])
            {
                var goPrefab = _prefabs[i];
                GameObject goComponent = GameObject.Instantiate(goPrefab);                          
                AddPattern(goComponent, count++);
                GameObject.Destroy(goComponent);
            }
        }
    }

    private void AddPattern(GameObject goComponent, int count)
    {
        //Declare all the patern variables
        string name = goComponent.name;
        List<Vector3Int> indices = new List<Vector3Int>();
        List<Vector3Int> anchorpoints = new List<Vector3Int>();
        List<Vector3Int> connections = new List<Vector3Int>();

        //Create a voxelgrid using the given voxelsize with the dimensions and origin of the component
        GameObject goPartMesh = Util.GetChildrenWithTag(goComponent.transform, "ComponentMesh").First();
        //GameObject goPartMesh = Util.GetChildrenWithTag(goComponent.transform, transform.localPosition, "ComponentMesh").First();
        if (goPartMesh == null)
        {
            Debug.Log($"Mesh gameobject {name} collider not found");
            return;
        }
        MeshCollider partCollider = goPartMesh.GetComponent<MeshCollider>();

        var displacement = new Vector3(0, count * 10, 0);
        goComponent.transform.Translate(displacement);

        ////Get dimensions of your component + connections/ voxelsize
        Bounds partBounds = new Bounds();
        partBounds.Encapsulate(partCollider.bounds);

        GameObject[] goConnections = Util.GetChildrenWithTag(goComponent.transform, "Connection").ToArray();
        foreach (var connection in goConnections)
        {
            partBounds.Encapsulate(connection.GetComponent<Collider>().bounds);
        }

        Vector3Int gridDimensions = (partBounds.size / _voxelSize).ToVector3IntCeil();      
        Vector3[,,] vecGrid = new Vector3[gridDimensions.x, gridDimensions.y, gridDimensions.z];

        for (int x = 0; x < gridDimensions.x; x++)
        {
            for (int y = 0; y < gridDimensions.y; y++)
            {
                for (int z = 0; z < gridDimensions.z; z++)
                {
                    vecGrid[x, y, z] = displacement + partBounds.min + new Vector3(x, y, z) * _voxelSize + Vector3.one * 0.5f * _voxelSize;
                }
            }
        }

        //add all the voxelsindices with the centre inside the component collider to the indices list
        for (int x = 0; x < gridDimensions.x; x++)
        {
            for (int y = 0; y < gridDimensions.y; y++)
            {
                for (int z = 0; z < gridDimensions.z; z++)
                {
                    var pos = vecGrid[x, y, z];
                    var index = new Vector3Int(x, y, z);
                    if (Util.PointInsideCollider(pos, partCollider))
                    {
                        indices.Add(index);
                    }
                }
            }
        }   

        //Get all the children of the component gameobject with the tag anchorpoints in a list
        ////Loop over all the anchorpoints,
        //////Take the anchorpoint position and divide by your voxelsize, rounded to a Vector3Int
        //////Add the new vector3Int to the list of anchorpoint
        GameObject[] goAnchorpoints = Util.GetChildrenWithTag(goComponent.transform, "AnchorPoint").ToArray();

        foreach (GameObject goAnchorpoint in goAnchorpoints)
        {
            for (int x = 0; x < gridDimensions.x; x++)
            {
                for (int y = 0; y < gridDimensions.y; y++)
                {
                    for (int z = 0; z < gridDimensions.z; z++)
                    {
                        var pos = vecGrid[x, y, z];
                        var index = new Vector3Int(x, y, z);
                        if (Vector3.Distance(goAnchorpoint.transform.position, pos) < _voxelSize / 2)
                        {
                            anchorpoints.Add(index);
                        }
                    }
                }
            }           
        }

        //Get all the children of the component gameobject with the tag connections in a list
        ////Loop over all the connections,
        //////Take the connection position and divide by your voxelsize, rounded to a Vector3Int
        //////Add the new vector3Int to the list of connection
        foreach (GameObject goConnection in goConnections)
        {
            for (int x = 0; x < gridDimensions.x; x++)
            {
                for (int y = 0; y < gridDimensions.y; y++)
                {
                    for (int z = 0; z < gridDimensions.z; z++)
                    {
                        var pos = vecGrid[x, y, z];
                        var index = new Vector3Int(x, y, z);
                        if (Vector3.Distance(goConnection.transform.position, pos) < _voxelSize / 2)
                        {
                            connections.Add(index);
                        }
                    }
                }
            }            
        }
        Debug.Log($"{name} {indices.Count} {anchorpoints.Count} {connections.Count}");
        //Add the new paterns for the component
        PatternManager.AddPattern(indices, anchorpoints, connections, $"{name}-{Random.Range(0, 1000)}", goComponent, _voxelSize);
    }  

    public void showThis()
    {
        toogle.SetActive(true);
    }

}
