using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Alligator : Enemy
{
    [SerializeField] private float _windUpTime = 2f;
    [SerializeField] private float _miniChargeCooldown = 0.5f;
    [SerializeField] private float _chargingSpeedMult = 10;

    private float _windingUpCounter;
    private bool _charging = false;
    private int _chargesNumber = 0;
    private Vector2 _chargeDirection;
    private float _originalSpeed;
    private float _miniChargeCounter = 0f;
    
    private bool _canDetectCollision = true;

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
            return;
        }
        
        if (_miniChargeCounter > 0)
        {
            _miniChargeCounter -= Time.fixedDeltaTime;
            return;
        }
        
        if (_chargesNumber > 0)
        {
            MoveInDirection(_chargeDirection);
        }
        
        if (_chargesNumber <= 0)
        {
            _charging = false;
            EnemyStats.SetMovementSpeed(_originalSpeed);
            _windingUpCounter = _windUpTime;
        }
    }

    private void StartCharge()
    {
        _charging = true;
        EnemyStats.SetMovementSpeed(_originalSpeed * _chargingSpeedMult);
        _chargesNumber = RollChargesNumber();

        _chargeDirection = (_player.transform.position - transform.position).normalized;
        FlipTo(_chargeDirection.x);

        _miniChargeCounter = 0f;
        
        StartCoroutine(EnableCollisionNextFrame());
    }

    private IEnumerator EnableCollisionNextFrame()
    {
        _canDetectCollision = false;
        yield return new WaitForFixedUpdate();
        _canDetectCollision = true;
    }

    private int RollChargesNumber()
    {
        return Random.Range(1, 4);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!_canDetectCollision) return;

        if (!_charging) return;
        if (!other.gameObject.CompareTag("Obstacle") && !other.gameObject.CompareTag("Player")) return;
        
        _chargesNumber--;
        _chargeDirection = (_player.transform.position - transform.position).normalized;
        _miniChargeCounter = _miniChargeCooldown;
    }
}
