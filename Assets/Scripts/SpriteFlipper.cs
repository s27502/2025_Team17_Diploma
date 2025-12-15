
using UnityEngine;

public class SpriteFlipper : MonoBehaviour
{
    private SpriteRenderer _sr;
    [SerializeField] private SpriteRenderer helmet;
    [SerializeField] private SpriteRenderer armor;
    [SerializeField] private Animator _helmetAnim;
    [SerializeField] private Animator _armorAnim;
    private bool _lastFlip;

    void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _helmetAnim.enabled = false;
        _armorAnim.enabled = false;
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

    public void SetArmorAnimator(RuntimeAnimatorController animator)
    {
        _armorAnim.runtimeAnimatorController = animator;
        _armorAnim.enabled = true;
    }

    public void SetHelmetAnimator(RuntimeAnimatorController animator)
    {
        _helmetAnim.runtimeAnimatorController = animator;
        _helmetAnim.enabled = true;
    }
}