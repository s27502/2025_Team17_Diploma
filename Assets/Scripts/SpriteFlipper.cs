using UnityEngine;

public class SpriteFlipper : MonoBehaviour
{
    private SpriteRenderer _sr;
    [SerializeField] private SpriteRenderer helmet;
    [SerializeField] private SpriteRenderer armor;
    private bool _lastFlip;

    void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    public void Flip(Vector2 movement)
    {
        if (movement.x != 0)
        {
            _sr.flipX = movement.x < 0;
            helmet.flipX = movement.x < 0;
            armor.flipX = movement.x < 0;
            _lastFlip = _sr.flipX;
        }
        else if (movement.y != 0)
        {
            _sr.flipX = _lastFlip;
            helmet.flipX = _lastFlip;
            armor.flipX = _lastFlip;
        }
        else
        {
            _sr.flipX = _lastFlip;
            helmet.flipX = _lastFlip;
            armor.flipX = _lastFlip;
        }
    }

    public SpriteRenderer GetHelmetRenderer()
    {
        return helmet;
    }

    public SpriteRenderer GetArmorRenderer()
    {
        return armor;
    }
}