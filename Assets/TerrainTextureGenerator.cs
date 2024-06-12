using UnityEngine;

public class TerrainTextureGenerator : MonoBehaviour
{
    public Terrain terrain;
    public TerrainLayer grassLayer;
    public TerrainLayer dirtLayer;
    public TerrainLayer rockLayer;

    public float grassHeightThreshold = 0.3f;
    public float dirtSlopeThreshold = 30f;

    void Start()
    {
        ApplyTextures();
    }

    void ApplyTextures()
    {
        TerrainData terrainData = terrain.terrainData;
        float[,,] splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        for (int y = 0; y < terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < terrainData.alphamapWidth; x++)
            {
                float terrainHeight = terrainData.GetHeight(y, x);
                float terrainSteepness = terrainData.GetSteepness((float)x / terrainData.alphamapWidth, (float)y / terrainData.alphamapHeight);

                float[] splat = new float[splatmapData.GetLength(2)];

                if (terrainHeight < grassHeightThreshold * terrainData.size.y)
                {
                    splat[0] = 1;  // Grass
                }
                else if (terrainSteepness > dirtSlopeThreshold)
                {
                    splat[1] = 1;  // Dirt
                }
                else
                {
                    splat[2] = 1;  // Rock
                }

                for (int i = 0; i < splatmapData.GetLength(2); i++)
                {
                    splatmapData[x, y, i] = splat[i];
                }
            }
        }

        terrainData.SetAlphamaps(0, 0, splatmapData);
    }
}
