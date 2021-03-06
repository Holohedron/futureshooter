﻿using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int advanceTimeMin;
    public int advanceTimeMax;
    public int flankTimeMin;
    public int flankTimeMax;
    public int stopTimeMin;
    public int stopTimeMax;
    public int hitTime;
    public float advanceSpeed;
    public float flankSpeed;

    public int health;
    public bool dbugFreeze = false;
    
    private GameObject player;
    private IEnemyState state;

    public delegate void EnemyHit();
    public event EnemyHit OnEnemyHit;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        state = new StopState();
    }

    private void Update ()
    {
        if (player.GetComponent<PlayerCharacter>().dead)
        {
            state = new StopState();
            return;
        }

        var newState = state.HandleTransition(this);
        if (newState != null && !dbugFreeze)
        {
            state.OnExit(this);
            state = newState;
            state.OnEnter(this);
        }
        state.HandleUpdate(this);
	}

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Projectile"))
        {
            HandleProjectileHit(collider);
        }
        if (collider.CompareTag("Hitbox"))
        {
            HandleHitboxCollision(collider);
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        PlayerCharacter playerChar = player.GetComponent<PlayerCharacter>();

        if (hit.gameObject.CompareTag("Player"))
        {
            player.GetComponent<PlayerCharacter>().OnPlayerDamaged();
        }
    }


    private void HandleProjectileHit(Collider projectile)
    {
        Destroy(projectile.gameObject);
        --health;
        HandleHit();
    }

    private void HandleHitboxCollision(Collider collider)
    {
        var hitboxDamage = collider.gameObject.GetComponent<Hitbox>().damage;
        health -= hitboxDamage;
        HandleHit();
    }

    private void HandleHit()
    {
        OnEnemyHit();

        var audioSource = GetComponent<AudioSource>();
        audioSource.Play();

        if (health <= 0)
        {
            GetComponent<MeshRenderer>().enabled = false;
            var renderers = GetComponentsInChildren<MeshRenderer>();
            foreach (var renderer in renderers)
                renderer.enabled = false;
            GetComponent<CharacterController>().enabled = false;
            Destroy(gameObject, audioSource.clip.length);
        }
    }
}
