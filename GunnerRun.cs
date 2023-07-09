using UnityEngine;

namespace Assets.Platforming_script_one
{
    [RequireComponent(typeof(PlayerController))]
    public class GunnerRun : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
    
        [SerializeField] private float jumpTime;
        [SerializeField] private float jumpHeight;

        [SerializeField] private Transform RotationHolder;
        [SerializeField] private Transform CheckPoint;

        [SerializeField] private float checkDistance;

        private float _jumpVelocity;
        private float _gravity;
    
        private Vector3 _velocity;

        private PlayerController _controller;
        private bool movingRight;

        private void Start()
        {
            _controller = GetComponent<PlayerController>();

            _gravity = -(2 * jumpHeight) / Mathf.Pow(jumpTime, 2);
            _jumpVelocity = Mathf.Abs(_gravity) * jumpTime;
        }

        private void Update()
        {
            if (_controller.Collisions.Above || _controller.Collisions.Below)
            {
                _velocity.y = 0;
            }

            RaycastHit2D groundInfo = Physics2D.Raycast(CheckPoint.position, Vector2.down, checkDistance);

            if (groundInfo.collider == false)
            {
                movingRight = !movingRight;
                RotationHolder.eulerAngles = !movingRight ? new Vector3(0f, 180, 0f) : new Vector3(0f, 0, 0f);
            }

            RotationHolder.eulerAngles = !movingRight ? new Vector3(0f, 180, 0f) : new Vector3(0f, 0, 0f);
            _velocity.x = !movingRight ? -moveSpeed * Time.deltaTime : moveSpeed * Time.deltaTime;
            
            _velocity.y += (_gravity * Time.deltaTime);
            
            _controller.Move(_velocity);
        }
    }
}
