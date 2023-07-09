using UnityEngine;

namespace Assets.Platforming_script_one
{
    public class PlayerVfx : VFXPlayer
    {
        private PlayerController _controller;

        private void Awake()
        {
            _controller = GetComponent<PlayerController>();
        }

        private void Update()
        {
            float xInput = Input.GetAxisRaw("Horizontal");
            bool isGrounded = _controller.Collisions.Below;
            
            if (CurrentState != "jump")
                    SetState("fall");
            

            if (xInput > 0f)
            {
                sprite.flipX = false;
            }
            else if (xInput < 0f)
            {
                sprite.flipX = true;
            }
        }
    }
}
