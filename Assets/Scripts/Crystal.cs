using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crystal : MonoBehaviour
{
    public Player player;
    public int money;

    public int health;
    public int maxHealth;

    public Slider healthSlider;
    private void Start()
    {
        health = maxHealth;
        healthSlider.maxValue = maxHealth;
    }
    public void AddMoney(int amount)
    {
        money += amount;
    }
    private void Update()
    {
        healthSlider.value = health;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
