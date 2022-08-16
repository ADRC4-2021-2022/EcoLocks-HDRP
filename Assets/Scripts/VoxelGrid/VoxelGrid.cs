using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//Try to use Kevin his voxelgrid class instead of Davids

public enum FunctionColour { Yellow = 1, Green = 2, Blue = 3, Red = 4, White = 5, Purple = 6, Cyan = 7, Black = 8, Void = 9 }
public class VoxelGrid
{
    #region  HSVIndex
    private const int HSV_H = 180;
    private const int HSV_S = 255;
    private const int HSV_V = 255;

    //HSV YELLOW
    private const float HSV_Yellow_H_Max = 34f;
    private const float HSV_Yellow_H_Min = 26f;
    private const float HSV_Yellow_S_Max = 255f;
    private const float HSV_Yellow_S_Min = 43f;
    private const float HSV_Yellow_V_Max = 255f;
    private const float HSV_Yellow_V_Min = 43f;
    //HSV GREEN
    private const float HSV_Green_H_Max = 77f;
    private const float HSV_Green_H_Min = 35f;
    private const float HSV_Green_S_Max = 255f;
    private const float HSV_Green_S_Min = 43f;
    private const float HSV_Green_V_Max = 255f;
    private const float HSV_Green_V_Min = 43f;
    //HSV BLUE
    private const float HSV_Blue_H_Max = 124f;
    private const float HSV_Blue_H_Min = 100f;
    private const float HSV_Blue_S_Max = 255f;
    private const float HSV_Blue_S_Min = 43f;
    private const float HSV_Blue_V_Max = 255f;
    private const float HSV_Blue_V_Min = 43f;
    //HSV RED
    private const float HSV_Red_H_Max = 10f;
    private const float HSV_Red_H_Min = 0f;
    private const float HSV_Red_S_Max = 255f;
    private const float HSV_Red_S_Min = 43f;
    private const float HSV_Red_V_Max = 255f;
    private const float HSV_Red_V_Min = 43f;
    //HSV WHITE
    private const float HSV_White_H_Max = 180f;
    private const float HSV_White_H_Min = 0f;
    private const float HSV_White_S_Max = 30f;
    private const float HSV_White_S_Min = 0f;
    private const float HSV_White_V_Max = 255f;
    private const float HSV_White_V_Min = 221f;
    //HSV PURPLE
    private const float HSV_Purple_H_Max = 155f;
    private const float HSV_Purple_H_Min = 125f;
    private const float HSV_Purple_S_Max = 255f;
    private const float HSV_Purple_S_Min = 43f;
    private const float HSV_Purple_V_Max = 255f;
    private const float HSV_Purple_V_Min = 46f;
    //HSV CYAN
    private const float HSV_Cyan_H_Max = 99f;
    private const float HSV_Cyan_H_Min = 78f;
    private const float HSV_Cyan_S_Max = 255f;
    private const float HSV_Cyan_S_Min = 43f;
    private const float HSV_Cyan_V_Max = 255f;
    private const float HSV_Cyan_V_Min = 46f;
    //HSV BLACK
    private const float HSV_Black_H_Max = 180f;
    private const float HSV_Black_H_Min = 0f;
    private const float HSV_Black_S_Max = 255f;
    private const float HSV_Black_S_Min = 0f;
    private const float HSV_Black_V_Max = 46f;
    private const float HSV_Black_V_Min = 0f;
    #endregion

    #region public fields

    public Vector3Int GridDimensions { get; private set; }
    public float VoxelSize { get; private set; }
    public Vector3 Origin { get; private set; }

    //public Vector3Int GridDimensions;
    public Voxel[,,] Voxels;
    public Face[][,,] Faces = new Face[3][,,];

    public Vector3 Corner;

    public int PixelsPerVoxel;

    public Vector3 Centre => Origin + (Vector3)GridDimensions * VoxelSize / 2;

    public bool ShowAliveVoxels
    {
        get
        {
            return _showAliveVoxels;
        }
        set
        {
            _showAliveVoxels = value;
            foreach (Voxel voxel in GetVoxels())
            {
                voxel.ShowAliveVoxel = value;
            }
        }
    }

    public bool ShowAvailableVoxels
    {
        get
        {
            return _showAvailableVoxels;
        }
        set
        {
            _showAvailableVoxels = value;
            foreach (Voxel voxel in GetVoxels())
            {
                voxel.ShowAvailableVoxel = value;
            }
        }
    }

    /// <summary>
    /// what percentage of the available grid has been filled up in percentage
    /// </summary>
    public float Efficiency
    {
        get
        {
            //We're storing the voxels as a list so that we don't have to get them twice. counting a list is more efficient than counting an IEnumerable
            List<Voxel> flattenedVoxels = GetVoxels().ToList();
            //if we don't cast this value to a float, it always returns 0 as it is rounding down to integer values
            return (float)flattenedVoxels.Count(v => v.Status == VoxelState.Alive) / flattenedVoxels.Where(v => v.Status != VoxelState.Dead).Count() * 100;
        }
    }

    public List<Connection> GetAvailableConnections()
    {
        List<Connection> availableConnections = new List<Connection>();
        foreach (var block in _blocks.Where(b => b.State == BlockState.Placed))
        {
            availableConnections.AddRange(block.Connections.Where(c => c.Available));
        }
        return availableConnections;
    }

    #region Block fields
    /*public Dictionary<int, GameObject> GOPatternPrefabs
    {
        get
        {
            if (_goPatternPrefabs == null)
            {
                _goPatternPrefabs = new Dictionary<int, GameObject>();
                _goPatternPrefabs.Add(0, Resources.Load("Prefabs/9Shape") as GameObject);
                _goPatternPrefabs.Add(1, Resources.Load("Prefabs/HookShape") as GameObject);
                _goPatternPrefabs.Add(2, Resources.Load("Prefabs/HShape") as GameObject);
                _goPatternPrefabs.Add(3, Resources.Load("Prefabs/LongHShape") as GameObject);
                _goPatternPrefabs.Add(4, Resources.Load("Prefabs/LonglongHShape") as GameObject);
                _goPatternPrefabs.Add(5, Resources.Load("Prefabs/LShape") as GameObject);
                _goPatternPrefabs.Add(6, Resources.Load("Prefabs/LonglongShape") as GameObject);
                _goPatternPrefabs.Add(7, Resources.Load("Prefabs/MidHshape") as GameObject);
                _goPatternPrefabs.Add(8, Resources.Load("Prefabs/OShape") as GameObject);
                _goPatternPrefabs.Add(9, Resources.Load("Prefabs/SShape") as GameObject);
                _goPatternPrefabs.Add(10, Resources.Load("Prefabs/UShape") as GameObject);
                _goPatternPrefabs.Add(11, Resources.Load("Prefabs/YShape") as GameObject);
            }
            return _goPatternPrefabs;
        }
    }*/
    #endregion

    #endregion

    #region private fields
    //private Voxel[,,] Voxels;
    private bool _showAliveVoxels = false;
    private bool _showAvailableVoxels = false;
    ComputeShader computerShader;
    #region block fields
    private List<Block> _blocks = new List<Block>();
    private List<Block> _currentBlocks => _blocks.Where(b => b.State != BlockState.Placed).ToList();

    private Dictionary<int, GameObject> _goPatternPrefabs;
    private int _currentPattern = 1;



    #endregion
    #endregion

    #region Constructors
    /// <summary>
    /// Create a new voxelgrid
    /// </summary>
    /// <param name="gridDimensions">X,Y,Z dimensions of the grid</param>
    /// <param name="voxelSize">The size of the voxels</param>
    /// <param name="origin">Where the voxelgrid starts</param>
    public VoxelGrid(Vector3Int gridDimensions, float voxelSize, Vector3 origin)
    {
        GridDimensions = gridDimensions;
        VoxelSize = voxelSize;
        Origin = origin;

        MakeVoxels();
    }

    //Copy constructor with different signature. will refer to the original constructor
    /// <summary>
    /// Create a new voxelgrid
    /// </summary>
    /// <param name="x">X dimensions of the grid</param>
    /// <param name="y">Y dimensions of the grid</param>
    /// <param name="z">Z dimensions of the grid</param>
    /// <param name="voxelSize">The size of the voxels</param>
    /// <param name="origin">Where the voxelgrid starts</param>
    public VoxelGrid(int x, int y, int z, float voxelSize, Vector3 origin) : this(new Vector3Int(x, y, z), voxelSize, origin) { }


    /// <summary>
    /// Constructor for a basic <see cref="VoxelGrid"/>
    /// </summary>
    /// <param name="size">Size of the grid</param>
    /// <param name="origin">Origin of the grid</param>
    /// <param name="voxelSize">The size of each <see cref="Voxel"/></param>
    public VoxelGrid(Vector3Int size, Vector3 origin, float voxelSize)
    {
        GridDimensions = size;
        Origin = origin;
        VoxelSize = voxelSize;

        Voxels = new Voxel[GridDimensions.x, GridDimensions.y, GridDimensions.z];

        for (int x = 0; x < GridDimensions.x; x++)
        {
            for (int y = 0; y < GridDimensions.y; y++)
            {
                for (int z = 0; z < GridDimensions.z; z++)
                {
                    Voxels[x, y, z] = new Voxel(
                        new Vector3Int(x, y, z),
                        this);
                }
            }
        }

        MakeFaces();
    }

    public VoxelGrid(Texture2D source, int height, Vector3 origin, float voxelSize)
    {
        //06 Read grid dimensions in X and Z from image
        GridDimensions = new Vector3Int(source.width, height, source.height);

        Origin = origin;
        VoxelSize = voxelSize;

        Voxels = new Voxel[GridDimensions.x, GridDimensions.y, GridDimensions.z];

        for (int x = 0; x < GridDimensions.x; x++)
        {
            for (int y = 0; y < GridDimensions.y; y++)
            {
                for (int z = 0; z < GridDimensions.z; z++)
                {
                    Voxels[x, y, z] = new Voxel(new Vector3Int(x, y, z), this);
                }
            }
        }
        MakeFaces();
    }

    public VoxelGrid(Texture2D source, int pixelPerVoxel, int height, Vector3 origin, float voxelSize)
    {

        //06 Read grid dimensions in X and Z from image
        GridDimensions = new Vector3Int(source.width / pixelPerVoxel, height, source.height / pixelPerVoxel);

        Origin = origin;
        VoxelSize = voxelSize;
        PixelsPerVoxel = pixelPerVoxel;

        Voxels = new Voxel[GridDimensions.x, GridDimensions.y, GridDimensions.z];

        for (int x = 0; x < GridDimensions.x; x++)
        {
            for (int y = 0; y < GridDimensions.y; y++)
            {
                for (int z = 0; z < GridDimensions.z; z++)
                {
                    Voxels[x, y, z] = new Voxel(new Vector3Int(x, y, z), this);
                }
            }
        }
    }


    #endregion

    #region private functions
    /// <summary>
    /// Create all the voxels in the grid
    /// </summary>
    private void MakeVoxels()
    {
        Voxels = new Voxel[GridDimensions.x, GridDimensions.y, GridDimensions.z];
        for (int x = 0; x < GridDimensions.x; x++)
        {
            for (int y = 0; y < GridDimensions.y; y++)
            {
                for (int z = 0; z < GridDimensions.z; z++)
                {
                    Voxels[x, y, z] = new Voxel(x, y, z, this);
                }
            }
        }

        //Put this to true if you want to see the voxels on creation of the grid
        ShowAvailableVoxels = false;
        ShowAliveVoxels = true;
    }

    #endregion

    #region public function
    /// <summary>
    /// Get the Voxels of the <see cref="VoxelGrid"/>
    /// </summary>
    /// <returns>A flattened collections of all the voxels</returns>
    public IEnumerable<Voxel> GetVoxels()
    {
        for (int x = 0; x < GridDimensions.x; x++)
            for (int y = 0; y < GridDimensions.y; y++)
                for (int z = 0; z < GridDimensions.z; z++)
                {
                    yield return Voxels[x, y, z];
                }

    }


    //Shorthand syntax for a function returning the output of GetVoxelByIndex
    //Two function with the same name, but different parameters ==> different signature
    /// <summary>
    /// Return a voxel at a certain index
    /// </summary>
    /// <param name="x">x index</param>
    /// <param name="y">y index</param>
    /// <param name="z">z index</param>
    /// <returns>Voxel at x,y,z index. null if the voxel doesn't exist or is out of bounds</returns>
    public Voxel GetVoxelByIndex(int x, int y, int z) => GetVoxelByIndex(new Vector3Int(x, y, z));

    /// <summary>
    /// Return a voxel at a certain index
    /// </summary>
    /// <param name="index">x,y,z index</param>
    /// <returns>Voxel at x,y,z index. null if the voxel doesn't exist or is out of bounds</returns>
    public Voxel GetVoxelByIndex(Vector3Int index)
    {
        if (!Util.CheckInBounds(GridDimensions, index) || Voxels[index.x, index.y, index.z] == null)
        {
            Debug.Log($"A Voxel at {index} doesn't exist");
            return null;
        }
        return Voxels[index.x, index.y, index.z];
    }

    /// <summary>
    /// 
    /// Get all the voxels at a certain XZ layer
    /// </summary>
    /// <param name="yLayer">Y index of the layer</param>
    /// <returns>List of voxels in XZ layer</returns>
    public List<Voxel> GetVoxelsYLayer(int yLayer)
    {
        List<Voxel> yLayerVoxels = new List<Voxel>();

        //Check if the yLayer is within the bounds of the grid
        if (yLayer < 0 || yLayer >= GridDimensions.y)
        {
            Debug.Log($"Requested Y Layer {yLayer} is out of bounds");
            return null;
        }

        for (int x = 0; x < GridDimensions.x; x++)
            for (int z = 0; z < GridDimensions.z; z++)
                yLayerVoxels.Add(Voxels[x, yLayer, z]);

        return yLayerVoxels;
    }

    /// <summary>
    /// Set the entire grid 'Alive' to a certain state
    /// </summary>
    /// <param name="state">the state to set</param>
    public void SetGridState(VoxelState state)
    {
        foreach (var voxel in Voxels)
        {
            voxel.Status = state;
        }
    }

    /// <summary>
    /// Set the non dead voxels of the  grid to a certain state
    /// </summary>
    /// <param name="state">the state to set</param>
    public void SetNonDeadGridState(VoxelState state)
    {
        foreach (var voxel in GetVoxels().Where(v => v.Status != VoxelState.Dead))
        {
            voxel.Status = state;
        }
    }
    /// <summary>
    /// Copy all the layers one layer up, starting from the top layer going down.
    /// The bottom layer will remain and the top layer will dissapear.
    /// </summary>
    public void CopyLayersUp()
    {
        //Check the signature of the for loop. Starting at the top layer and going down
        for (int y = GridDimensions.y - 1; y > 0; y--)
        {
            for (int x = 0; x < GridDimensions.x; x++)
            {
                for (int z = 0; z < GridDimensions.z; z++)
                {
                    Voxels[x, y, z].Status = Voxels[x, y - 1, z].Status;
                }
            }
        }
    }

    /// <summary>
    /// Get the number of neighbouring voxels that are alive
    /// </summary>
    /// <param name="voxel">the voxel to get the neighbours from</param>
    /// <returns>number of alive neighbours</returns>
    public int GetNumberOfAliveNeighbours(Voxel voxel)
    {
        int nrOfAliveNeighbours = 0;
        foreach (var vox in voxel.GetFaceNeighbourList())
        {
            if (vox.Status == VoxelState.Alive) nrOfAliveNeighbours++;
        }

        return nrOfAliveNeighbours;
    }

    /// <summary>
    /// Get a random voxel with the Status Available
    /// </summary>
    /// <returns>The random available voxel</returns>
    public Voxel GetRandomAvailableVoxel()
    {
        List<Voxel> voxels = new List<Voxel>(GetVoxels().Where(v => v.Status == VoxelState.Available));
        return voxels[Random.Range(0, voxels.Count)];
    }

    #region Block functionality
    /// <summary>
    /// Set a random pattern index based on all the possible patterns in Pattern list.
    /// </summary>
    public void SetRandomPatternIndex() =>
        _currentPattern = Random.Range(0, PatternManager.Patterns.Count);

    /// <summary>
    /// Set a random pattern index based on all the possible patterns in Pattern list.
    /// </summary>
    public void SetPatternIndex(int index)
    {
        if (index >= 0 && index < PatternManager.Patterns.Count)
            _currentPattern = index;
        else
            Debug.LogWarning($"There's not pattern with Index {index}");
    }



    /// <summary>
    /// Temporary add a block to the grid. To confirm the block at it's current position, use the TryAddCurrentBlocksToGrid function
    /// </summary>
    /// <param name="anchor">The voxel where the pattern will start building from index(0,0,0) in the pattern</param>
    /// <param name="rotation">The rotation for the current block. This will be rounded to the nearest x,y or z axis</param>
    public void AddBlock(Vector3Int anchor, Quaternion rotation) => _blocks.Add(new Block(_currentPattern, anchor, rotation, this));

    /// <summary>
    /// Try to add the blocks that are currently pending to the grids
    /// </summary>
    /// <returns>true if the function managed to place all the current blocks. False in all other cases</returns>
    public bool TryAddCurrentBlocksToGrid()
    {
        //Stop if there are no blocks to add
        if (_currentBlocks == null || _currentBlocks.Count == 0)
        {
            _currentBlocks.Clear();
            return false;
        }
        //Stop if there are no valid blocks to add
        if (_currentBlocks.Count(b => b.State == BlockState.Valid) == 0)
        {
            _currentBlocks.Clear();
            return false;
        }

        //Keep adding blocks to the grid untill all the pending blocks are added
        int counter = 0;
        while (counter < _currentBlocks.Count)
        {
            var block = _currentBlocks[counter];
            block.ActivateVoxels();
        }

        return true;
    }

    /// <summary>
    /// Remove all pending blocks from the grid
    /// </summary>
    public void PurgeUnplacedBlocks()
    {
        _blocks.RemoveAll(b => b.State != BlockState.Placed);
    }

    public void PurgeAllBlocks()
    {
        foreach (var block in _blocks)
        {
            block.DestroyBlock();
        }
        _blocks = new List<Block>();
    }

    /// <summary>
    /// Counts the number of blocks placed in the voxelgrid
    /// </summary>
    public int NumberOfBlocks => _blocks.Count(b => b.State == BlockState.Placed);

    #endregion
    #endregion

    #region Grid elements constructors

    /// <summary>
    /// Creates the Faces of each <see cref="Voxel"/>
    /// </summary>
    private void MakeFaces()
    {
        // make faces
        Faces[0] = new Face[GridDimensions.x + 1, GridDimensions.y, GridDimensions.z];

        for (int x = 0; x < GridDimensions.x + 1; x++)
            for (int y = 0; y < GridDimensions.y; y++)
                for (int z = 0; z < GridDimensions.z; z++)
                {
                    Faces[0][x, y, z] = new Face(x, y, z, Axis.X, this);
                }

        Faces[1] = new Face[GridDimensions.x, GridDimensions.y + 1, GridDimensions.z];

        for (int x = 0; x < GridDimensions.x; x++)
            for (int y = 0; y < GridDimensions.y + 1; y++)
                for (int z = 0; z < GridDimensions.z; z++)
                {
                    Faces[1][x, y, z] = new Face(x, y, z, Axis.Y, this);
                }

        Faces[2] = new Face[GridDimensions.x, GridDimensions.y, GridDimensions.z + 1];

        for (int x = 0; x < GridDimensions.x; x++)
            for (int y = 0; y < GridDimensions.y; y++)
                for (int z = 0; z < GridDimensions.z + 1; z++)
                {
                    Faces[2][x, y, z] = new Face(x, y, z, Axis.Z, this);
                }
    }

    #endregion

    #region Grid operations
    public void SetStatesFromImageReduced(Texture2D sourceImage)
    {      
        //FunctionColour[,] combinedColours = new FunctionColour[GridDimensions.x, GridDimensions.z];

        //Loop over all the XZ voxels
        for (int x = 0; x < GridDimensions.x; x++)
        {
            for (int z = 0; z < GridDimensions.z; z++)
            {
                FunctionColour[] pixels = new FunctionColour[PixelsPerVoxel * PixelsPerVoxel];

                for (int i = 0; i < PixelsPerVoxel; i++)
                {
                    for (int j = 0; j < PixelsPerVoxel; j++)
                    {
                        int xPixIndex = x * PixelsPerVoxel + i;
                        int zPixIndex = z * PixelsPerVoxel + j;
                        pixels[i * PixelsPerVoxel + j] = GetPixelStateFromImage(sourceImage.GetPixel(xPixIndex, zPixIndex));
                    }
                }
                //Debug.Log(pixels.Count(p => p == FunctionColour.Green));
                //Check if the voxel should be green

                var countPerFunction = new Dictionary<FunctionColour, int>();
                foreach (var pixel in pixels)
                {
                    if (countPerFunction.ContainsKey(pixel)) countPerFunction[pixel]++;
                    else countPerFunction[pixel] = 1;
                }

                if (pixels.Count(p => p == FunctionColour.Black) > 0)
                {
                    for (int y = 0; y < Util.IndexPerFunction[FunctionColour.Black]; y++)
                    {

                        Voxel voxel = Voxels[x, y, z];
                        voxel.SetState(FunctionColour.Black, true);
                    }

                    for (int y = Util.IndexPerFunction[FunctionColour.Black]; y < GridDimensions.y; y++)
                    {
                        Voxel voxel = Voxels[x, y, z];
                        voxel.SetState(FunctionColour.Black, false);
                    }
                }
                else
                {
                    var maxCount = countPerFunction.Values.Max();
                    var maxColour = countPerFunction.First(p => p.Value == maxCount).Key;
                    for (int y = 0; y < Util.IndexPerFunction[maxColour]; y++)
                    {
                        Voxel voxel = Voxels[x, y, z];
                        voxel.SetState(maxColour, true);
                    }

                    for (int y = Util.IndexPerFunction[maxColour]; y < GridDimensions.y; y++)
                    {
                        Voxel voxel = Voxels[x, y, z];
                        voxel.SetState(maxColour, false);
                    }
                }

            }
        }
    }

    private FunctionColour GetPixelStateFromImage(Color pixel)
    {

        float avgColor = (pixel.r + pixel.g + pixel.b) / 3f;

        float h = 0;
        float s = 0;
        float v = 0;

        Color.RGBToHSV(pixel, out h, out s, out v);

        if (h * HSV_H >= HSV_Red_H_Min && h * HSV_H <= HSV_Red_H_Max && s * HSV_S >= HSV_Red_S_Min && s * HSV_S <= HSV_Red_S_Max && v * HSV_V >= HSV_Red_V_Min && v * HSV_V <= HSV_Red_V_Max)
        {
            return FunctionColour.Red;
        }

        else if (h * HSV_H >= HSV_Yellow_H_Min && h * HSV_H <= HSV_Yellow_H_Max && s * HSV_S >= HSV_Yellow_S_Min && s * HSV_S <= HSV_Yellow_S_Max && v * HSV_V >= HSV_Yellow_V_Min && v * HSV_V <= HSV_Yellow_V_Max)
        {
            return FunctionColour.Yellow;
        }

        else if (h * HSV_H >= HSV_Green_H_Min && h * HSV_H <= HSV_Green_H_Max && s * HSV_S >= HSV_Green_S_Min && s * HSV_S <= HSV_Green_S_Max && v * HSV_V >= HSV_Green_V_Min && v * HSV_V <= HSV_Green_V_Max)
        {
            return FunctionColour.Green;
        }

        else if (h * HSV_H >= HSV_Blue_H_Min && h * HSV_H <= HSV_Blue_H_Max && s * HSV_S >= HSV_Blue_S_Min && s * HSV_S <= HSV_Blue_S_Max && v * HSV_V >= HSV_Blue_V_Min && v * HSV_V <= HSV_Blue_V_Max)
        {
            return FunctionColour.Blue;
        }

        else if (h * HSV_H >= HSV_Purple_H_Min && h * HSV_H <= HSV_Purple_H_Max && s * HSV_S >= HSV_Purple_S_Min && s * HSV_S <= HSV_Purple_S_Max && v * HSV_V >= HSV_Purple_V_Min && v * HSV_V <= HSV_Purple_V_Max)
        {
            return FunctionColour.Purple;
        }

        else if (h * HSV_H >= HSV_Cyan_H_Min && h * HSV_H <= HSV_Cyan_H_Max && s * HSV_S >= HSV_Cyan_S_Min && s * HSV_S <= HSV_Cyan_S_Max && v * HSV_V >= HSV_Cyan_V_Min && v * HSV_V <= HSV_Cyan_V_Max)
        {
            return FunctionColour.Cyan;
        }

        else if (h * HSV_H >= HSV_Black_H_Min && h * HSV_H <= HSV_Black_H_Max && s * HSV_S >= HSV_Black_S_Min && s * HSV_S <= HSV_Black_S_Max && v * HSV_V >= HSV_Black_V_Min && v * HSV_V <= HSV_Black_V_Max)
        {
            return FunctionColour.Black;
        }

        else if (h * HSV_H >= HSV_White_H_Min && h * HSV_H <= HSV_White_H_Max && s * HSV_S >= HSV_White_S_Min && s * HSV_S <= HSV_White_S_Max && v * HSV_V >= HSV_White_V_Min && v * HSV_V <= HSV_White_V_Max)
        {
            return FunctionColour.White;
        }

        else
        {
            return FunctionColour.Void;
        }
    }

    /// <summary>
    /// Get the Faces of the <see cref="VoxelGrid"/>
    /// </summary>
    /// <returns>All the faces</returns>
    public IEnumerable<Face> GetFaces()
    {
        for (int n = 0; n < 3; n++)
        {
            int xSize = Faces[n].GetLength(0);
            int ySize = Faces[n].GetLength(1);
            int zSize = Faces[n].GetLength(2);

            for (int x = 0; x < xSize; x++)
                for (int y = 0; y < ySize; y++)
                    for (int z = 0; z < zSize; z++)
                    {
                        yield return Faces[n][x, y, z];
                    }
        }
    }

    #endregion   
}
