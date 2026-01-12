using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Managers;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EnemyState
{
    Idle,
    Attacking,
}

public class Enemy : MonoBehaviour
{
    [SerializeField] protected EnemyProjectileFactory projectileFactory;
    [SerializeField] private float startDelay = 1f;
    [SerializeField] private AudioClip enemyDie;

    [SerializeField] private float minCrossMoveDuration = 0.5f;
    [SerializeField] private float maxCrossMoveDuration = 1.5f;
    private Vector2 currentCrossDirection;
    private Vector2 newCrossDirection;

    private float _crossCounter = 0f;
    
    public System.Action<Enemy> OnDeath;

    protected EnemyStats EnemyStats;
    protected EnemyState State;
    protected GameObject _player;
    protected Rigidbody2D _rb;
    private bool canReact = false; 
    
    
    public bool FacingRight { get; private set; } = true;
    
    protected virtual void Start()
    {
        EnemyStats = GetComponent<EnemyStats>();
        _rb = GetComponent<Rigidbody2D>();
        StartCoroutine(StartAfterDelay());
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
        //throw new NotImplementedException();
    }

    protected virtual void Idle()
    {
        //throw new NotImplementedException();
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
        MoveTo(_player.transform.position);
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
        float threshold = 0.01f;
        if (Mathf.Abs(dirX) < threshold) return;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * -Mathf.Sign(dirX);
        transform.localScale = scale;

        FacingRight = dirX > 0;
    }

}
