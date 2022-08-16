using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Pix2PixVoxelGrid

{

    private const int HSV_H = 360;
    private const int HSV_S = 255;
    private const int HSV_V = 255;

    //HSV YELLOW
    private const float HSV_Yellow_H_Max = 68f;
    private const float HSV_Yellow_H_Min = 26f;
    private const float HSV_Yellow_S_Max = 255f;
    private const float HSV_Yellow_S_Min = 43f;
    private const float HSV_Yellow_V_Max = 255f;
    private const float HSV_Yellow_V_Min = 43f;
    //HSV GREEN
    private const float HSV_Green_H_Max = 154f;
    private const float HSV_Green_H_Min = 35f;
    private const float HSV_Green_S_Max = 255f;
    private const float HSV_Green_S_Min = 43f;
    private const float HSV_Green_V_Max = 255f;
    private const float HSV_Green_V_Min = 43f;
    //HSV BLUE
    private const float HSV_Blue_H_Max = 248f;
    private const float HSV_Blue_H_Min = 100f;
    private const float HSV_Blue_S_Max = 255f;
    private const float HSV_Blue_S_Min = 43f;
    private const float HSV_Blue_V_Max = 255f;
    private const float HSV_Blue_V_Min = 43f;
    //HSV RED
    private const float HSV_Red_H_Max = 20f;
    private const float HSV_Red_H_Min = 0f;
    private const float HSV_Red_S_Max = 255f;
    private const float HSV_Red_S_Min = 43f;
    private const float HSV_Red_V_Max = 255f;
    private const float HSV_Red_V_Min = 43f;
    //HSV WHITE
    private const float HSV_White_H_Max = 360f;
    private const float HSV_White_H_Min = 0f;
    private const float HSV_White_S_Max = 60f;
    private const float HSV_White_S_Min = 0f;
    private const float HSV_White_V_Max = 255f;
    private const float HSV_White_V_Min = 221f;







    #region Public fields

    public Vector3Int GridDimensions;
    public Pix2PixVoxel[,,] Voxels;
    public Face[][,,] Faces = new Face[3][,,];
    public Vector3 Origin;
    public Vector3 Corner;
    public float VoxelSize { get; private set; }


    public int PixelsPerVoxel;
    #endregion

    #region Constructors

    /// <summary>
    /// Constructor for a basic <see cref="VoxelGrid"/>
    /// </summary>
    /// <param name="size">Size of the grid</param>
    /// <param name="origin">Origin of the grid</param>
    /// <param name="voxelSize">The size of each <see cref="Voxel"/></param>
    public Pix2PixVoxelGrid(Vector3Int size, Vector3 origin, float voxelSize)
    {
        GridDimensions = size;
        Origin = origin;
        VoxelSize = voxelSize;

        Voxels = new Pix2PixVoxel[GridDimensions.x, GridDimensions.y, GridDimensions.z];

        for (int x = 0; x < GridDimensions.x; x++)
        {
            for (int y = 0; y < GridDimensions.y; y++)
            {
                for (int z = 0; z < GridDimensions.z; z++)
                {
                    Voxels[x, y, z] = new Pix2PixVoxel(
                        new Vector3Int(x, y, z),
                        this,
                        0.96f);
                }
            }
        }

        //MakeFaces();
    }

    //05 Create the constructor for the grid, based on an image
    /// <summary>
    /// Constructs a <see cref="VoxelGrid"/> with <see cref="Voxel"/> from an image
    /// </summary>
    /// <param name="source">The source <see cref="Texture2D"/></param>
    /// <param name="height">The height of the <see cref="VoxelGrid"/> to be constructed</param>
    /// <param name="origin">Origin vector of the grid</param>
    /// <param name="voxelSize">The size of each <see cref="Voxel"/></param>
    public Pix2PixVoxelGrid(Texture2D source, int height, Vector3 origin, float voxelSize)
    {
        //06 Read grid dimensions in X and Z from image
        GridDimensions = new Vector3Int(source.width, height, source.height);

        Origin = origin;
        VoxelSize = voxelSize;

        Voxels = new Pix2PixVoxel[GridDimensions.x, GridDimensions.y, GridDimensions.z];

        for (int x = 0; x < GridDimensions.x; x++)
        {
            for (int y = 0; y < GridDimensions.y; y++)
            {
                for (int z = 0; z < GridDimensions.z; z++)
                {
                    Voxels[x, y, z] = new Pix2PixVoxel(new Vector3Int(x, y, z), this, 1f, 0.96f);
                }
            }
        }

        //07 Add make Faces
        //MakeFaces();
    }

    //05 Create the constructor for the grid, based on an image
    /// <summary>
    /// Constructs a <see cref="VoxelGrid"/> with <see cref="Voxel"/> from an image
    /// </summary>
    /// <param name="source">The source <see cref="Texture2D"/></param>
    /// <param name="pixelPerVoxel">amount of pixel per voxel in 1 dimension</param>
    /// <param name="height">The height of the <see cref="VoxelGrid"/> to be constructed</param>
    /// <param name="origin">Origin vector of the grid</param>
    /// <param name="voxelSize">The size of each <see cref="Voxel"/></param>
    public Pix2PixVoxelGrid(Texture2D source, int pixelPerVoxel, int height, Vector3 origin, float voxelSize)
    {
        //06 Read grid dimensions in X and Z from image
        GridDimensions = new Vector3Int(source.width / pixelPerVoxel, height, source.height / pixelPerVoxel);

        Origin = origin;
        VoxelSize = voxelSize;
        PixelsPerVoxel = pixelPerVoxel;

        Voxels = new Pix2PixVoxel[GridDimensions.x, GridDimensions.y, GridDimensions.z];

        for (int x = 0; x < GridDimensions.x; x++)
        {
            for (int y = 0; y < GridDimensions.y; y++)
            {
                for (int z = 0; z < GridDimensions.z; z++)
                {
                    Voxels[x, y, z] = new Pix2PixVoxel(new Vector3Int(x, y, z), this, 1f, 0.96f);
                }
            }
        }

        //07 Add make Faces
        //MakeFaces();
    }

    #endregion

    #region Grid elements constructors
    /*
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
    */
    #endregion

    #region Grid operations

    //11 Create method to read states from images
    /// <summary>
    /// Sets the states of each <see cref="Voxel"/> based on a <see cref="Texture2D"/>
    /// </summary>
    /// <param name="source">The source image</param>
    public void SetStatesFromImage(Texture2D source)
    {
        
        for (int x = 0; x < GridDimensions.x; x++)
        {
            for (int z = 0; z < GridDimensions.z; z++)
            {

                Color pixel = source.GetPixel(x, z);
                float avgColor = (pixel.r + pixel.g + pixel.b) / 3f;

                float h = 0;
                float s = 0;
                float v = 0;

                Color.RGBToHSV(pixel, out h, out s, out v);

                if (h * HSV_H >= HSV_Red_H_Min && h * HSV_H <= HSV_Red_H_Max && s * HSV_S >= HSV_Red_S_Min && s * HSV_S <= HSV_Red_S_Max && v * HSV_V >= HSV_Red_V_Min && v * HSV_V <= HSV_Red_V_Max)
                {
                    Debug.Log("red");
                    for (int y = 0; y < GridDimensions.y; y++)
                    {
                        //16 Get Pix2PixVoxel on Coordinate as Pix2PixVoxel
                        Pix2PixVoxel voxel = Voxels[x, y, z];

                        //19 Calculate state based on height, decreasing as it goes up


                        //20 Set state on Pix2PixVoxel
                        voxel.SetStateRed(avgColor);

                    }


                }
                else if (h * HSV_H >= HSV_White_H_Min && h * HSV_H <= HSV_White_H_Max && s * HSV_S >= HSV_White_S_Min && s * HSV_S <= HSV_White_S_Max && v * HSV_V >= HSV_White_V_Min && v * HSV_V <= HSV_White_V_Max)
                {
                    Debug.Log("white");
                    for (int y = 0; y < GridDimensions.y; y++)
                    {

                        Pix2PixVoxel voxel = Voxels[x, y, z];

                        voxel.SetStateWhite(0.1f);

                    }

                }

                else if (h * HSV_H >= HSV_Yellow_H_Min && h * HSV_H <= HSV_Yellow_H_Max && s * HSV_S >= HSV_Yellow_S_Min && s * HSV_S <= HSV_Yellow_S_Max && v * HSV_V >= HSV_Yellow_V_Min && v * HSV_V <= HSV_Yellow_V_Max)
                {
                    Debug.Log("yellow");

                    for (int y = 0; y < GridDimensions.y; y++)
                    {

                        Pix2PixVoxel voxel = Voxels[x, y, z];

                        voxel.SetStateYellow(0.1f);

                    }

                }


                else if (h * HSV_H >= HSV_Green_H_Min && h * HSV_H <= HSV_Green_H_Max && s * HSV_S >= HSV_Green_S_Min && s * HSV_S <= HSV_Green_S_Max && v * HSV_V >= HSV_Green_V_Min && v * HSV_V <= HSV_Green_V_Max)
                {
                    Debug.Log("green");
                    for (int y = 0; y < GridDimensions.y; y++)
                    {

                        Pix2PixVoxel voxel = Voxels[x, y, z];

                        voxel.SetStateGreen(0.1f);

                    }


                }


                else if (h * HSV_H >= HSV_Blue_H_Min && h * HSV_H <= HSV_Blue_H_Max && s * HSV_S >= HSV_Blue_S_Min && s * HSV_S <= HSV_Blue_S_Max && v * HSV_V >= HSV_Blue_V_Min && v * HSV_V <= HSV_Blue_V_Max)
                {
                    Debug.Log("blue");
                    for (int y = 0; y < GridDimensions.y; y++)
                    {
                        //16 Get Pix2PixVoxel on Coordinate as Pix2PixVoxel
                        Pix2PixVoxel voxel = Voxels[x, y, z];


                        //20 Set state on Pix2PixVoxel
                        voxel.SetStateBlue(0.1f);

                    }
                }
            }
        }

    }

    public void SetStageFromImageReduced(Texture2D sourceImage)
    {
        FunctionColour[,] combinedColours = new FunctionColour[GridDimensions.x, GridDimensions.z];

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
                if (pixels.Count(p => p == FunctionColour.Green) > 0)
                {
                    Debug.Log("green");
                    for (int y = 0; y < GridDimensions.y; y++)
                    {

                        Pix2PixVoxel voxel = Voxels[x, y, z];

                        voxel.SetStateGreen(0.1f);

                    }
                }

                //Check if the voxel should be white
                if (pixels.Count(p => p == FunctionColour.White) / (PixelsPerVoxel * PixelsPerVoxel) > 0.3f)
                {
                    Debug.Log("White");
                    for (int y = 0; y < GridDimensions.y; y++)
                    {

                        Pix2PixVoxel voxel = Voxels[x, y, z];

                        voxel.SetStateWhite(0.1f);

                    }
                }

                //Check if the voxel should be yellow
                if (pixels.Count(p => p == FunctionColour.Yellow) / (PixelsPerVoxel * PixelsPerVoxel) > 0.3f)
                {
                    Debug.Log("Yellow");
                    for (int y = 0; y < GridDimensions.y; y++)
                    {

                        Pix2PixVoxel voxel = Voxels[x, y, z];

                        voxel.SetStateYellow(0.1f);

                    }
                }

                //Check if the voxel should be red
                if (pixels.Count(p => p == FunctionColour.Red) / (PixelsPerVoxel * PixelsPerVoxel) > 0.3f)
                {
                    Debug.Log("Red");
                    for (int y = 0; y < GridDimensions.y; y++)
                    {

                        Pix2PixVoxel voxel = Voxels[x, y, z];

                        voxel.SetStateRed(0.1f);

                    }
                }

                //Check if the voxel should be blue
                if (pixels.Count(p => p == FunctionColour.Blue) / (PixelsPerVoxel * PixelsPerVoxel) > 0.3f)
                {
                    Debug.Log("Blue");
                    for (int y = 0; y < GridDimensions.y; y++)
                    {

                        Pix2PixVoxel voxel = Voxels[x, y, z];

                        voxel.SetStateBlue(0.1f);

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
        else if (h * HSV_H >= HSV_White_H_Min && h * HSV_H <= HSV_White_H_Max && s * HSV_S >= HSV_White_S_Min && s * HSV_S <= HSV_White_S_Max && v * HSV_V >= HSV_White_V_Min && v * HSV_V <= HSV_White_V_Max)
        {
            return FunctionColour.White;

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

        return FunctionColour.White;
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

    /// <summary>
    /// Get the Voxels of the <see cref="VoxelGrid"/>
    /// </summary>
    /// <returns>All the Voxels</returns>
    public IEnumerable<Pix2PixVoxel> GetVoxels()
    {
        for (int x = 0; x < GridDimensions.x; x++)
            for (int y = 0; y < GridDimensions.y; y++)
                for (int z = 0; z < GridDimensions.z; z++)
                {
                    yield return Voxels[x, y, z];
                }
    }

    #endregion


}