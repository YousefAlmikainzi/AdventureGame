using UnityEngine;

public class BetterApplyHeight : MonoBehaviour
{
    [SerializeField] Terrain ourTerrain;
    [SerializeField] Texture2D heightMapTex;
    [SerializeField] float heightScale = 1.0f;
    [SerializeField] int smoothness = 1;

    void Start()
    {
        ApplyHeights();   
    }
    void ApplyHeights()
    {
        var data = ourTerrain.terrainData;

        int resolution = data.heightmapResolution;

        float[,] heights = new float [resolution, resolution];

        for(int y = 0; y < resolution; y++)
        {
            for(int x = 0; x < resolution; x++)
            {
                float u = (float)x / (resolution - 1);
                float v = (float)y / (resolution - 1);

                Color c = heightMapTex.GetPixelBilinear(u, v);
                float gray = c.r;

                heights[y,x] = gray * heightScale;
            }
        }
        SmoothHeights(heights, smoothness);
        data.SetHeights(0, 0, heights);
    }
    void SmoothHeights(float[,] heights, int iterations)
    {
        int resY = heights.GetLength(0);
        int resX = heights.GetLength(1);
        float[,] temp = new float[resY, resX];

        for (int it = 0; it < iterations; it++)
        {
            for (int y = 1; y < resY - 1; y++)
            {
                for (int x = 1; x < resX - 1; x++)
                {
                    float sum =
                        heights[y, x] +
                        heights[y - 1, x] +
                        heights[y + 1, x] +
                        heights[y, x - 1] +
                        heights[y, x + 1];

                    temp[y, x] = sum / 5f;
                }
            }

            for (int y = 1; y < resY - 1; y++)
            {
                for (int x = 1; x < resX - 1; x++)
                {
                    heights[y, x] = temp[y, x];
                }
            }
        }
    }
}
