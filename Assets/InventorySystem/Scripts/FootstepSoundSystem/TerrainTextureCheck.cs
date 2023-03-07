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

        for (int i = 0; i < textureValues.Length; i++) textureValues[i].TextureValue = splatMap[0, 0, i];
    }
}
[Serializable]
public struct TerrainTextureValue
{
    public string textureName;
    public float TextureValue;
}