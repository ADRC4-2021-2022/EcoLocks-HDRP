using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class EnvironmentManager : MonoBehaviour
{
    #region Private fields

    VoxelGrid _voxelGrid;
    TextureHSVColor _voxelGrid2;
    //08 Create image variable
    Texture2D _sourceImage;
    Texture2D _sourceImage123;
    Texture2D _sourceImageCopy;
    //44 Create list to stores target voxels
    List<Voxel> _targets = new List<Voxel>();
    GameObject originVoxel;
    #endregion

    #region Unity methods

    public void Start()
    {
        
    }

    public void Update()
    {
        
    }

    #endregion

    #region Public methods

    public void LoadImage()
    {
        _sourceImage = Resources.Load<Texture2D>("Data/new01");
        _voxelGrid = new VoxelGrid(_sourceImage, 3, 5, transform.position, 0.3f);
    }

    //37 Create public method to read image from button
    /// <summary>
    /// Read the image and set the states of the Voxels
    /// </summary>
    public void ReadImage()
    {
        _voxelGrid.SetStatesFromImageReduced(_sourceImage);
        _targets = new List<Voxel>();
    }

    //38 Create public method to change the visibility of the void voxels
    /// <summary>
    /// Change the visibility of the void GraphVoxels
    /// </summary>
    public void VoidsVisibility()
    {     
        GameObject[] voidVoxels = GameObject.FindGameObjectsWithTag("VoidVoxel");
        foreach (GameObject item in voidVoxels)
        {
            item.SetActive(false);
        }   
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Get the voxels that are part of a Path
    /// </summary>
    /// <returns>All the path <see cref="GraphVoxel"/></returns>
    private IEnumerable<Voxel> GetPathVoxels()
    {
        foreach (Voxel voxel in _voxelGrid.Voxels)
        {
            if (voxel.IsPath)
            {
                yield return voxel;
            }
        }
    }

    private Texture2D duplicateTexture(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }

    #endregion
}