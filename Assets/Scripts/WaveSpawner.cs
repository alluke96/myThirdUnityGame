﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------------
    // Serialized fields
    //----------------------------------------------------------------------------------------------------------------
    [Header("Settings")]
    [SerializeField] private float _timeBetweenWaves = 5f;
    [Header("Dependencies")] 
    [SerializeField] private Transform[] _enemyPrefab;
    [SerializeField] private Transform[] _spawnPoint;
    [SerializeField] private Text _waveCountdownText;

    //----------------------------------------------------------------------------------------------------------------
    // Non-serialized fields
    //----------------------------------------------------------------------------------------------------------------
    private float _countdown = 2f; // value is set for the first wave
    private int _waveIndex = 0;
    
    //----------------------------------------------------------------------------------------------------------------
    // Unity events
    //----------------------------------------------------------------------------------------------------------------
    private void Update()
    {
        if (_countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            _countdown = _timeBetweenWaves;
        }

        _countdown -= Time.deltaTime;
        _countdown = Mathf.Clamp(_countdown,0f, Mathf.Infinity);

        _waveCountdownText.text = Mathf.Round(_countdown).ToString();
    }

    //----------------------------------------------------------------------------------------------------------------
    // Coroutines
    //----------------------------------------------------------------------------------------------------------------
    private IEnumerator SpawnWave()
    {
        Debug.Log("Wave Incoming!");
        
        _waveIndex++;
        PlayerStats.Rounds++; // UI
        
        for (int i = 0; i < _waveIndex; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }
    }

    //----------------------------------------------------------------------------------------------------------------
    // Private methods
    //----------------------------------------------------------------------------------------------------------------
    private void SpawnEnemy()
    {
        int randomRow = Random.Range(0, _spawnPoint.Length);
        int randomEnemy = Random.Range(0, _enemyPrefab.Length);
        Transform enemy = Instantiate(_enemyPrefab[randomEnemy], _spawnPoint[randomRow].position, Quaternion.Euler(90,0,0), _spawnPoint[randomRow].parent.parent);
        Transform waypointsList = _spawnPoint[randomRow].transform.parent.gameObject.transform.parent.transform;
        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        enemyController.ListOfWaypointObjects = waypointsList.gameObject;
    }
}
