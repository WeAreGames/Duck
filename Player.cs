using UnityEngine;


namespace Assets.Platforming_script_one
{ 
    [RequireComponent(typeof(PlayerController))]
    public class Player : MonoBehaviour
 {
    [SerializeField] private float moveSpeed;

    [Space]
    
    [SerializeField] private float accelerationTimeAir;
    [SerializeField] private float accelerationTimeGround;
    
    [Space]

    [SerializeField] private float jumpHeight;

    [Space]

    [SerializeField] private float jumpTime;

    [Space]

    [Range(1, 5)]
    [SerializeField] private float gravityMultiplier;
    [SerializeField] private float gravityHeavy;

    [Space] 
    
    private float _velocitySmoothingX;

    private float _gravity;
    private float _jumpVelocity;

    [HideInInspector] public Vector3 _velocity;

    private PlayerController _controller;
    private PlayerVfx _vFX;

    public Player(float jumpTime, float gravityMultiplier, float jumpHeight, float accelerationTimeGround, float accelerationTimeAir, float moveSpeed)
    {
        this.jumpTime = jumpTime;
        this.gravityMultiplier = gravityMultiplier;
        this.jumpHeight = jumpHeight;
        this.accelerationTimeGround = accelerationTimeGround;
        this.accelerationTimeAir = accelerationTimeAir;
        this.moveSpeed = moveSpeed;
    }

    private void Start()
    {
        _controller = GetComponent<PlayerController>();
        _vFX = GetComponent<PlayerVfx>();

        _gravity = -(2 * jumpHeight) / Mathf.Pow(jumpTime, 2);

        _jumpVelocity = Mathf.Abs(_gravity) * jumpTime;

        print(_jumpVelocity + " " + _gravity);
    }

    private void Update()
    {
        if (_controller.Collisions.Above || _controller.Collisions.Below)
        {
            _velocity.y = 0;
        }

        if (Input.GetButtonDown("Jump"))
        {
            Jump(_jumpVelocity);
        }
        
        _velocity.y += (_gravity * Time.deltaTime) * gravityMultiplier;
         
        float targetVelocity = MoveInput().x * moveSpeed;
        _velocity.x = Mathf.SmoothDamp (_velocity.x, targetVelocity, ref _velocitySmoothingX, (_controller.Collisions.Below)?accelerationTimeGround:accelerationTimeAir);

        _controller.Move(_velocity * Time.deltaTime);
    }

    private Vector2 MoveInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void Jump(float vel)
    {
        _velocity.y = vel;
        _vFX.SetState("Jump");
    }
}
}



