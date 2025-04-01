using UnityEngine;

public class DebugBoundingBox : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
            return;

        // Inicializamos el bounds con el primero
        Bounds bounds = renderers[0].bounds;

        // Expandimos con el resto
        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
}
