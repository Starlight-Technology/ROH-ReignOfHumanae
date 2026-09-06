using UnityEngine;

namespace Assets.Scripts.TerrainHelper
{
    public class TerrainConnector : MonoBehaviour
    {
        public Terrain[] terrains;

        void Start()
        {
            ConnectTerrains();
        }

        void ConnectTerrains()
        {
            int rows = Mathf.FloorToInt(Mathf.Sqrt(terrains.Length));
            int cols = rows;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Terrain current = terrains[i * cols + j];
                    if (j < cols - 1) // Connect to the right terrain
                    {
                        Terrain right = terrains[i * cols + j + 1];
                        MatchEdges(current, right, true);
                    }
                    if (i < rows - 1) // Connect to the bottom terrain
                    {
                        Terrain bottom = terrains[(i + 1) * cols + j];
                        MatchEdges(current, bottom, false);
                    }
                }
            }
        }

        void MatchEdges(Terrain current, Terrain adjacent, bool horizontal)
        {
            float[,] heightsCurrent = current.terrainData.GetHeights(0, 0, current.terrainData.heightmapResolution, current.terrainData.heightmapResolution);
            float[,] heightsAdjacent = adjacent.terrainData.GetHeights(0, 0, adjacent.terrainData.heightmapResolution, adjacent.terrainData.heightmapResolution);

            int resolution = current.terrainData.heightmapResolution;
            for (int k = 0; k < resolution; k++)
            {
                if (horizontal)
                {
                    heightsCurrent[k, resolution - 1] = heightsAdjacent[k, 0]; // Right edge of current to left edge of adjacent
                    heightsAdjacent[k, 0] = heightsCurrent[k, resolution - 1]; // Left edge of adjacent to right edge of current
                }
                else
                {
                    heightsCurrent[resolution - 1, k] = heightsAdjacent[0, k]; // Bottom edge of current to top edge of adjacent
                    heightsAdjacent[0, k] = heightsCurrent[resolution - 1, k]; // Top edge of adjacent to bottom edge of current
                }
            }

            current.terrainData.SetHeights(0, 0, heightsCurrent);
            adjacent.terrainData.SetHeights(0, 0, heightsAdjacent);
        }
    }
}