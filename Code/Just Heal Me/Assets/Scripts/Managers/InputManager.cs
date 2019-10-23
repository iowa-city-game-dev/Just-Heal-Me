using System.Collections;
using Core;
using UnityEngine;

namespace Managers
{
    public class InputManager : Singleton<InputManager>
    {
        private Player _player;
        private bool _facingRight = true;

        #region -----[ Unity Lifecycle ]---------------------------------------

        // Start is called before the first frame update
        void Start()
        {
            // Find the Player  
            _player = FindObjectOfType<Player>();
            if (_player == null) Debug.LogError("Could not find the player!");
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
            _player.transform.rotation = Quaternion.Euler(_facingRight ? 45 : -45, _facingRight ? 0 : 180, 0);

            var velocity = _player.Speed * new Vector3(horizontal, 0, vertical); // Speed (3) is hardcoded for right now
            _player.CharacterController.Move(velocity * Time.deltaTime);
        }

        #endregion

        #region -----[ Co-Routines ]-------------------------------------------

        #endregion
    }
}