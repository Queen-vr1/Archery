using UnityEngine;

public class DebugBoundingBox : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
            return;

        string tag = gameObject.tag;

        if (tag == "Aerostatic_Ballon")
        {
            // Modo completo: bounding box general (amarillo)
            Bounds bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
        else if (tag == "Balloon")
        {
            // Modo individual: solo bounds rojos por componente
            Gizmos.color = Color.red;
            foreach (var r in renderers)
            {
                Gizmos.DrawWireCube(r.bounds.center, r.bounds.size);
            }
        }
    }
}
