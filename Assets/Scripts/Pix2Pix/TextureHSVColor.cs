using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class TextureHSVColor : MonoBehaviour
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


    #region Public fields

    public Vector3Int GridDimensions;
    public Pix2PixVoxel[,,] Voxels;
    public Face[][,,] Faces = new Face[3][,,];
    public Vector3 Origin;
    public Vector3 Corner;
    public float VoxelSize { get; private set; }

    #endregion

    public void SetStatesFromImage(Texture2D source)
    {

        for (int x = 0; x < GridDimensions.x; x++)
        {
            for (int z = 0; z < GridDimensions.z; z++)
            {

                Color pixel = source.GetPixel(x, z);
                float h = 0;
                float s = 0;
                float v = 0;
                
                Color.RGBToHSV(pixel, out h, out s, out v);






                if (h * HSV_H >= HSV_Yellow_H_Min && h * HSV_H <= HSV_Yellow_H_Max && s * HSV_S >= HSV_Yellow_S_Min && s * HSV_S <= HSV_Yellow_S_Max && v * HSV_V >= HSV_Yellow_V_Min && v * HSV_V <= HSV_Yellow_V_Max)
                {
                    Debug.Log("yellow");
                    for (int y = 0; y < GridDimensions.y; y++)
                    {
                        //16 Get Pix2PixVoxel on Coordinate as Pix2PixVoxel
                        Pix2PixVoxel voxel = Voxels[x, y, z];


                        //20 Set state on Pix2PixVoxel
                        voxel.SetStateYellow(0.1f);

                    }

                }

            }

        }
                     

    }




















}

