﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------------
    // Serialized fields
    //----------------------------------------------------------------------------------------------------------------
    [SerializeField] private  GameObject _listOfWaypointObjects;
    [SerializeField] private float _startSpeed = 10f;
    [SerializeField] private float _startHealth = 100f;
    [SerializeField] private int _worth = 50;
    [SerializeField] private Image _healthBar;
    
    //-----------------------------------------------------------------------------------------------------------------
    // Non-serializable fields
    //-----------------------------------------------------------------------------------------------------------------
    public GameObject ListOfWaypointObjects
    {
        get => _listOfWaypointObjects;
        set => _listOfWaypointObjects = value;
    }
    private float _speed;
    private float _health;
    private Transform _target;
    private int _wavePointIndex = 0;
    private CreateWaypointsList _waypointList;
    private Transform _nextTarget;

    //----------------------------------------------------------------------------------------------------------------
    // Unity events
    //----------------------------------------------------------------------------------------------------------------
    private void Start()
    {
        _health = _startHealth;
        _speed = _startSpeed;
        _waypointList = _listOfWaypointObjects.GetComponent<CreateWaypointsList>();
        _target = _waypointList.listOfWaypointObjects[0];
        _nextTarget = _waypointList.listOfWaypointObjects[1];
    }
    private void Update()
    {
                Vector3 direction = _target.position - transform.position;
                transform.Translate(direction.normalized * (_speed * Time.deltaTime), Space.World);
                // normalized to use only the "speed" to control the speed.

                if (Vector3.Distance(transform.position, _target.position) <= 0.4f)
                {
                    try
                    {
                        if (_nextTarget.GetComponent<NodeInterations>().turret != null)
                        {
                            return;
                        }

                        GetNextWaypoint();
                    }
                    catch (Exception e)
                    {
                        GetNextWaypoint();
                    }
                }
    }

    //----------------------------------------------------------------------------------------------------------------
    // Private methods
    //----------------------------------------------------------------------------------------------------------------
    private void GetNextWaypoint()
    {
        if (_wavePointIndex > _waypointList.listOfWaypointObjects.Length)
        {
            PlayerStats.Lives--;
            Debug.Log(PlayerStats.Lives);
            Destroy(gameObject);
        }
        _wavePointIndex++;
        _nextTarget = _waypointList.listOfWaypointObjects[_wavePointIndex+1];
       
        _target =  _waypointList.listOfWaypointObjects[_wavePointIndex];
    }

    private void Die()
    {
        Destroy(gameObject);
    }
    
    //----------------------------------------------------------------------------------------------------------------
    // Public methods
    //----------------------------------------------------------------------------------------------------------------
    public void TakeDamage(float amount)
    {
        
        _health -= amount;

        _healthBar.fillAmount = _health / _startHealth; // No matter how much life the enemy has, it always shows correctly
        
        if (_health <= 0)
        {
            Die();
        }
    }
}
