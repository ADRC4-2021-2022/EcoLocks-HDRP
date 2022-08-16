using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection
{
    private VoxelGrid _grid;
    public Vector3Int Index;
    public List<Pattern> PossiblePatterns;

    public bool Available
    {
        get
        {
            if (_grid.GetVoxelByIndex(Index).Status != VoxelState.Available)
                return false;
            if (PossiblePatterns.Count <= 0) return false;
            return true;
        }
    }
    
    public Connection(Vector3Int index, VoxelGrid grid)
    {
        Index = index;
        PossiblePatterns = new List<Pattern>(PatternManager.Patterns);
        _grid = grid;
    }
}
