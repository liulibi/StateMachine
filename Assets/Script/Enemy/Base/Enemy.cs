using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour,IDamamgeable,IEnemyMoveable,ITriggerCheckable
{
    [field: SerializeField] public float MaxHealth { get; set; } = 100f;

    public float CurrentHealth { get; set; }
    public Rigidbody2D RB { get; set; }
    public bool IsFacingRight { get; set; } = true;

    public bool IsAggroed { get; set; }
    public bool IsWithinStrikingDistance { get; set; }

    public GameObject BulletPerfab;

    #region State Machine Variables
    public EnemyStateMachine stateMachine { get; set; }

    public EnemyIdleState IdleState { get; set; }

    public EnemyAttackState AttackState { get; set; }

    public EnemyChaseState ChaseState { get; set; }


    #endregion

    #region ScriptableObject Variables
    [SerializeField] private EnemyIdleSOBase EnemyIdleBase;
    [SerializeField] private EnemyChaseSOBase EnemyChaseBase;
    [SerializeField] private EnemyAttackSOBase EnemyAttackBase;

    public EnemyIdleSOBase EnemyIdleBaseInstance { get; set; }
    public EnemyChaseSOBase EnemyChaseBaseInstance { get; set; }
    public EnemyAttackSOBase EnemyAttackBaseInstance { get; set; }
    #endregion


    private void Awake()
    {
        EnemyIdleBaseInstance = Instantiate(EnemyIdleBase);
        EnemyChaseBaseInstance = Instantiate(EnemyChaseBase);
        EnemyAttackBaseInstance = Instantiate(EnemyAttackBase);

        stateMachine = new EnemyStateMachine();

        IdleState = new EnemyIdleState(this, stateMachine);
        AttackState = new EnemyAttackState(this, stateMachine);
        ChaseState = new EnemyChaseState(this, stateMachine);

    }

    private void Start()
    {
        CurrentHealth = MaxHealth;
        RB = GetComponent<Rigidbody2D>();

        EnemyIdleBaseInstance.Initialize(gameObject, this);
        EnemyChaseBaseInstance.Initialize(gameObject, this);
        EnemyAttackBaseInstance.Initialize(gameObject, this);

        stateMachine.Initialize(IdleState);

    }

    private void Update()
    {
        stateMachine.CurrentEnemyState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        if(stateMachine.CurrentEnemyState!=null) 
        stateMachine.CurrentEnemyState.PhysicsUpdate();
    }

    #region Health and Die
    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        
        if(CurrentHealth < 0)
        {
            Die();
            Destroy(gameObject);
        }
    }

    public void Die()
    {
    }

#endregion

    #region move function
    public void MoveEnemy(Vector2 velocity)
    {
        RB.velocity = velocity;
        CheckForLeftOrRightFacing(velocity);
    }

    public void CheckForLeftOrRightFacing(Vector2 velocity)
    {
        if (IsFacingRight && velocity.x < .0f)
        {
            Vector3 rotator = new Vector3(transform.rotation.x,180,transform.rotation.z);
            //用于创建一个旋转的四元数（Quaternion）。
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;
        }
        else if (!IsFacingRight && velocity.x > .0f)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0, transform.rotation.z);
            //用于创建一个旋转的四元数（Quaternion）。
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;
        }
    }
    #endregion

    #region Animation Triggers
    private void AnimationTriggerEvent(AnimationTriggerType animationTriggerType)
    {
        stateMachine.CurrentEnemyState.AnimationTriggerEvent(animationTriggerType);
    }

    public enum AnimationTriggerType 
    {
        EnemyDamaged,
        PlayFootstepSound
    }

    #endregion

    #region Distance Checks
    public void SetAggroStatus(bool isAggroed)
    {
        IsAggroed = isAggroed;
    }

    public void SetStrikingDistanceBool(bool isWithinStrikingDistance)
    {
        IsWithinStrikingDistance = isWithinStrikingDistance; 
    }
    #endregion
}
