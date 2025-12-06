using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Player;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Attacking,
}

public class Enemy : MonoBehaviour
{
    [SerializeField] protected EnemyProjectileFactory projectileFactory;
    protected EnemyStats EnemyStats;
    protected EnemyState State;
    protected GameObject _player;
    protected Rigidbody2D _rb;
    public bool FacingRight { get; private set; } = true;
    
    protected virtual void Start()
    {
        EnemyStats = gameObject.GetComponent<EnemyStats>();
        State = EnemyState.Idle;
        _rb = GetComponent<Rigidbody2D>();
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


    private void Die()
    {
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

    public void FlipTo(float dirX)
    {
        if (dirX == 0) return;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * -Mathf.Sign(dirX);
        transform.localScale = scale;

        FacingRight = dirX > 0;
    }
}
