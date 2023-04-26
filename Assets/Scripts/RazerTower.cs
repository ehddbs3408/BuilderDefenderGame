using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazerTower : MonoBehaviour
{
    [SerializeField]
    private Razer razer;
    [SerializeField]
    private float targetMaxRadius = 20f;

    private Enemy targetEnemy;
    private float lookForTargetTimer;
    private float lookForTargetTimerMax = 0.2f;

    private void Awake()
    {
        
    }

    private void Update()
    {
        if (targetEnemy != null)
        {
            razer.OnRazer(targetEnemy.transform.position);
        }
        else
        {
            razer.OffRazer();
        }

        HandleTargetting();
    }


    private void HandleTargetting()
    {
        lookForTargetTimer -= Time.deltaTime;
        if (lookForTargetTimer < 0)
        {
            lookForTargetTimer += lookForTargetTimerMax;
            LookForTargets();
        }
    }

    private void LookForTargets()
    {

        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

        foreach (Collider2D collider2D in collider2DArray)
        {
            Enemy enemy = collider2D.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (targetEnemy == null)
                {
                    targetEnemy = enemy;
                }
                else
                {
                    if (Vector3.Distance(transform.position, enemy.transform.position) <
                        Vector3.Distance(transform.position, targetEnemy.transform.position))
                    {
                        targetEnemy = enemy;
                    }
                }
            }
        }
    }
}
