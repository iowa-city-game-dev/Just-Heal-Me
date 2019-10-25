using Core;
using UnityEngine;

namespace Managers
{
    public class InputManager : Singleton<InputManager>
    {
        public float CameraAngle { get; set; } = 45; // Default is 45degrees

        private Player _player;
        private bool _facingRight = true;
        
        #region -----[ Unity Lifecycle ]---------------------------------------

        // Start is called before the first frame update
        void Start()
        {
            // Find the Player  
            _player = FindObjectOfType<Player>();
            if (_player == null) Debug.LogError("Could not find the player!");

            _player.SetupPlayerAngles(CameraAngle);
        }

        // Update is called once per frame
        void Update()
        {
            HandleMovement();
        }

        #endregion

        #region -----[ Private Methods ]---------------------------------------

        private void HandleMovement()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            if (horizontal == 0f && vertical == 0f) return; // no need to do anything

            var isRunning = Input.GetKey(KeyCode.LeftShift);
            _player.Animator.speed = isRunning ? 3 : 1;

            // we want to maintain our facing direction
            if (horizontal < 0)
            {
                _facingRight = false;
            }
            else if (horizontal > 0)
            {
            
                _facingRight = true;
            }

            // Which way are we running?
            _player.transform.rotation = Quaternion.Euler(_facingRight ? CameraAngle : CameraAngle * -1, _facingRight ? 0 : 180, 0);

            var currentSpeed = isRunning ? _player.Speed * 2f : _player.Speed;
            var velocity = currentSpeed  * Time.deltaTime * new Vector3(horizontal, 0, vertical);

//            velocity.y = _player.transform.position.y; // This needs to be a static position
                
            _player.CharacterController.Move(velocity);
        }

        #endregion

        #region -----[ Co-Routines ]-------------------------------------------

        #endregion
    }
}