using System;
using Player;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerMovement _movement;
    private SpriteFlipper _spriteFlipper;
    private PlayerInteractions _interactions;
    private Animator _animator;
    private PauseManager _pauseManager;
    [SerializeField] private Animator helmetAnimator;
    [SerializeField] private Animator armorAnimator;

    private Vector2 _moveInput;

    void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
        _spriteFlipper = GetComponent<SpriteFlipper>();
        _interactions = GetComponent<PlayerInteractions>();
        _animator = GetComponent<Animator>();
        
    }

    void Start()
    {
        _pauseManager = ServiceLocator.Instance.GetService<PauseManager>();
    }

    private void FixedUpdate()
    {
        _moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _movement.Move(_moveInput);

        bool isWalking = _moveInput.sqrMagnitude > 0;
        _animator.SetBool("isWalking", isWalking);
        helmetAnimator.SetBool("isWalking", isWalking);
        armorAnimator.SetBool("isWalking", isWalking);
        _spriteFlipper.Flip(_moveInput);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _interactions.Interact();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _movement.Dash();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_pauseManager.GetPaused())
            {
                _pauseManager.Resume();
            }
            else
            {
                _pauseManager.Pause();
            }
        }
    }
}