using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, Health
{
    private bool _isDead = false;
    public int _health = 100;
    private static PlayerHealth _instance;

    public PlayerHealth()
    {
        IsDying = false;
    }

    public static PlayerHealth Instance
    {
        get
        {
            if (_instance == null) throw new NullReferenceException("Instance is null");
            return _instance;
        }
    }

    public int MaxHealth { get; set; }

    public bool IsDying { get; set; }

    public int Health
    {
        get { return _health; }
        set
        {
            if (IsDying) return;
            if (value < _health)
            {
                if (value <= 0)
                {
                    value = 0;
                    Death();
                }
                else
                {
                    AnimateHit();
                }
            }
            else if (value > MaxHealth)
            {
                value = MaxHealth;
            }
            _health = value;

            var healthTextBox = GameObject.Find("health_1").GetComponent<Text>();
            healthTextBox.text = _health.ToString() + '%';
        }
    }

    void Start()
    {
        MaxHealth = Health;
        _instance = this;
    }

    void AnimateHit()
    {
        //Animator.SetTrigger("damage");
    }

    void Death()
    {
        IsDying = true;
        //Animator.SetTrigger("death");
    }
}
