using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class BalloonType
{
    public GameObject prefab;
    public float weight = 1f;
}

public class SpawmerBalloon : MonoBehaviour
{
    public List<BalloonType> balloonTypes;
    public int balloonCount = 10;
    public Vector3 balloonSize = new Vector3(3, 3, 3);
    public float minDistance = 3f;
    public float minHeight = 10f;
    public float maxHeight = 50f;

    private List<Bounds> placedBounds = new List<Bounds>();
    private List<GameObject> collidables;
    private List<GameObject> planes;

    void Start()
    {
        initializateAndSpawn();
    }
    
    public void initializateAndSpawn()
    {
        collidables = ObtenerObjetosPorTags(new List<string> { "Colissionable_Object"});
        planes = ObtenerObjetosPorTags(new List<string> { "Floor" });
        SpawnBalloons();
    }

    public void SpawnBalloons()
    {
        for (int i = 0; i < balloonCount; i++)
        {
            Bounds? validBounds = GetValidBounds();
            if (validBounds.HasValue)
            {   
                Vector3 spawnPos = validBounds.Value.center;
                GameObject prefab = GetRandomBalloonPrefab();
                Quaternion randomYRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
                GameObject balloon = Instantiate(prefab, spawnPos, randomYRotation);
                balloon.transform.localScale = balloonSize;
                placedBounds.Add(GetFullBounds(balloon));

				// Initialize
				Balloon b = balloon.GetComponent<Balloon>();
				if (b != null)
				{
					b.Init();
				}
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
        List<GameObject> planes = ObtenerObjetosPorTags(new List<string> { "Floor" });
        if (planes.Count == 0) return null;

        Bounds floorBounds = GetCombinedFloorBounds(planes);
        int maxAttempts = 100;

        for (int i = 0; i < maxAttempts; i++)
        {
            float x = Random.Range(floorBounds.min.x, floorBounds.max.x);
            float z = Random.Range(floorBounds.min.z, floorBounds.max.z);
            float y = Random.Range(minHeight, maxHeight);

            Vector3 worldPos = new Vector3(x, y, z);

            GameObject temp = Instantiate(balloonTypes[0].prefab, worldPos, Quaternion.identity);
            temp.transform.localScale = balloonSize;
            Bounds b = GetFullBounds(temp);
            DestroyImmediate(temp);

            if (IsAboveAnyFloor(planes, b) && IsFarFromOthers(b))
                return b;
        }

        return null;
    }


    bool IsAboveAnyFloor(List<GameObject> floors, Bounds balloonBounds)
    {
        RaycastHit hit;
        Vector3 from = balloonBounds.center;
        Vector3 down = Vector3.down;

        if (Physics.Raycast(from, down, out hit, Mathf.Infinity))
        {
            return floors.Contains(hit.collider.gameObject) || floors.Contains(hit.collider.transform.root.gameObject);
        }

        return false;
    }



    Bounds GetCombinedFloorBounds(List<GameObject> floors)
    {
        if (floors == null || floors.Count == 0)
            return new Bounds(Vector3.zero, Vector3.zero);

        Bounds combinedBounds = GetFullBounds(floors[0]);

        for (int i = 1; i < floors.Count; i++)
        {
            combinedBounds.Encapsulate(GetFullBounds(floors[i]));
        }

        return combinedBounds;
    }


    bool IsFarFromOthers(Bounds newBounds)
    {
        // Verifica colisión con globos ya existentes
        foreach (Bounds existing in placedBounds)
        {
            if (existing.Intersects(newBounds))
                return false;
        }

        // Verifica colisión con objetos colisionables
        foreach (var obj in collidables)
        {
            if (obj == null) continue;

            Bounds otherBounds = GetFullBounds(obj);
            if (newBounds.Intersects(otherBounds))
                return false;
        }

        return true;
    }


    Bounds GetFullBounds(GameObject obj)
    {
        Renderer[] renderers;

        if (obj.CompareTag("Aerostatic_Ballon"))
        {
            // Usa todos los Renderers normalmente
            renderers = obj.GetComponentsInChildren<Renderer>();
        }
        else if (obj.CompareTag("Balloon"))
        {
            // Solo usa Renderers que también tengan el tag "Balloon"
            renderers = obj.GetComponentsInChildren<Renderer>();
            renderers = System.Array.FindAll(renderers, r => r.gameObject.CompareTag("Balloon"));
        }
        else
        {
            // Por defecto, usa todos los Renderers
            renderers = obj.GetComponentsInChildren<Renderer>();
        }

        if (renderers.Length == 0)
            return new Bounds(obj.transform.position, Vector3.zero);

        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        return bounds;
    }


    List<GameObject> ObtenerObjetosPorTags(List<string> tags)
    {
        List<GameObject> objetosEncontrados = new List<GameObject>();

        foreach (string tag in tags)
        {
            GameObject[] objetosConTag = GameObject.FindGameObjectsWithTag(tag);
            objetosEncontrados.AddRange(objetosConTag);
        }

        return objetosEncontrados;
    }
}
