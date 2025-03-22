using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAI : MonoBehaviour
{

    public GameObject enemy;
    public GameObject player;
    public float speed = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // If player is within range
        if (Vector3.Distance(player.transform.position, enemy.transform.position) < 10)
        {
            MoveToPlayer();
        }
    }

    void MoveToPlayer()
    {
        enemy.transform.LookAt(player.transform);
        enemy.transform.position += enemy.transform.forward * speed * Time.deltaTime;
    }
}
