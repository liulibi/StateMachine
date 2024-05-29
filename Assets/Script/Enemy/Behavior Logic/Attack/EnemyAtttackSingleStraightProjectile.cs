using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Attack-Straight-Single Projectile",menuName ="Enemy Logic/Attack Logic/Straight Single Projectile")]
public class EnemyAtttackSingleStraightProjectile : EnemyAttackSOBase
{
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private float _timeBetweenShots = 2f;
    [SerializeField] private float _timeTillExit = 3f;
    [SerializeField] private float _distanceToCountExit = 3f;

    private float _timer;
    private float _exitTimer;

    private float bulletSpeed = 10f;

    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        enemy.MoveEnemy(Vector2.zero);

        if (_timer > _timeBetweenShots)
        {
            _timer = 0;

            Vector2 dir = (playerTransform.position - enemy.transform.position).normalized;

            GameObject bullet = GameObject.Instantiate(BulletPrefab, enemy.transform.position, Quaternion.identity);
            Rigidbody2D bullerRB = bullet.GetComponent<Rigidbody2D>();
            bullerRB.velocity = dir * bulletSpeed;
        }

        if(Vector2.Distance(playerTransform.position, enemy.transform.position) > _distanceToCountExit)
        {
            _exitTimer += Time.deltaTime;

            if(_exitTimer>_timeTillExit)
            {
                enemy.stateMachine.ChangeState(enemy.ChaseState) ;
            }
        }

        else
        {
            _exitTimer = 0;
        }

        _timer += Time.deltaTime;
    }

    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();
    }

    public override void Initialize(GameObject gameObject, Enemy enemy)
    {
        base.Initialize(gameObject, enemy);
    }

    public override void ResetValues()
    {
        base.ResetValues();
    }
}
