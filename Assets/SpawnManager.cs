using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] zombieePrefabs;
    [SerializeField] int number;
    // Start is called before the first frame update
    void Start()
    {
        SpawnZombiees();
    }

    private void SpawnZombiees()
    {
        for (int j = 0; j < number; j++)
        {
            for (int i = 0; i < zombieePrefabs.Length; i++)
            {
                Vector3 randomPoint = new Vector3(UnityEngine.Random.Range(8f, 9f), 0f, UnityEngine.Random.Range(-12f,-10f));
                Instantiate(zombieePrefabs[0], randomPoint, Quaternion.identity);

            }
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
