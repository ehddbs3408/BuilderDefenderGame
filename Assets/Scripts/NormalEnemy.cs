using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : Enemy
{
    public static NormalEnemy Create(Vector3 position)
    {
        Transform enemyTransform = Instantiate(GameAssets.Instance.pfNormalEnemy, position, Quaternion.identity);
        NormalEnemy enemy = enemyTransform.GetComponent<NormalEnemy>();
        return enemy;
    }
}
