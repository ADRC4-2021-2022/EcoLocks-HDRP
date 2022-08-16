using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum BlockState { Valid = 0, Intersecting = 1, OutOfBounds = 1, Placed = 2 }
public class Block
{
    public List<Voxel> Voxels;
    public List<Connection> Connections;

    public int PatternIndex;
    private Pattern _pattern => PatternManager.GetPatternByIndex(PatternIndex);
    private VoxelGrid _grid;
    private GameObject _goBlock;

    public Vector3Int Anchor;
    public Quaternion Rotation;
    private bool _placed = false;

    /// <summary>
    /// Get the current state of the block. Can be Valid, Intersecting, OutOfBound or Placed
    /// </summary>
    public BlockState State
    {
        get
        {
            if (_placed) return BlockState.Placed;
            if (Voxels.Count < _pattern.Indices.Count) return BlockState.OutOfBounds;
            if (Voxels.Count(v => v.Status != VoxelState.Available) > 0)
            {
                return BlockState.Intersecting;
            }
            return BlockState.Valid;
        }
    }

    /// <summary>
    /// Block constructor. Will create block starting at an anchor with a certain rotation of a given type.
    /// </summary>
    /// <param name="type">The block type</param>
    /// <param name="anchor">The index where the block needs to be instantiated</param>
    /// <param name="rotation">The rotation the blocks needs to be instantiated in</param>
    public Block(int patternIndex, Vector3Int anchor, Quaternion rotation, VoxelGrid grid)
    {
        PatternIndex = patternIndex;
        Anchor = anchor;
        Rotation = rotation;
        _grid = grid;

        PositionPattern();
    }

    /// <summary>
    /// Add all the relevant voxels to the block according to it's anchor point, pattern and rotation
    /// </summary>
    public void PositionPattern()
    {
        Voxels = new List<Voxel>();
        Connections = new List<Connection>();

        //next to the indices you will also have to run over the pattern connections
        foreach (var connection in _pattern.Connections)
        {
            if (Util.TryOrientIndex(connection, Anchor, Rotation, _grid.GridDimensions, out var newIndex))
            {
                Connections.Add(new Connection(newIndex, _grid));
            }
        }

        foreach (var index in _pattern.Indices)
        {
            if (Util.TryOrientIndex(index, Anchor, Rotation, _grid.GridDimensions, out var newIndex))
            {
                Voxels.Add(_grid.GetVoxelByIndex(newIndex.x, newIndex.y, newIndex.z));
            }
        }
    }

    /// <summary>
    /// Try to activate all the voxels in the block. This method will always return false if the block is not in a valid state.
    /// </summary>
    /// <returns>Returns true if it managed to activate all the voxels in the grid</returns>
    public bool ActivateVoxels()
    {
        if (State != BlockState.Valid)
        {
            Debug.LogWarning("Block can't be placed");
            return false;
        }

        Color randomCol = Util.RandomColor;
        CreateGOBlock();

        foreach (var voxel in Voxels)
        {
            if (voxel.Status != VoxelState.Available) Debug.Log("Wrong voxel state");
            voxel.Status = VoxelState.Alive;
            voxel.SetColor(randomCol);
            if (_goBlock != null)
                voxel.SetParent(_goBlock);
        }

        _placed = true;
        return true;
    }

    /// <summary>
    /// Create the block GameObject
    /// </summary>
    public void CreateGOBlock()
    {
        if (_pattern.GOPrefab != null)
        {
            //We're trying to get the gameobjects to allign with the voxel representation
            GameObject moveAnchor = new GameObject();
            moveAnchor.name = this._pattern.Name;

            _goBlock = GameObject.Instantiate(_pattern.GOPrefab);
            _goBlock.transform.SetParent(moveAnchor.transform);
            _goBlock.transform.localPosition = -(Vector3)_pattern.AnchorPoint * _grid.VoxelSize - Vector3.one * _grid.VoxelSize * 0.5f;

            if (State == BlockState.Intersecting) _goBlock.name = "INTERSECTING";

            moveAnchor.transform.position = _grid.GetVoxelByIndex(Anchor).Centre;
            moveAnchor.transform.rotation = Rotation;;
            var mesh = Util.GetChildrenWithTag(_goBlock.transform, "ComponentMesh").First();
        }
        else
            Debug.LogWarning($"The GameObject for pattern  {PatternIndex}: '{PatternManager.GetPatternByIndex(PatternIndex).Name}' is not found");
    }

    /// <summary>
    /// Remove the block from the grid
    /// </summary>
    public void DeactivateVoxels()
    {
        foreach (var voxel in Voxels)
            voxel.Status = VoxelState.Available;
    }

    /// <summary>
    /// Completely remove the block
    /// </summary>
    public void DestroyBlock()
    {
        DeactivateVoxels();
        if (_goBlock != null) GameObject.Destroy(_goBlock);
    }
}
