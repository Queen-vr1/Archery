using System;
using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour
{

    // For the explosive arrow
    public GameObject explosionEffect;
    public float explosionRadius = 5f;

    public AudioSource failSound;
    public bool hasHit = false;

    string arrowType = "normal"; // normal, explosive, ice, fire, etc.

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance != null)
        {
            string activeArrow = "";
            bool ok = GameManager.Instance.GetActiveArrow(ref activeArrow);
            if (ok)
            {
                Debug.Log("Active arrow: " + activeArrow);
                arrowType = activeArrow;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }


    void Explode(Vector3 position)
    {
        // Instanciar FX de explosión
        if (explosionEffect != null)
        {
            GameObject fx = Instantiate(explosionEffect, position, Quaternion.identity);
            // fx.transform.localScale = new Vector3(1, 1, 1);
            Destroy(fx, 1f);
        }

        // Encontrar todos los colliders dentro del radio
        Collider[] colliders = Physics.OverlapSphere(position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Debug.Log("Arrow exploded near: " + nearbyObject.name);

            if (nearbyObject.name == "balloon_up")
            {
                Balloon balloon = nearbyObject.transform.parent.GetComponent<Balloon>();
                if (balloon != null)
                {
                    balloon.TakeDamage(1);
                    if (balloon.IsDestroyed())
                    {
                        balloon.GetRewards();
                        Debug.Log("Balloon destroyed (area explosion)");
                    }
                }
            }
        }

        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Arrow collided with: " + other.gameObject.name);

        if (other.gameObject.name == "balloon_up")
        {
            Balloon balloon = other.gameObject.transform.parent.GetComponent<Balloon>();
            if (balloon != null)
            {
                Debug.Log("Es balloon");
                balloon.TakeDamage(1);
                if (balloon.IsDestroyed())
                {
                    balloon.GetRewards();
                    Debug.Log("Balloon destroyed");
                }
            }

            if (arrowType == "Arrow_Explosion")
            {
                if (!hasHit)
                {
                    Explode(other.gameObject.transform.position);
                    hasHit = true;
                }
                Destroy(gameObject);
            }
            else if (arrowType == "Arrow_Steel")
            {
                if (!hasHit)
                {
                    StartCoroutine("DestroyAfter20");
                    hasHit = true;
                }
            }
            else Destroy(gameObject);

        }
        else if (other.gameObject.CompareTag("Floor"))
        {
            // Destroy the arrow when it hits the floor
            if (!hasHit)
            {
                failSound.Play();
                StartCoroutine("DestroyAfter10");
                hasHit = true;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {

        // si el tag del ovjeto de la colision es
        if (collision.gameObject.CompareTag("Floor"))
        {
            // Destroy the arrow when it hits the floor
            if (!hasHit)
            {
                failSound.Play();
                StartCoroutine("DestroyAfter10");
                hasHit = true;
            }

        }
    }

    IEnumerator DestroyAfter10()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }

    IEnumerator DestroyAfter20()
    {
        yield return new WaitForSeconds(20);
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        GameManager.Instance.ArrowUp();
        Debug.Log("Arrow destroyed");
        // Aqui puedes agregar cualquier otro comportamiento que desees al destruir la flecha
    }


}
