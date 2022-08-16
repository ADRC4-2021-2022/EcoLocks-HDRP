using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

/// <summary>
/// Status of the voxel. Dead are voxels that won't be used,
/// Alive are voxels that are currently activated,
/// Available are voxels that can be activated
/// </summary>
public enum VoxelState { Dead = 0, Alive = 1, Available = 2 }

/// <summary>
/// Data structure for a voxel in a grid
/// </summary>
public class Voxel
{
    #region Protected fields

    //protected GameObject _voxelGO;
    //protected VoxelGrid _voxelGrid;
    //protected float _size;

    #endregion

    #region public fields

    //index can not be set outside of the scope of this class
    public Vector3Int Index { get; private set; }


    public List<Face> Faces = new List<Face>(6);
    public Vector3 Center => (Index + _grid.Origin) * _voxelSixe;
    public bool IsActive;

    //23 Create IsObstacle field
    public bool IsObstacle;

    //43 Create IsTarget field
    public bool IsTarget;

    //75 Create IsPath field
    public bool IsPath;

    public FunctionColour VoxelColour;




    /// <summary>
    /// Change the value of _showVoxel and enable/disable the _goVoxelTrigger 
    /// </summary>
    public bool ShowAliveVoxel
    {
        get
        {
            return _showAliveVoxel;
        }
        set
        {
            _showAliveVoxel = value;
            ChangeVoxelVisability();
        }
    }

    /// <summary>
    /// Change the value of _showVoxel and enable/disable the _goVoxelTrigger 
    /// </summary>
    public bool ShowAvailableVoxel
    {
        get
        {
            return _showAvailableVoxel;
        }
        set
        {
            _showAvailableVoxel = value;
            ChangeVoxelVisability();
        }
    }

    /// <summary>
    /// Get and set the status of the voxel. When setting the status, the linked gameobject will be enable or disabled depending on the state.
    /// </summary>
    public VoxelState Status
    {
        get
        {
            return _voxelStatus;
        }
        set
        {
            _voxelStatus = value;
            ChangeVoxelVisability();
        }
    }

    /// <summary>
    /// Get the centre point of the voxel in worldspace
    /// </summary>
    public Vector3 Centre => _gridOrigin + (Vector3)Index * _voxelSixe + Vector3.one * 0.5f * _voxelSixe;
    #endregion

    #region private fields
    private GameObject _goVoxelTrigger;
    private VoxelGrid _grid;

    private VoxelState _voxelStatus;
    private bool _showAliveVoxel;
    private bool _showAvailableVoxel;

    private float _scalefactor = 0.95f;
    private float _voxelSixe => _grid.VoxelSize;
    private Vector3 _gridOrigin => _grid.Origin;
    private float _state;


    #endregion

    #region constructors
    public Voxel(int x, int y, int z, VoxelGrid grid) : this(new Vector3Int(x, y, z), grid) { }

    public Voxel(Vector3Int index, VoxelGrid grid)
    {
        Index = index;
        _grid = grid;
        _state = 1;
        CreateGameobject();

        Status = VoxelState.Available;
    }

    /// <summary>
    /// Creates a regular voxel on a voxel grid
    /// </summary>
    /// <param name="index">The index of the Pix2PixVoxel</param>
    /// <param name="voxelgrid">The <see cref="VoxelGrid"/> this <see cref="Voxel"/> is attached to</param>
    /// <param name="voxelGameObject">The <see cref="GameObject"/> used on the Pix2PixVoxel</param>
    //public Voxel(Vector3Int index, VoxelGrid voxelGrid, float state, float sizeFactor = 1f)
    //{
    //    Index = index;
    //    _grid= voxelGrid;

    //    //04 Add state to the voxel constructor
    //    _state = state;
    //    Status = VoxelState.Available;
    //    CreateGameobject();
    //    //_voxelGO = GameObject.Instantiate(Resources.Load<GameObject>("Pix2PixPrefabs/Basic Cube"));
    //    //_voxelGO.transform.position = (_voxelGrid.Origin + Index) * _size;
    //    //_voxelGO.transform.localScale *= _voxelGrid.VoxelSize * sizeFactor;
    //    //_voxelGO.name = $"Voxel_{Index.x}_{Index.y}_{Index.z}";
    //    //_voxelGO.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Pix2PixMaterials/Basic");
    //    //_voxelGO.GetComponent<VoxelTrigger>().AttachedVoxel = this;
    //      _goVoxelTrigger = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //      _goVoxelTrigger.name = $"Voxel {Index}";
    //      _goVoxelTrigger.tag = "Voxel";
    //      _goVoxelTrigger.transform.position = Centre;
    //      _goVoxelTrigger.transform.localScale = Vector3.one* _voxelSixe * _scalefactor;
    //      VoxelTrigger trigger = _goVoxelTrigger.AddComponent<VoxelTrigger>();
    //      trigger.AttachedVoxel = this;
    //}




    #endregion

    #region private functions
    private void ChangeVoxelVisability()
    {
        bool visible = false;
        if (Status == VoxelState.Dead) visible = false;
        if (Status == VoxelState.Available && _showAvailableVoxel) visible = true;
        if (Status == VoxelState.Alive && _showAliveVoxel) visible = true;

       // _goVoxelTrigger.SetActive(visible);
    }
    #endregion

    #region public functions

    public void SetState(FunctionColour colour, bool state)
    {
        _goVoxelTrigger.GetComponent<MeshRenderer>().material = Util.MaterialPerFunction[colour];
        if (colour == FunctionColour.Yellow || colour == FunctionColour.White || colour == FunctionColour.Red || colour == FunctionColour.Void)
        {
            Status = VoxelState.Dead;
            SetStateEnable(false);
        }
        else
        {
            if (!state) Status = VoxelState.Dead;
            SetStateEnable(state);
        }

    }

    public void SetStateEnable(bool visibility)
    {
       // _goVoxelTrigger.SetActive(visibility);
        if (!visibility)
        {
           _goVoxelTrigger.tag = "VoidVoxel";
           // DestroyGameObject();
        }    
    }


    //41 Create public method to toggle the visibility of this voxel
    /// <summary>
    /// Toggle the visibility of the voxel
    /// </summary>
    public void ToggleVisibility()
    {
        _goVoxelTrigger.GetComponent<MeshRenderer>().enabled = !_goVoxelTrigger.GetComponent<MeshRenderer>().enabled;
    }



    public IEnumerable<Voxel> GetFaceNeighbours()
    {
        int x = Index.x;
        int y = Index.y;
        int z = Index.z;
        var s = _grid.GridDimensions;

        if (x != 0) yield return _grid.Voxels[x - 1, y, z];
        if (x != s.x - 1) yield return _grid.Voxels[x + 1, y, z];

        if (y != 0) yield return _grid.Voxels[x, y - 1, z];
        if (y != s.y - 1) yield return _grid.Voxels[x, y + 1, z];

        if (z != 0) yield return _grid.Voxels[x, y, z - 1];
        if (z != s.z - 1) yield return _grid.Voxels[x, y, z + 1];
    }

    public void ActivateVoxel(bool state)
    {
        IsActive = state;
        _goVoxelTrigger.GetComponent<MeshRenderer>().enabled = state;
        _goVoxelTrigger.GetComponent<BoxCollider>().enabled = state;
    }

    public void CreateGameobject()
    {
        _goVoxelTrigger = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _goVoxelTrigger.name = $"Voxel {Index}";
        _goVoxelTrigger.tag = "Voxel";
        _goVoxelTrigger.transform.position = Centre;
        _goVoxelTrigger.transform.localScale = Vector3.one * _voxelSixe * _scalefactor;
        VoxelTrigger trigger = _goVoxelTrigger.AddComponent<VoxelTrigger>();
        trigger.AttachedVoxel = this;
    }

    public void DestroyGameObject()
    {       
        GameObject.Destroy(_goVoxelTrigger);   
    }

    public List<Voxel> GetFaceNeighbourList()
    {
        List<Voxel> neighbours = new List<Voxel>();
        foreach (Vector3Int direction in Util.Directions)
        {
            Vector3Int neighbourIndex = Index + direction;
            if (Util.CheckInBounds(_grid.GridDimensions, neighbourIndex))
            {
                neighbours.Add(_grid.GetVoxelByIndex(neighbourIndex));
            }
        }
        return neighbours;
    }

    /// <summary>
    /// Get all the voxels that exist with relative indices to the this voxel. 
    /// </summary>
    /// <param name="relativeIndices">indexes related to the voxels indices</param>
    /// <returns>List of relative indices. If requested indices are out of bounds, the list will be empty</returns>
    public List<Voxel> GetRelatedVoxels(List<Vector3Int> relativeIndices)
    {
        List<Voxel> relatedVoxels = new List<Voxel>();
        foreach (Vector3Int relativeIndex in relativeIndices)
        {
            Vector3Int newIndex = Index + relativeIndex;
            if (Util.CheckInBounds(_grid.GridDimensions, newIndex))
            {
                relatedVoxels.Add(_grid.GetVoxelByIndex(newIndex));
            }
        }
        return relatedVoxels;
    }

    /// <summary>
    /// Toggle the visibility status of the neighbours
    /// </summary>
    public void ToggleNeighbours()
    {
        List<Voxel> neighbours = GetFaceNeighbourList();

        foreach (var neighbour in neighbours)
        {
            neighbour.ShowAliveVoxel = !neighbour.ShowAliveVoxel;
        }
    }

    public void SetColor(Color color)
    {
        _goVoxelTrigger.GetComponent<MeshRenderer>().material.color = color;
    }

    public void SetParent(GameObject goParent)
    {
        _goVoxelTrigger.transform.SetParent(goParent.transform, true);
    }

    #endregion

    #region Equality checks
    /// <summary>
    /// Checks if two Voxels are equal based on their Index
    /// </summary>
    /// <param name="other">The <see cref="Voxel"/> to compare with</param>
    /// <returns>True if the Voxels are equal</returns>
    public bool Equals(Pix2PixVoxel other)
    {
        return (other != null) && (Index == other.Index);
    }

    /// <summary>
    /// Get the HashCode of this <see cref="Voxel"/> based on its Index
    /// </summary>
    /// <returns>The HashCode as an Int</returns>
    public override int GetHashCode()
    {
        return Index.GetHashCode();
    }

    #endregion


    #region Trash

    public void SetStateRed(float newState)
    {
        _state = newState;
        _goVoxelTrigger.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Pix2PixMaterials/RED");
        _goVoxelTrigger.tag = "RedVoxel";
    }

    public void SetStateWhite(float newState)
    {
        _state = newState;
        _goVoxelTrigger.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Pix2PixMaterials/WHITE");
        _goVoxelTrigger.tag = "WhiteVoxel";
    }


    public void SetStateYellow(float newState)
    {
        _state = newState;
        _goVoxelTrigger.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Pix2PixMaterials/YELLOW");
        _goVoxelTrigger.tag = "YellowVoxel";
    }

    public void SetStateBlue(float newState)
    {
        _state = newState;
        _goVoxelTrigger.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Pix2PixMaterials/BLUE");
        _goVoxelTrigger.tag = "BlueVoxel";
    }

    public void SetStateGreen(float newState)
    {
        _state = newState;
        _goVoxelTrigger.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Pix2PixMaterials/GREEN");
        _goVoxelTrigger.tag = "GreenVoxel";
    }

    public void SetStatePurple(float newState)
    {
        _state = newState;
        _goVoxelTrigger.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Pix2PixMaterials/PURPLE");
        _goVoxelTrigger.tag = "PurpleVoxel";
    }

    public void SetStateCyan(float newState)
    {
        _state = newState;
        _goVoxelTrigger.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Pix2PixMaterials/CYAN");
        _goVoxelTrigger.tag = "CyanVoxel";
    }

    public void SetStateBlack(float newState)
    {
        _state = newState;
        _goVoxelTrigger.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Pix2PixMaterials/Black");
        _goVoxelTrigger.tag = "BlackVoxel";
    }

    public void SetStateVoid(float newState)
    {
        _state = newState;
        _goVoxelTrigger.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Pix2PixMaterials/Void");
        _goVoxelTrigger.tag = "VoidVoxel";
    }

    #endregion
}
