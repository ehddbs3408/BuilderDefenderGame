using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    public static BossEnemy Create(Vector3 position)
    {
        Transform enemyTransform = Instantiate(GameAssets.Instance.pfBossEnemy, position, Quaternion.identity);
        BossEnemy enemy = enemyTransform.GetComponent<BossEnemy>();
        return enemy;
    }

    [SerializeField]
    protected Transform[] spawnList;
    [SerializeField]
    protected float spawnTime;

    protected float time;

    protected override void Update()
    {
        base.Update();
        BossSkill();
    }
    public void BossSkill()
    {
        time += Time.deltaTime;
        if (time > spawnTime)
        {
            StartCoroutine(TimeToSpawnEnemy(5, 0.5f));
            time = 0;
        }
    }

    public IEnumerator TimeToSpawnEnemy(int spawnCnt,float spawnTime)
    {
        for(int i = 0;i<spawnCnt;i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnTime);
        }
    }
    public void SpawnEnemy()
    {
        foreach(Transform t in spawnList)
        {
            NormalEnemy enemy =NormalEnemy.Create(t.position);
            enemy.moveSpeed = 15;
        }
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        Building building = collision.gameObject.GetComponent<Building>();
        if (building != null)
        {
            BuildingTypeHolder type = building.GetComponent<BuildingTypeHolder>();
            HealthSystem healthSystem = building.GetComponent<HealthSystem>();
            if (type.buildingType.nameString == "HQ")
                healthSystem.Damage(999);
        }
    }

    protected override void LookForTargets()
    {
        if (BuildingManager.Instance.GetHqBuilding() != null)
        {
            targetTransform = BuildingManager.Instance.GetHqBuilding().transform;
        }
    }
}
