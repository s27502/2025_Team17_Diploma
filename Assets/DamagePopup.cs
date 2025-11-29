using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private float lifetime = 1f;
    [SerializeField] private Vector2 moveSpeedRange = new Vector2(1f, 2f);
    [SerializeField] private TextMeshPro text;
    private Vector3 moveDir;
    
    public void Setup(int damage)
    {
        text.text = damage.ToString();

        moveDir = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(1f, 2f), 0f);
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += moveDir * Time.deltaTime;
        moveDir *= 0.9f;
    }
}