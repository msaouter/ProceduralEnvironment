using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PoissonDistribution : MonoBehaviour
{

    /* 2D space distribution */
    public static List<Vector3> PointGeneration2D(float radius, Vector3 regionSize, float samplesBeforeRejection)
    {
        float cellSize = radius / Mathf.Sqrt(2);

        int gridX = Mathf.CeilToInt(regionSize.x / cellSize);
        int gridY = Mathf.CeilToInt(regionSize.z / cellSize);

        int[,] grid = new int[gridX, gridY];

        
        for(int i = 0; i < gridX; i++)
        {
            for(int j = 0; j < gridY; j++)
            {
                grid[i, j] = -1;
            }
        }

        List<Vector3> points = new List<Vector3>();
        List<Vector3> spawnPoints = new List<Vector3>();

        spawnPoints.Add(regionSize / 2);


        while(spawnPoints.Count > 0)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            Vector3 spawnCenter = spawnPoints[spawnIndex];

            bool candidateAccepted = false;

            for(int i = 0; i < samplesBeforeRejection; i++)
            {
                float angle = Random.value * Mathf.PI * 2;
                Vector3 dir = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
                Vector3 candidate = spawnCenter + dir * Random.Range(radius, 2 * radius);

                if(isValid2(candidate, regionSize, cellSize, radius, points, grid))
                {
                    points.Add(candidate);
                    spawnPoints.Add(candidate); 
                    grid[(int)(candidate.x / cellSize), (int)(candidate.z / cellSize)] = points.Count;
                    candidateAccepted = true;
                    break;
                }
            }

            if (!candidateAccepted)
            {
                spawnPoints.RemoveAt(spawnIndex);
            }
        }

        return points;
    }


    public static void displayArray(int[,] array, int gridX, int gridY)
    {
        for(int i = 0; i < gridX; i++)
        {
            for(int j = 0; j < gridY; j++)
            {
                Debug.LogError(array[i, j]);
            }
        }
    }


    static bool isValid2(Vector3 candidate, Vector3 regionSize, float cellSize, float radius, List<Vector3> points, int[,] grid)
    {
        if(candidate.x >=0 && candidate.x < regionSize.x && candidate.z >=0 && candidate.z < regionSize.z)
        {
            int cellX = (int)(candidate.x / cellSize);
            int cellZ = (int)(candidate.z / cellSize);

            int searchStartX = Mathf.Max(0, cellX - 2);
            int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);

            int searchStartZ = Mathf.Max(0, cellZ - 2);
            int searchEndZ = Mathf.Min(cellZ + 2, grid.GetLength(1) - 1);

            for(int x = searchStartX; x <= searchEndX; x++)
            {
                for(int y = searchStartZ; y <= searchEndZ; y++)
                {
                    int pointIndex = grid[x, y] - 1;

                    if(pointIndex >= 0)
                    {
                        float sqrDst = (candidate - points[pointIndex]).sqrMagnitude;
                        if(sqrDst < radius * radius)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        return false;
    }



    /********************* 3 dimension **********************/


    /* 3D space distribution */
    public static List<Vector3> PointGeneration3D(float radius, Vector3 regionSize, float samplesBeforeRejection)
    {
        float cellSize = radius / Mathf.Sqrt(2);

        int gridX = Mathf.CeilToInt(regionSize.x / cellSize);
        int gridY = Mathf.CeilToInt(regionSize.y / cellSize);
        int gridZ = Mathf.CeilToInt(regionSize.z / cellSize);

        int[,,] grid = new int[gridX, gridY, gridZ];

        for (int i = 0; i < gridX; i++)
        {
            for (int j = 0; j < gridY; j++)
            {
                for(int k = 0; k < gridZ; k++)
                {
                    grid[i, j, k] = -1;
                }
            }
        }

        List<Vector3> points = new List<Vector3>();
        List<Vector3> spawnPoints = new List<Vector3>();

        spawnPoints.Add(regionSize / 2);


        while (spawnPoints.Count > 0)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            //Debug.Log("spawn : " + spawnPoints[spawnIndex]);
            Vector3 spawnCenter = spawnPoints[spawnIndex];

            bool candidateAccepted = false;

            for (int i = 0; i < samplesBeforeRejection; i++)
            {
                float angle = Random.value * Mathf.PI * 2;
                Vector3 dir = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)); 

                Vector3 candidate = spawnCenter + dir * Random.Range(radius, 2 * radius);

                if (isValid3(candidate, regionSize, cellSize, radius, points, grid))
                {
                    candidate.y = Random.Range((float)0.2 , regionSize.y);

                    points.Add(candidate);
                    spawnPoints.Add(candidate);

                    grid[(int)(candidate.x / cellSize),(int)(candidate.y / cellSize) , (int)(candidate.z / cellSize)] = points.Count;
                    candidateAccepted = true;
                    break;
                }
            }

            if (!candidateAccepted)
            {
                spawnPoints.RemoveAt(spawnIndex);
            }
        }

        return points;
    }

    static bool isValid3(Vector3 candidate, Vector3 regionSize, float cellSize, float radius, List<Vector3> points, int[,,] grid)
    {
        if (candidate.x >= 0 && candidate.x < regionSize.x && candidate.z >= 0 && candidate.z < regionSize.z)
        {
            int cellX = (int)(candidate.x / cellSize);
            int cellY = (int)(candidate.y / cellSize);
            int cellZ = (int)(candidate.z / cellSize);

            int searchStartX = Mathf.Max(0, cellX - 2);
            int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);
            
            int searchStartY = Mathf.Max(0, cellY - 2);
            int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);

            int searchStartZ = Mathf.Max(0, cellZ - 2);
            int searchEndZ = Mathf.Min(cellZ + 2, grid.GetLength(2) - 1);

            for (int x = searchStartX; x <= searchEndX; x++)
            {
                for(int y = searchStartY; y <= searchEndY; y++)
                {
                    for (int z = searchStartZ; z <= searchEndZ; z++)
                    {
                        int pointIndex = grid[x, y, z] - 1;

                        if (pointIndex >= 0)
                        {
                            float sqrDst = (candidate - points[pointIndex]).sqrMagnitude;
                            if (sqrDst < radius * radius)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        return false;
    }


}
