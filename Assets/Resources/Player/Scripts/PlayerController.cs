using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    private static PlayerController _instance;
    public static PlayerController Instance
    {
        get
        {
            if (_instance == null)
            {
                var playerObj = Instantiate(Resources.Load<GameObject>("Player/Prefabs/Player"));
                DontDestroyOnLoad(playerObj);
                playerObj.name = playerObj.name.Replace("(Clone)", "");
                playerObj.SetActive(true);
                _instance = playerObj.GetComponent<PlayerController>();
            }

            return _instance;
        }
    }
    
    public float attackCooldown;
    public float jumpSpeed;
    public float jumpBufferTime;
    public float jumpTimeMax;
    public float runSpeed;
    public float walkSpeed;

    public int currentHealth;
    public int maxHealth;

    public GameObject slashEffectPrefab;

    public Interactive interactingWith;

    public UnityEvent<int, int> healthChanged;

    private Animator _anim;
    private Collider2D _col;
    private Rigidbody2D _body;
    private SpriteRenderer _sprite;

    private PlayerInputActions _input;

    private Vector2 _inputVector;
    private float _jumpBufferTimer;
    private float _jumpTime;
    private float _timeBetweenAttacks;

    private bool _bufferingJump;
    private bool _jumping;
    private bool _interacting;
    private bool _reading;

    private void Awake()
    {
        _anim ??= GetComponent<Animator>();
        _col ??= GetComponent<Collider2D>();
        _body ??= GetComponent<Rigidbody2D>();
        _sprite ??= GetComponent<SpriteRenderer>();

        _input ??= new PlayerInputActions();
        _instance ??= this;
        DontDestroyOnLoad(_instance.gameObject);

        _terrainMask = 1 << LayerMask.NameToLayer("Terrain");

        UIManager.Instance.GetComponentInChildren<Typewriter>(true).finishedTyping.AddListener(() => _reading = false);
    }

    private void Update()
    {
        _timeBetweenAttacks += Time.deltaTime;
        _inputVector = new Vector2(_input.Player.Movement.ReadValue<float>(), _input.Player.Vertical.ReadValue<float>());
        if (!_interacting && Mathf.Abs(_inputVector.x) > 0)
        {
            if (_inputVector.x < 0 && transform.localScale.x > 0 ||
                _inputVector.x > 0 && transform.localScale.x < 0)
            {
                Flip();
            }
            
            //_anim.Play("Walk");
            _body.velocity = new Vector2(transform.localScale.x * walkSpeed, _body.velocity.y);
        }
        else
        {
            //_anim.Play("Idle");
            _body.velocity = new Vector2(0, _body.velocity.y);
        }

        if (!_interacting)
        {
            if (_bufferingJump)
            {
                if (IsGrounded())
                {
                    _bufferingJump = false;
                    _jumpBufferTimer = 0;
                    _jumping = true;
                }

                _jumpBufferTimer += Time.deltaTime;
                if (_jumpBufferTimer >= jumpBufferTime)
                {
                    _bufferingJump = false;
                    _jumpBufferTimer = 0;
                }
            }

            if (_input.Player.Jump.WasPressedThisFrame())
            {
                if (IsGrounded())
                {
                    _jumping = true;
                }
                else
                {
                    _bufferingJump = true;
                }
            }

            if (!_input.Player.Jump.IsPressed() && _jumping)
            {
                _jumping = false;
                _jumpTime = 0;
                if (_body.velocity.y > 0)
                {
                    _body.velocity = new Vector2(_body.velocity.x, jumpSpeed / 2);
                }
            }

            if (_jumping)
            {
                //_anim.Play("Jump");
                _jumpTime += Time.deltaTime;
                if (_jumpTime <= jumpTimeMax)
                {
                    _body.velocity = new Vector2(_body.velocity.x, jumpSpeed);
                }
                else
                {
                    _jumping = false;
                    _jumpTime = 0;
                    if (_body.velocity.y > 0)
                    {
                        _body.velocity = new Vector2(_body.velocity.x, jumpSpeed / 2);
                    }
                }
            }
        }
    }

    private void OnEnable()
    {
        StartControl();
    }

    private void OnDisable()
    {
        StopControl();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player Damager"))
        {
            var enemy = collider.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                currentHealth -= enemy.damager.damageAmount;
                healthChanged.Invoke(maxHealth, currentHealth);
            }
        }
    }

    public void StopControl()
    {
        _input.Disable();

        _input.Player.Attack.performed -= Attack;
        _input.Player.Interact.performed -= Interact;
    }

    public void StartControl()
    {
        _input.Enable();

        _input.Player.Attack.performed += Attack;
        _input.Player.Interact.performed += Interact;
    }

    private void Attack(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValueAsButton() && _timeBetweenAttacks >= attackCooldown)
        {
            _timeBetweenAttacks = 0;
            GameObject slashEffect = Instantiate(slashEffectPrefab, transform.position, Quaternion.identity);
            if (_inputVector.y > Mathf.Epsilon)
            {
                //_anim.Play("Attack Up");
                slashEffect.transform.Rotate(0, 0, -90 * transform.localScale.x);
            }
            else if (_inputVector.y < -Mathf.Epsilon && !IsGrounded())
            {
                //_anim.Play("Attack Down");
                slashEffect.transform.Rotate(0, 0, 90 * transform.localScale.x);
            }
            else
            {
                //_anim.Play("Attack");
                slashEffect.transform.Rotate(0, 0, 180);
            }
        }
    }

    private Coroutine _typeRoutine;
    private void Interact(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValueAsButton() && interactingWith != null)
        {
            switch (interactingWith.interactionType)
            {
                case InteractionType.Note:
                    var typewriter = UIManager.Instance.GetComponentInChildren<Typewriter>(true);
                    if (!_reading && !_interacting)
                    {
                        UIManager.Instance.ShowUI("Fader");
                        _interacting = _reading = true;
                        _typeRoutine = StartCoroutine(typewriter.StartTyping(((InteractiveNote)interactingWith).interactText));
                    }
                    else if (_reading && _interacting)
                    {
                        _reading = false;
                        typewriter.SkipTyping();
                        StopCoroutine(_typeRoutine);
                    }
                    else if (!_reading && _interacting)
                    {
                        _interacting = false;
                        UIManager.Instance.HideUI("Fader");
                    }
                    break;
            }
        }
    }

    public void Flip()
    {
        Vector2 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private float _groundCheck = 0.02f;
    private int _terrainMask;
    public bool IsGrounded()
    {
        Bounds bounds = _col.bounds;
        var leftOrigin = new Vector2(bounds.min.x, bounds.min.y);
        var centerOrigin = new Vector2(bounds.center.x, bounds.min.y);
        var rightOrigin = new Vector2(bounds.max.x, bounds.min.y);
        Debug.DrawRay(leftOrigin, Vector2.down, Color.red);
        Debug.DrawRay(centerOrigin, Vector2.down, Color.red);
        Debug.DrawRay(rightOrigin, Vector2.down, Color.red);
        return Physics2D.Raycast(leftOrigin, Vector2.down, _groundCheck, _terrainMask) ||
               Physics2D.Raycast(centerOrigin, Vector2.down, _groundCheck, _terrainMask) ||
               Physics2D.Raycast(rightOrigin, Vector2.down, _groundCheck, _terrainMask);
    }
}
