/*using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class BalloonType
{
    public GameObject prefab;
    public float weight = 1f;
}

public class SpawmerBalloonTerrain : MonoBehaviour
{
    public List<BalloonType> balloonTypes;
    public int balloonCount = 10;
    public Vector3 balloonSize = new Vector3(2, 4, 2);
    public float minDistance = 3f;
    public float minHeight = 10f;
    public float maxHeight = 50f;
    public Terrain terrain;

    private List<Bounds> placedBounds = new List<Bounds>();

    void Start()
    {
        SpawnBalloons();
    }

    void SpawnBalloons()
    {
        for (int i = 0; i < balloonCount; i++)
        {
            Bounds? validBounds = GetValidBounds();

            if (validBounds.HasValue)
            {
                Vector3 spawnPos = validBounds.Value.center;
                GameObject prefab = GetRandomBalloonPrefab();
                GameObject balloon = Instantiate(prefab, spawnPos, Quaternion.identity);
                balloon.transform.localScale = balloonSize;
                placedBounds.Add(GetFullBounds(balloon));
            }
        }
    }

    GameObject GetRandomBalloonPrefab()
    {
        float totalWeight = balloonTypes.Sum(bt => bt.weight);
        float rand = Random.Range(0, totalWeight);
        float cumulative = 0f;

        foreach (var bt in balloonTypes)
        {
            cumulative += bt.weight;
            if (rand <= cumulative)
                return bt.prefab;
        }

        return balloonTypes[0].prefab;
    }

    Bounds? GetValidBounds()
    {
        int maxAttempts = 100;

        for (int i = 0; i < maxAttempts; i++)
        {
            float x = Random.Range(0, terrain.terrainData.size.x);
            float z = Random.Range(0, terrain.terrainData.size.z);
            float y = Random.Range(minHeight, maxHeight);
            float terrainY = terrain.SampleHeight(new Vector3(x, 0, z)) + terrain.transform.position.y;

            Vector3 worldPos = new Vector3(x, y, z);

            GameObject temp = Instantiate(balloonTypes[0].prefab, worldPos, Quaternion.identity);
            temp.transform.localScale = balloonSize;
            Bounds b = GetFullBounds(temp);
            DestroyImmediate(temp);

            if (y - b.extents.y > terrainY && IsFarFromOthers(b))
                return b;
        }

        return null;
    }

    bool IsFarFromOthers(Bounds newBounds)
    {
        foreach (Bounds existing in placedBounds)
        {
            if (existing.Intersects(newBounds))
                return false;
        }
        return true;
    }

    Bounds GetFullBounds(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
            return new Bounds(obj.transform.position, Vector3.zero);

        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }
        return bounds;
    }
}
*/