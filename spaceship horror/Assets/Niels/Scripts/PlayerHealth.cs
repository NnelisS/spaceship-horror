using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    Image healthImage;
    [SerializeField]
    private float currentHealth;

    float timer;

    void Start()
    {
        currentHealth = 100;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(currentHealth > 100) { currentHealth = 100; }
        if(timer > 3 && currentHealth < 100) { currentHealth += Time.deltaTime * 5; }

        healthImage.color = new Color(1, 1, 1, (1 - currentHealth/100) * 1f);

        if (currentHealth <= 0)
        {
            SceneManager.LoadScene("YouLose");
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        timer = 0;
    }
}
