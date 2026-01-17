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
        transform.SetParent(ServiceLocator.Instance.GetService<FloorManager>().GetCurrentRoom().transform);
    }

    public void SetPool(IObjectPool pool) => _pool = pool;
    public void Reparent(GameObject obj)
    {
        transform.SetParent(obj.transform);
    }

    public virtual void Spawn(Vector2 position, Vector2 direction)
    {
        transform.position = position;
        gameObject.SetActive(true);

        _dir = direction.normalized;

        float angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

        _rb.velocity = _dir * speed;
    }



    public virtual void Despawn()
    {
        _rb.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }
}