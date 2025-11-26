using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerMovement _movement;
    private SpriteFlipper _spriteFlipper;
    private PlayerInteractions _interactions;
    private Animator _animator;

    private Vector2 _moveInput;

    void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
        _spriteFlipper = GetComponent<SpriteFlipper>();
        _interactions = GetComponent<PlayerInteractions>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        _moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _movement.Move(_moveInput);

        bool isWalking = _moveInput.sqrMagnitude > 0;
        _animator.SetBool("isWalking", isWalking);

        _spriteFlipper.Flip(_moveInput);
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            _interactions.Interact();
        }
    }
}