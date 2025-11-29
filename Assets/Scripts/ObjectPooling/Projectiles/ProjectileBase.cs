using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour, IPoolableObject
{
    [SerializeField] protected float speed;
    protected IObjectPool _pool;
    protected Rigidbody2D _rb;
    protected Vector2 _dir;
    [SerializeField] protected int damage = 1;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0;
    }

    public void SetPool(IObjectPool pool) => _pool = pool;

    public virtual void Spawn(Vector2 position, Vector2 direction)
    {
        transform.position = position;
        gameObject.SetActive(true);

        _dir = direction.normalized;
        _rb.velocity = _dir * speed;
    }

    public virtual void Despawn()
    {
        _rb.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }
}