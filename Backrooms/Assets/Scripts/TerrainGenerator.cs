using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int ySize = 20;
    public int xSize = 256;
    public int zSize = 256;
    public float scale = 2;
    public float offsetX = 2f;
    public float offsetZ = 2f;

    void Start()
    {
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

    private TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = xSize + 1;
        terrainData.size = new Vector3(xSize, ySize, zSize);
        terrainData.SetHeights(0, 0, GenerateHeights());
        return terrainData;
    }

    private float[,] GenerateHeights()
    {
        float[,] heights = new float[xSize, zSize];
        for (int x = 0; x < xSize; x++)
        { 
            for (int z = 0; z < zSize; z++)
            {
                heights[x, z] = CalculateHeight(x, z);
            }
        }

        return heights;
    }

    public float CalculateHeight(float x, float z) {
        float xCord = x / xSize * scale + offsetX;
        float zCord = z / zSize * scale + offsetZ;

        return Mathf.PerlinNoise(xCord, zCord);
    }
}
