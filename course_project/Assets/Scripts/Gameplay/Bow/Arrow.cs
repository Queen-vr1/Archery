using UnityEngine;

public class Arrow : MonoBehaviour
{

    // For the explosive arrow
    public GameObject explosionEffect;
    public float explosionRadius = 200f;

    // Start is called before the first frame update
    void Start()
    {
       
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


    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Arrow colided with: " + collision.gameObject.name);

        if (collision.gameObject.name == "balloon_up")
        {
            Balloon balloon = collision.gameObject.transform.parent.GetComponent<Balloon>();
            if (balloon != null)
            {
                Debug.Log("Es balloon");
                balloon.TakeDamage(1); // Ver cuanto daño hace la flecha
				if (balloon.IsDestroyed())
				{
					balloon.GetRewards();
					Debug.Log("Balloon destroyed");
				}
			}

            //Explode(collision.contacts[0].point);
            

        }
        else if (collision.gameObject.CompareTag("Floor"))
        {
            // Destroy the arrow when it hits the floor
            Destroy(gameObject);
        }
    
    }
}
