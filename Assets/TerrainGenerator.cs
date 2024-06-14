using UnityEngine;
using System.Collections.Generic;

public class TerrainGenerator : MonoBehaviour
{
    public int depth = 20;
    public int width = 256;
    public int height = 256;
    public float scale = 20f;
    public int octaves = 2; // increasing yields terrain detail --> computational cost
    public float persistence = 0.3f; // decreasing yields smoother terrain
    public float lacunarity = 1.5f; // increasing yields more frequent details/more chaotic terrain

    public GameObject tree1;
    public GameObject tree2;
    public GameObject bush1;
    public GameObject bush2;
    public int numberOfObjects = 100;
    public int objectDisplacement = 0;
    public List<GameObject> objectCollection = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
        objectCollection.Add(tree1);
        objectCollection.Add(tree2);
        objectCollection.Add(bush1);
        objectCollection.Add(bush2);

        for (int i = 0; i < numberOfObjects; i++)
        {
            int listReader = Random.Range(0, 3);
            GameObject objectToPlace = objectCollection[listReader];
            int newX = Random.Range(0, width);
            int newY = Random.Range(0, height);
            float newZ = terrain.terrainData.GetHeight(newX, newY);
            Vector3 position = new Vector3(newX, newZ + objectDisplacement, newY);
            GameObject item = Instantiate(objectToPlace, position, Quaternion.identity);
            item.transform.localScale = item.transform.localScale;
            item.SetActive(true);
        }
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, height);
        terrainData.SetHeights(0, 0, GenerateHeights());
        return terrainData;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                heights[x, y] = CalculateHeight(x,y);
            }
        }
        return heights;
    }

    float CalculateHeight(int x, int y)
    {
        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float maxValue = 1; //used for normalizing result

        for (int i = 0; i < octaves; i++)
        {
            float xCoord = x / (float)width * scale * frequency;
            float yCoord = y / (float)height * scale * frequency;
            total += Mathf.PerlinNoise(xCoord, yCoord) * amplitude;
            maxValue += amplitude;
            amplitude *= persistence;
            frequency *= lacunarity;
        }
        
        return total / maxValue;
    }
}
