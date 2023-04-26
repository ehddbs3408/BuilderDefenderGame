using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpicEnemy : Enemy
{
    public static EpicEnemy Create(Vector3 position)
    {
        Transform enemyTransform = Instantiate(GameAssets.Instance.pfEpicEnemy, position, Quaternion.identity);
        EpicEnemy enemy = enemyTransform.GetComponent<EpicEnemy>();
        return enemy;
    }

    [SerializeField]
    protected float radiusRotationSpeed = 10;
    [SerializeField]
    protected float radiusRotatiionScale = 20;

    private float radius = 0;

    protected override void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyHit);
        CinemachineShake.Instance.ShakeCamera(5f, .1f);
        ChromaticAberrationEffect.Instance.SetWeight(.5f);
    }

    protected override void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyDie);
        CinemachineShake.Instance.ShakeCamera(7f, .15f);
        ChromaticAberrationEffect.Instance.SetWeight(.5f);
        Instantiate(GameAssets.Instance.pfEnemyDieParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    protected override void HandleMovement()
    {
        if (targetTransform != null)
        {
            radius += Time.deltaTime * radiusRotationSpeed;
            Vector3 offsetVec = new Vector3(Mathf.Cos(radius),Mathf.Sin(radius)) * radiusRotatiionScale;
            Vector3 moveDir = (targetTransform.position - transform.position).normalized  + offsetVec;

            enemyRigidbody2D.velocity = moveDir * moveSpeed;
        }
        else
        {
            enemyRigidbody2D.velocity = Vector2.zero;
        }
    }
}
