using Core;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class InputManager : Singleton<InputManager>
    {
        public float CameraAngle { get; set; } = 45; // Default is 45degrees

        private Player _player;
        private bool _facingRight = true;

        #region -----[ Unity Lifecycle ]---------------------------------------

        private void Awake()
        {
        }

        // Start is called before the first frame update
        void Start()
        {
            // Find the Player  
            _player = FindObjectOfType<Player>();
            if (_player == null) Debug.LogError("Could not find the player!");

            Unit[] units = FindObjectsOfType<Unit>();

            for (int i = 0; i < units.Length; i++)
            {
                units[i].SetupUnitAngles(CameraAngle);
            }
        }

        // Update is called once per frame
        void Update()
        {
            HandleMovement();
            HandleActions();
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
            //_player.HealthBarContainer.transform.rotation = Quaternion.Euler(_facingRight ? CameraAngle : CameraAngle * -1, _facingRight ? 180 : 0, 0);

            var currentSpeed = isRunning ? _player.Speed * 2f : _player.Speed;
            var velocity = currentSpeed * Time.deltaTime * new Vector3(horizontal, 0, vertical);

//            velocity.y = _player.transform.position.y; // This needs to be a static position

            _player.CharacterController.Move(velocity);
        }

        private void HandleActions()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Unit clickedUnit = GetClickedUnit();
                clickedUnit?.ReceiveHeal(_player.GetHealingPower());
            }

            if (Input.GetMouseButtonDown(1))
            {
                // This code is only here to test damaging a unit.  It will eventually be another player ability.
                Unit clickedUnit = GetClickedUnit();
                clickedUnit?.TakeDamage(_player.GetAttackPower());
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }

        private Unit GetClickedUnit()
        {
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                return hit.collider.gameObject.GetComponent<Unit>();
            }

            return null;
        }

        #endregion

        #region -----[ Co-Routines ]-------------------------------------------

        #endregion
    }
}