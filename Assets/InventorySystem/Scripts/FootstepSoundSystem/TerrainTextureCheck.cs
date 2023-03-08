using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTextureCheck : MonoBehaviour
{
    //Code made by Victor Paulo Melo da Silva - Junior Unity Programmer - https://www.linkedin.com/in/victor-nekra-dev/
    //TerrainTextureCheck - Code Update Version 0.2 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Object References - 
    public Transform playerTransform => gameObject.transform;
    public Terrain terrainObject => Terrain.activeTerrain;
    #endregion

    #region - Player Position -
    public int posX;
    public int posZ;
    #endregion

    #region - Player Position on Texture Storage -
    public TerrainTextureValue[] textureValues;
    #endregion

    #region - Player Texture Chekcer Update -
    public void GetTerrainTexture()//This method update all terrain texture checker data
    {
        UpdatePosition();
        CheckTexture();
    }
    #endregion

    #region - Player Position Gathering -
    private void UpdatePosition()//This method calculates the current player position on terrain, considering the terrain width and height
    {
        Vector3 terrainPosition = playerTransform.position - terrainObject.transform.position;
        Vector3 mapPosition = new Vector3(terrainPosition.x / terrainObject.terrainData.size.x, 0, terrainPosition.z / terrainObject.terrainData.size.z);
        float xCoord = mapPosition.x * terrainObject.terrainData.alphamapWidth;
        float zCoord = mapPosition.z * terrainObject.terrainData.alphamapHeight;
        posX = (int)xCoord;
        posZ = (int)zCoord;
    }
    #endregion

    #region - Player Stepped Texture Indentifier -
    private void CheckTexture()//This method indentifies the texture that the player is stepping
    {
        float[,,] splatMap = terrainObject.terrainData.GetAlphamaps(posX, posZ, 1, 1);

        for (int i = 0; i < textureValues.Length; i++) textureValues[i].TextureValue = splatMap[0, 0, i];//This
                                                                                                         //identifies witch texture its been stepped and set his value to 1 
    }
    #endregion
}
#region - Terrain Texture Struct Data -
[Serializable]
public struct TerrainTextureValue//This struct representes and Texture data value that storage not just your value, but also his indentification name/tag
{
    public string textureName;
    public float TextureValue;
}
#endregion