using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Slider healthBar;
    [SerializeField]
    private float currentHealth;

    void Start()
    {
        currentHealth = 100;
    }

    void Update()
    {
        if(healthBar == null) { return; }
        healthBar.value = currentHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }
}
