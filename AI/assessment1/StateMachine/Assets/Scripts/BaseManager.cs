using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseManager : MonoBehaviour
{
    //protected is basically private but inherited classes also have access to it
    [SerializeField] protected float _health = 100f;
    [SerializeField] protected float _maxHealth = 100f;
    [SerializeField] protected Text _healthText;

    protected virtual void Start()
    {
        UpdateHealthText();
        Debug.Log("I'm sorry for the code you're about to read, it's as painful to read as it was to create. ;_;");
    }

    public abstract void TakeTurn();
    /// <summary>
    /// Method that updates inherited agent's health text.
    /// </summary>
    public void UpdateHealthText()
    {
        _healthText.text = _health.ToString("0");
    }
    public void Heal(float heal)
    {
        _health = Mathf.Min(_health + heal, _maxHealth);
        UpdateHealthText();
    }
    public void DealDamage(float damage)
    {
        //if the health after damage is less than 0, then 0 is the bigger number and therefore health could never be below 0.
        _health = Mathf.Max(_health - damage, 0);
        if (_health <= 0)
        {
            Debug.Log("You died");
        }
        UpdateHealthText();
    }

    /// <summary>
    /// Method that resets health value to max for the agent that inherits the BaseManager class, and calls <see cref="UpdateHealthText"/>.
    /// </summary>
    public void Refresh()
    {
        //reset health and text
        _health = _maxHealth;
        UpdateHealthText();
    }
}
