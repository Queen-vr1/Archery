using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameMenuManager : MonoBehaviour
{
    public Transform head;
    public float spawnDistance = 3f;
    public float spawnHeight = 1.5f;
    public GameObject menu;

    void Start()
    {
        menu.SetActive(true);
        LookAtPlayer();
    }

    void Update()
    {
        LookAtPlayer();
    }

    void LookAtPlayer()
    {
        menu.transform.position = head.position + new Vector3(head.forward.x, 0, head.forward.z).normalized * spawnDistance;
        menu.transform.position = new Vector3(menu.transform.position.x, head.position.y + spawnHeight, menu.transform.position.z);
        menu.transform.LookAt(new Vector3(head.position.x, menu.transform.position.y, head.position.z));
        menu.transform.forward *= -1;
    }
}
