using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour, IPoolableObject
{
    [SerializeField] private float lifetime = .5f;
    [SerializeField] private TextMeshPro text;

    private Vector3 moveDir;
    private float timer;
    private IObjectPool _pool;

    public void SetPool(IObjectPool pool) => _pool = pool;

    public void Spawn(Vector2 position, Vector2 direction)
    {
        transform.position = position;
        gameObject.SetActive(true);

        timer = lifetime;

        moveDir = new Vector3(
            Random.Range(-0.5f, 0.5f),
            Random.Range(1f, 2f),
            0f
        );
    }

    public void Setup(int damage)
    {
        text.text = damage.ToString();
    }

    public void Despawn()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!gameObject.activeSelf)
            return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            _pool?.ReleaseObject(this);
            return;
        }

        transform.position += moveDir * Time.deltaTime;
        moveDir *= 0.9f;
    }
}