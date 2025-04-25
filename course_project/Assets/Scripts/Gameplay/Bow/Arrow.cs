using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
            }
        }
        else if (collision.gameObject.CompareTag("Floor"))
        {
            // Destroy the arrow when it hits the floor
            Destroy(gameObject);
        }
    
    }
}
