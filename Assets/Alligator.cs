using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Alligator : Enemy
{
    [SerializeField] private float _windUpTime = 2f;
    
    private float _windingUpCounter;
    private bool _charging = false;
    private int _chargesNumber = 0;
    private Vector2 _chargeDirection;
    private float _originalSpeed;

    protected override void Start()
    {
        base.Start();
        _windingUpCounter = _windUpTime;
        _originalSpeed = EnemyStats.GetMovementSpeed();
    }

    protected override void Attack()
    {
        PerformChargeAttacks();
    }

    private void PerformChargeAttacks()
    {
        if (!_charging)
        {
            _windingUpCounter -= Time.fixedDeltaTime;
            if (_windingUpCounter <= 0)
            {
                StartCharge();
            }
        }

        if (_charging && _chargesNumber > 0)
        {
            MoveInDirection(_chargeDirection);
        }

        if (_charging && _chargesNumber <= 0)
        {
            _charging = false;
            EnemyStats.SetMovementSpeed(_originalSpeed);
            _windingUpCounter = _windUpTime;
        }
    }

    private void StartCharge()
    {
        _charging = true;
        EnemyStats.SetMovementSpeed(_originalSpeed * 10);
        _chargesNumber = RollChargesNumber();
        _chargeDirection = (_player.transform.position - transform.position).normalized;
        FlipTo(_chargeDirection.x);
    }

    private int RollChargesNumber()
    {
        return Random.Range(1, 4);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Obstacle") && _charging)
        {
            _chargesNumber--;
            _chargeDirection = (_player.transform.position - transform.position).normalized;
        }
    }
}
