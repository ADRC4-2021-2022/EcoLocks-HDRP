using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;


/// <summary>
/// Singleton class to manage block patterns in the project
/// </summary>
public static class PatternManager
{
    //The pattern manager is a singleton class. This means there is only one instance of the PatternManager class in the entire project and it can be refered to anywhere withing the project

    /// <summary>
    /// Singleton object of the PatternManager class. Refer to this to access the data inside the object.
    /// </summary>
    //public static PatternManager Instance { get; } = new PatternManager();

    private static List<Pattern> _patterns = new List<Pattern>();
    public static Dictionary<string, Pattern> _patternsByName = new Dictionary<string, Pattern>();

    /// <summary>
    /// returns a read only list of the patterns defined in the project
    /// </summary>
    public static ReadOnlyCollection<Pattern> Patterns => new ReadOnlyCollection<Pattern>(_patterns);


    /// <summary>
    /// returns a read only dictionary of the patterns defined in the project organised by name
    /// </summary>
    public static ReadOnlyDictionary<string, Pattern> PatternsByName => new ReadOnlyDictionary<string, Pattern>(_patternsByName);

    /// <summary>
    /// private constructor. All initial patterns will be defined in here
    /// </summary>
    //private PatternManager()
    //{
    //    _patterns = new List<Pattern>();
    //    _patternsByName = new Dictionary<string, Pattern>();



    //}
    /// <summary>
    /// Use this method rather than adding directly to the _patterns field. This method will check if the pattern is valid and can be added to the list. Invalid input will be refused.
    /// </summary>
    /// <param name="indices">List of indices that define the patter. The indices should always relate to Vector3In(0,0,0) as anchor point</param>
    /// <param name="type">The PatternType of this pattern to add. Each type can only exist once</param>
    /// <returns></returns>
    public static bool  AddPattern(List<Vector3Int> indices, List<Vector3Int> anchorPoints, List<Vector3Int> connections, string name, GameObject goPrefab, float voxelSize)
    {

        //only add valid patterns
        if (indices == null) return false;
        //if (indices[0] != Vector3Int.zero) return false;
        if (_patterns.Count(p => p.Name == name) > 0) return false;

        foreach (var anchor in anchorPoints)
        {
            GeneratePatterns(indices, anchor, connections, name, goPrefab,voxelSize );
        }

        return true;
    }

    //When the patternloader is working, add the gameobject to the parameters of this function, position it correctly according to the anchorpoints
    public static void GeneratePatterns(List<Vector3Int> indices, Vector3Int anchorPoint, List<Vector3Int> connections, string name, GameObject goPrefab, float voxelSize)
    {
        List<Vector3Int> newIndices = new List<Vector3Int>();
        List<Vector3Int> newConnections = new List<Vector3Int>();
        string newName = name + anchorPoint.ToString();
        foreach (var index in indices)
        {
            newIndices.Add(index - anchorPoint);
        }
        foreach (var connection in connections)
        {
            newConnections.Add(connection - anchorPoint);
        }

        //GameObject goPrefab = null;
        //Load the prefab out of resources
        //goPrefab = Resources.Load(@"Prefabs/Parts");

        //Create a copy
        //Move the mesh child object of the prefab according to the anchorpoint
        //==> position = position - anchorpoint * voxelsize
        //goPrefab.transform.Translate((Vector3)anchorPoint * voxelSize);


        _patterns.Add(new Pattern(newIndices, newConnections, _patterns.Count, newName, goPrefab, anchorPoint));
        _patternsByName.Add(newName, _patterns.Last());
    }

    /// <summary>
    /// Return the pattern linked to its index
    /// </summary>
    /// <param name="index">The index to look for</param>
    /// <returns>The pattern linked to the type. Will return null if the type is never defined</returns>
    public static Pattern GetPatternByIndex(int index) => Patterns[(int)index];

    public static Pattern GetPatternByName(string name) => PatternsByName[name];
}
/// <summary>
/// The pattern that defines a block. Object of this class should only be made in the PatternManager
/// </summary>
public class Pattern
{
    public GameObject GOPrefab { get; }
    /// <summary>
    /// The patterns are saved as ReadOnlyCollections rather than list so that once defined, the pattern can never be changed
    /// </summary>
    public ReadOnlyCollection<Vector3Int> Indices { get; }

    //List of indices of connection voxels where new block can be added to
    public ReadOnlyCollection<Vector3Int> Connections { get; }
    public int Index { get; }

    public string Name { get; }

    public Vector3Int AnchorPoint { get; }

    /// <summary>
    /// Pattern constructor. The indices will be stored in a ReadOnlyCollection
    /// </summary>
    ///<param name = "indices" > List of indices that define the patter.The indices should always relate to Vector3In(0,0,0) as anchor point</param>
    /// <param name="type">The PatternType of this pattern to add. Each type can only exist once</param>
    public Pattern(List<Vector3Int> indices, List<Vector3Int> connections, int index, string name, GameObject goPrefab, Vector3Int anchorPoint)
    {
        Indices = new ReadOnlyCollection<Vector3Int>(indices);
        Connections = new ReadOnlyCollection<Vector3Int>(connections);
        Index = index;
        Name = name;
        GOPrefab = goPrefab;
        AnchorPoint = anchorPoint;

    }
}

