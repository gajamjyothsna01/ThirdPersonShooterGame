using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] int startingHealth;
    [SerializeField] int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DamageMethod(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log(currentHealth);
        if(currentHealth <= 0)
        {
            Death();
            Debug.Log("Health : " + currentHealth);
        }
    }

    private void Death()
    {
        gameObject.SetActive(false);
    }
}
