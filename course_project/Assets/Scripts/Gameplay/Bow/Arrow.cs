using System;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    // For the explosive arrow
    public GameObject explosionEffect;
    public float explosionRadius = 200f;

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
            fx.transform.localScale = new Vector3(30, 30, 30);
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
        if (arrowType == "Arrow_Steel"){
            Debug.Log("Arrow triggered with: " + other.gameObject.name);

            if (other.gameObject.name == "balloon_up")
            {
                Balloon balloon = other.transform.parent.GetComponent<Balloon>();
                if (balloon != null)
                {
                    balloon.TakeDamage(1);
                    if (balloon.IsDestroyed())
                    {
                        balloon.GetRewards();
                        Debug.Log("Balloon destroyed (trigger)");
                    }
                }
            }
            else if (other.CompareTag("Floor"))
            {
                // Destroy the arrow when it hits the floor
                Destroy(gameObject);
            }
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (arrowType != "Arrow_Steel"){
            Debug.Log("Arrow colided with: " + collision.gameObject.name);

            if (collision.gameObject.name == "balloon_up")
            {
                Balloon balloon = collision.gameObject.transform.parent.GetComponent<Balloon>();
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
                    Explode(collision.contacts[0].point);
                }
                
            }
            else if (collision.gameObject.CompareTag("Floor"))
            {
                // Destroy the arrow when it hits the floor
                Destroy(gameObject);
            }
        }
    }
}
