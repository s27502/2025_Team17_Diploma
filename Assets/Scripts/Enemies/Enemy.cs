using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Managers;
using Player;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum EnemyState
{
    Idle,
    Attacking,
}

public class Enemy : MonoBehaviour
{
    [SerializeField] protected Animator _animator;
    [SerializeField] protected EnemyProjectileFactory projectileFactory;
    [SerializeField] private float startDelay = 1f;
    [SerializeField] private AudioClip enemyDie;

    [SerializeField] private float minCrossMoveDuration = 0.5f;
    [SerializeField] private float maxCrossMoveDuration = 1.5f;
    private GameObject _spriteObject;
    private Vector2 currentCrossDirection;
    private Vector2 newCrossDirection;

    private float _crossCounter = 0f;
    
    public System.Action<Enemy> OnDeath;

    protected EnemyStats EnemyStats;
    protected EnemyState State;
    protected GameObject _player;
    protected Rigidbody2D _rb;
    private bool canReact = false;

    protected NavMeshAgent _agent;
    
    
    public bool FacingRight { get; private set; } = true;
    
    protected virtual void Start()
    {
        EnemyStats = GetComponent<EnemyStats>();
        _rb = GetComponent<Rigidbody2D>();
        StartCoroutine(StartAfterDelay());
        _spriteObject = transform.Find("Sprite").gameObject;
        SetUpAgent();
    }

    

    protected virtual void FixedUpdate()
    {
        switch (State)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Attacking:
                Attack();
                break;
        }
        
    }

    private void SetUpAgent()
    {
        _agent = GetComponent<NavMeshAgent>();
        if (_agent)
        {
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
            _agent.speed = EnemyStats.GetMovementSpeed();
        }

    }
    
    private IEnumerator StartAfterDelay()
    {
        yield return new WaitForSeconds(startDelay);
        OnStartAfterDelay();
    }

    protected virtual void OnStartAfterDelay()
    {
        canReact = true;
        
        if (_player != null)
            StartAttacking();
    }

    public bool CanReact() => canReact;

    protected virtual void Attack()
    {
        //MoveToPlayer();
    }

    protected virtual void Idle()
    {
        //throw new NotImplementedException();
    }

    protected void NavMoveTo(Transform target)
    {
        _agent.SetDestination(target.position);
        
        if (_rb == null) return;
    
        Vector2 currentPos = _rb.position;
        Vector2 dir = ((Vector2)target.position - currentPos).normalized;
        
        FlipTo(dir.x);
    }

    protected void MoveTo(Vector2 targetPos)
    {
        if (_rb == null) return;
    
        Vector2 currentPos = _rb.position;
        Vector2 dir = (targetPos - currentPos).normalized;
        
        Vector2 newPos = currentPos + dir * EnemyStats.GetMovementSpeed() * Time.fixedDeltaTime;
        _rb.MovePosition(newPos);
        
        FlipTo(dir.x);
    }
    
    public void PushBack(Vector2 sourcePosition, float force)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null) return;

        Vector2 direction = ((Vector2)transform.position - sourcePosition).normalized;
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }
    
    public void PushBackReflect(Collision2D collision, float force)
    {
        if (!TryGetComponent(out Rigidbody2D rb)) return;
        if (collision.contactCount == 0) return;

        Vector2 incoming = rb.velocity;
        
        if (incoming.sqrMagnitude < 0.001f)
            incoming = transform.position - collision.transform.position;

        Vector2 normal = collision.contacts[0].normal;
        Vector2 reflectDir = Vector2.Reflect(incoming, normal).normalized;

        rb.AddForce(reflectDir * force, ForceMode2D.Impulse);

        StartCoroutine(ResetForceCoroutine());
    }

    private IEnumerator ResetForceCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        _rb.velocity = Vector2.zero;
    }


    protected void MoveInDirection(Vector2 direction)
    {
        if (_rb == null) return;
        
        Vector2 dir = direction.normalized;
        
        Vector2 currentPos = _rb.position;
        Vector2 newPos = currentPos + dir * EnemyStats.GetMovementSpeed() * Time.fixedDeltaTime;
        
        _rb.MovePosition(newPos);
        
        FlipTo(dir.x);
    }

    protected void CrossMove()
    {
        if (_crossCounter <= 0f)
        {
            _crossCounter = Random.Range(minCrossMoveDuration, maxCrossMoveDuration);
            newCrossDirection = RollCrossDirection();
            if (newCrossDirection == currentCrossDirection)
            {
                currentCrossDirection = -newCrossDirection;
            }
            else
            {
                currentCrossDirection = newCrossDirection;
            }
        }
        else
        {
            _crossCounter -= Time.fixedDeltaTime;
            MoveInDirection(currentCrossDirection);
        }
        
    }

    private Vector2 RollCrossDirection()
    {
        int dirNumber = Random.Range(0, 4);

        switch (dirNumber)
        {
            case 0:
                return Vector2.up;
            case 1:
                return Vector2.down;
            case 2:
                return Vector2.left;
            case 3:
                return Vector2.right;
        }

        return Vector2.up;
    }


    protected void MoveToPlayer()
    {
        if (_player == null) return;
        //MoveTo(_player.transform.position);
        NavMoveTo(_player.transform);
    }
    

    public void TakeDamage(int dmg)
    {
        EnemyStats.ModifyHp(-dmg);
        Debug.Log(EnemyStats.GetHp());
        if (EnemyStats.GetHp() <= 0)
        {
            Die();
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        other.gameObject
            .GetComponent<PlayerStats>()
            .ModifyHp(-EnemyStats.GetDmg());
    }


    public virtual void Die()
    {
        AudioManager.Instance.PlaySfx(enemyDie);
        OnDeath?.Invoke(this);
        Destroy(gameObject);
    }

    public void StartAttacking()
    {
        State = EnemyState.Attacking;
    }

    public void StopAttacking()
    {
        State = EnemyState.Idle;
    }

    public void SetPlayer(GameObject player)
    {
        _player = player;
    }

    private void DealDamage()
    {
        _player.GetComponent<PlayerStats>().ModifyHp(-EnemyStats.GetDmg());
    }
    
    public void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public virtual void FlipTo(float dirX)
    {
        if (Mathf.Abs(dirX) < 0.01f) return;
        Vector3 scale = _spriteObject.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * -Mathf.Sign(dirX);
        _spriteObject.transform.localScale = scale;
    }
    
    protected Vector2 RotateProjectile(Vector2 v, float angle)
    {
        return Quaternion.Euler(0, 0, angle) * v;
    }

}
