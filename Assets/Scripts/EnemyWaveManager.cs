using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemyWaveManager : MonoBehaviour
{
    public static EnemyWaveManager Instance { get; private set; }

    private enum State
    {
        WaitingToSpawnNextWave,
        SpawningWave,
    }

    public event EventHandler OnWaveNumberChanged;

    [SerializeField] private List<Transform> spawnPositionTransformList;
    [SerializeField] private Transform nextWaveSpawnPositionTransform;
    private State state;
    private int waveNumber;
    private float nextWaveSpawnTimer;
    private float nextEnemySpawnTimer;
    private int remainingEnemySpawnAmount;
    private Vector3 spawnPosition;

    private bool isBoss = false;


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        state = State.WaitingToSpawnNextWave;
        spawnPosition = spawnPositionTransformList[UnityEngine.Random.Range(0, spawnPositionTransformList.Count)].position;
        nextWaveSpawnPositionTransform.position = spawnPosition;
        nextWaveSpawnTimer = 3f; 
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToSpawnNextWave:
                nextWaveSpawnTimer -= Time.deltaTime;
                if (nextWaveSpawnTimer < 0f)
                {
                    SpawnWave();
                    state = State.SpawningWave;
                }
                break;
            case State.SpawningWave:
                if (remainingEnemySpawnAmount > 0)
                {
                    nextEnemySpawnTimer -= Time.deltaTime;
                    if (nextEnemySpawnTimer < 0f)
                    {
                        nextEnemySpawnTimer = UnityEngine.Random.Range(0f, .2f);
                        SpawnEnemy();
                        remainingEnemySpawnAmount--;
                        if (remainingEnemySpawnAmount <= 0)
                        {
                            isBoss = false;
                            state = State.WaitingToSpawnNextWave;
                            spawnPosition = spawnPositionTransformList[UnityEngine.Random.Range(0, spawnPositionTransformList.Count)].position;
                            nextWaveSpawnPositionTransform.position = spawnPosition;
                            nextWaveSpawnTimer = 10f;
                        }
                    }
                }
                break;
        }
    }
    private void SpawnEnemy()
    {
        if (waveNumber < 10)
            NormalEnemy.Create(spawnPosition + UtilClass.GetRandomDir() * UnityEngine.Random.Range(0f, 10f));
        else if (waveNumber < 20)
            EpicEnemy.Create(spawnPosition + UtilClass.GetRandomDir() * UnityEngine.Random.Range(0f, 10f));
        else
        {
            if(isBoss == false)
            {
                BossEnemy.Create(spawnPosition + UtilClass.GetRandomDir() * UnityEngine.Random.Range(0f, 10f));
                isBoss = true;
            }
            else
                EpicEnemy.Create(spawnPosition + UtilClass.GetRandomDir() * UnityEngine.Random.Range(0f, 10f));
        }
             
        

    }

    private void SpawnWave()
    {
        remainingEnemySpawnAmount = 5+3* waveNumber;
        waveNumber+=1;
        OnWaveNumberChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetWaveNumber()
    {
        return waveNumber;
    }

    public float GetNextWaveSpawnTimer()
    {
        return nextWaveSpawnTimer;
    }

    public Vector3 GetSpawnPosition()
    {
        return spawnPosition;
    }
}
