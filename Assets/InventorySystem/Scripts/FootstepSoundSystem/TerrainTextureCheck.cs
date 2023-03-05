using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTextureCheck : MonoBehaviour
{
    public Transform playerTransform;
    public Terrain terrainObject;

    public int posX;
    public int posZ;
    public TerrainTextureValue[] textureValues;

    private void Start()
    {
        terrainObject = Terrain.activeTerrain;
        playerTransform = gameObject.transform;
    }
    public void GetTerrainTexture()
    {
        UpdatePosition();
        CheckTexture();
    }
    private void UpdatePosition()
    {
        Vector3 terrainPosition = playerTransform.position - terrainObject.transform.position;
        Vector3 mapPosition = new Vector3(terrainPosition.x / terrainObject.terrainData.size.x, 0, terrainPosition.z / terrainObject.terrainData.size.z);
        float xCoord = mapPosition.x * terrainObject.terrainData.alphamapWidth;
        float zCoord = mapPosition.z * terrainObject.terrainData.alphamapHeight;
        posX = (int)xCoord;
        posZ = (int)zCoord;
    }
    private void CheckTexture()
    {
        float[,,] splatMap = terrainObject.terrainData.GetAlphamaps(posX, posZ, 1, 1);
        textureValues[0].TextureValue = splatMap[0, 0, 0];
        textureValues[1].TextureValue = splatMap[0, 0, 1];
        textureValues[2].TextureValue = splatMap[0, 0, 2];
        textureValues[3].TextureValue = splatMap[0, 0, 3];
        textureValues[4].TextureValue = splatMap[0, 0, 4];
        textureValues[5].TextureValue = splatMap[0, 0, 5];
        textureValues[6].TextureValue = splatMap[0, 0, 6];
        textureValues[7].TextureValue = splatMap[0, 0, 7];
    }
}
[Serializable]
public struct TerrainTextureValue
{
    public string textureName;
    public float TextureValue;
}