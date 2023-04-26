using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 8f;
    public int damage = 10;

    protected Transform targetTransform;
    protected Rigidbody2D enemyRigidbody2D;
    protected HealthSystem healthSystem;

    protected float lookForTargetTimer;
    protected float lookForTargetTimerMax = .2f;
    

    private void Awake()
    {
        enemyRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (BuildingManager.Instance.GetHqBuilding() != null)
        {
            targetTransform = BuildingManager.Instance.GetHqBuilding().transform;
        }
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDied += HealthSystem_OnDied;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;


        lookForTargetTimer = Random.Range(0f, lookForTargetTimerMax);
    }

    protected virtual void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyHit);
        CinemachineShake.Instance.ShakeCamera(5f, .1f);
        ChromaticAberrationEffect.Instance.SetWeight(.5f);
    }

    protected virtual void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyDie);
        CinemachineShake.Instance.ShakeCamera(7f, .15f);
        ChromaticAberrationEffect.Instance.SetWeight(.5f);
        Instantiate(GameAssets.Instance.pfEnemyDieParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    protected virtual void Update()
    {
        HandleMovement();
        HandleTargetting();
    }

    protected virtual void HandleMovement()
    {
        if (targetTransform != null)
        {
            Vector3 moveDir = (targetTransform.position - transform.position).normalized;

            enemyRigidbody2D.velocity = moveDir * moveSpeed;
        }
        else
        {
            enemyRigidbody2D.velocity = Vector2.zero;
        }
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

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Building building = collision.gameObject.GetComponent<Building>();
        if (building != null)
        {
            HealthSystem healthSystem = building.GetComponent<HealthSystem>();
            healthSystem.Damage(damage);
            this.healthSystem.Damage(999);
        }
    }

    protected virtual void LookForTargets()
    {
        float targetMaxRadius = 20f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

        foreach(Collider2D collider2D in collider2DArray)
        {
            Building building = collider2D.GetComponent<Building>();
            if (building != null)
            {
                if (targetTransform == null)
                {
                    targetTransform = building.transform;
                }
                else
                {
                    if (Vector3.Distance(transform.position, building.transform.position) < 
                        Vector3.Distance(transform.position, targetTransform.position))
                    {
                        targetTransform = building.transform;
                    }
                }
            }
        }

        if (targetTransform == null)
        {
            if (BuildingManager.Instance.GetHqBuilding() != null)
            {
                targetTransform = BuildingManager.Instance.GetHqBuilding().transform;
            }
            
        }
    }
}
