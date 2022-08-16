using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pix2PixVoxel : IEquatable<Pix2PixVoxel>
{
    #region Public fields

    public Vector3Int Index;
    public List<Face> Faces = new List<Face>(6);
    public Vector3 Center => (Index + _voxelGrid.Origin) * _size;
    public bool IsActive;

    //23 Create IsObstacle field
    public bool IsObstacle;

    //43 Create IsTarget field
    public bool IsTarget;

    //75 Create IsPath field
    public bool IsPath;

    public FunctionColour VoxelColour;

    #endregion
    //02 Create the private fields region
    #region Private fields

    //03 Create the state property
    private float _state;

    #endregion

    #region Protected fields

    protected GameObject _voxelGO;
    protected Pix2PixVoxelGrid _voxelGrid;
    protected float _size;

    #endregion

    #region Contructors

    /// <summary>
    /// Creates a regular voxel on a voxel grid
    /// </summary>
    /// <param name="index">The index of the Pix2PixVoxel</param>
    /// <param name="voxelgrid">The <see cref="VoxelGrid"/> this <see cref="Voxel"/> is attached to</param>
    /// <param name="voxelGameObject">The <see cref="GameObject"/> used on the Pix2PixVoxel</param>
    public Pix2PixVoxel(Vector3Int index, Pix2PixVoxelGrid voxelGrid, float state, float sizeFactor = 1f)
    {
        Index = index;
        _voxelGrid = voxelGrid;
        _size = _voxelGrid.VoxelSize;

        //04 Add state to the voxel constructor
        _state = state;
    }

    #endregion

    #region Public methods

    //17 Create SetState method for the Pix2PixVoxel
    /// <summary>
    /// Changes the state of the <see cref="Voxel"/>, updating the material
    /// </summary>
    /// <param name="newState">The new state to be set</param>
    public void SetStateRed(float newState)
    {
        // 18 Set the state from the input value
        _state = newState;
        _voxelGO.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Pix2PixMaterials/RED");
        _voxelGO.tag = "RedVoxel";
    }

    public void SetStateWhite(float newState)
    {
        _state = newState;
        _voxelGO.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Pix2PixMaterials/WHITE");
        _voxelGO.tag = "WhiteVoxel";
    }


    public void SetStateYellow(float newState)
    {
        _state = newState;
        _voxelGO.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Pix2PixMaterials/YELLOW");
        _voxelGO.tag = "YellowVoxel";
    }

    public void SetStateBlue(float newState)
    {
        _state = newState;
        _voxelGO.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Pix2PixMaterials/BLUE");
    }

    public void SetStateGreen(float newState)
    {
        _state = newState;
        _voxelGO.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Pix2PixMaterials/GREEN");
    }


    public void SetStateVoid(float newState)
    {
        _state = newState;
        _voxelGO.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Pix2PixMaterials/Void");
        _voxelGO.tag = "VoidVoxel";
    }




    //41 Create public method to toggle the visibility of this voxel
    /// <summary>
    /// Toggle the visibility of the voxel
    /// </summary>
    public void ToggleVisibility()
    {
        _voxelGO.GetComponent<MeshRenderer>().enabled = !_voxelGO.GetComponent<MeshRenderer>().enabled;
    }

    //51 Create method to set voxel as target
    /// <summary>
    /// Switch the state of the voxel between target states
    /// </summary>
    public void SetAsTarget()
    {
        //52 Flip target bool
        IsTarget = !IsTarget;

        //53 If voxel is target, set material and tag
        if (IsTarget)
        {
            _voxelGO.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Pix2PixMaterials/Target");
            _voxelGO.tag = "TargetVoxel";
        }
        //54 Else, set material and tag to void
        else
        {
            _voxelGO.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Pix2PixMaterials/Void");
            _voxelGO.tag = "VoidVoxel";
        }
    }

    //76 Create method to set voxel as path
    /// <summary>
    /// Set the voxel as a Path voxel
    /// </summary>
    public void SetAsPath()
    {
        //77 Check if it is not a target
        if (!IsTarget)
        {
            //78 Set material and tag
            _voxelGO.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Pix2PixMaterials/Path");
            _voxelGO.tag = "PathVoxel";
            IsPath = true;
        }
    }

    /// <summary>
    /// Get the neighbouring voxels at each face, if it exists
    /// </summary>
    /// <returns>All neighbour voxels</returns>
    public IEnumerable<Pix2PixVoxel> GetFaceNeighbours()
    {
        int x = Index.x;
        int y = Index.y;
        int z = Index.z;
        var s = _voxelGrid.GridDimensions;

        if (x != 0) yield return _voxelGrid.Voxels[x - 1, y, z];
        if (x != s.x - 1) yield return _voxelGrid.Voxels[x + 1, y, z];

        if (y != 0) yield return _voxelGrid.Voxels[x, y - 1, z];
        if (y != s.y - 1) yield return _voxelGrid.Voxels[x, y + 1, z];

        if (z != 0) yield return _voxelGrid.Voxels[x, y, z - 1];
        if (z != s.z - 1) yield return _voxelGrid.Voxels[x, y, z + 1];
    }

    /// <summary>
    /// Activates the visibility of this voxel
    /// </summary>
    public void ActivateVoxel(bool state)
    {
        IsActive = state;
        _voxelGO.GetComponent<MeshRenderer>().enabled = state;
        _voxelGO.GetComponent<BoxCollider>().enabled = state;
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
}