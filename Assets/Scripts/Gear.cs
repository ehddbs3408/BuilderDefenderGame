using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    private CircleCollider2D circleCollider2D;
    private void Awake()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
    }
    private void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            int damage = 1;
            enemy.GetComponent<HealthSystem>().Damage(damage);
        }
    }
}
