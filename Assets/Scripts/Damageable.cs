using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    Animator animator;
    [SerializeField] private int _maxHealth = 100;
    [SerializeField]
    private bool _isAlive = true;
    [SerializeField]
    private bool isInvincible;
    [SerializeField]
    private float timeSinceHit = 0;
    [SerializeField]
    private float invincibilityTimer = 1f; //play around with this time to make sure you dont get hit multiple times
    public bool Invincible { get; set; }

    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
        }
    }

    [SerializeField] private int _currentHealth;

    public int CurrentHealth
    {
        get
        {
            return _currentHealth;
        }
        set
        {
            _currentHealth = value;
            if (_currentHealth <= 0)
            {
                IsAlive = false;
            }
        }
    }

    

    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
            //animator.SetBool(AnimationStrings.IsAlive, value);
        }
    }

    /*public bool LockVelocity { get
        {
            //return animator.GetBool(AnimationStrings.lockVelocity);
        } set
        {
            //animator.SetBool(AnimationStrings.lockVelocity, value);
        }
    }*/


    private void Awake()
    {
        _currentHealth = _maxHealth;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isInvincible)
        {
            if (timeSinceHit > invincibilityTimer)
            {
                isInvincible = false;
                timeSinceHit = 0;
            }
            timeSinceHit += Time.deltaTime;
        }
    }

    public void Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && !isInvincible)
        {
            CurrentHealth -= damage;
            isInvincible = true;
            //animator.SetTrigger(AnimationStrings.hitTrigger);
            //LockVelocity = true;
            damageableHit?.Invoke(damage, knockback); //unity event invoked to have the player script implement its own version of how to deal with the knockback check the subscriber list of who invokes this event if its null it will error since it will be an erroneous event.
                                                      //? checks if null
                                                      //using this to pass back damage and knockback information to respective objects instead of dealing with velocity here 
            
        }
    }
}