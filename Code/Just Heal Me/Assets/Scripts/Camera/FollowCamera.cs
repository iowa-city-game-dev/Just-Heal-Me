using Managers;
using UnityEngine;
using UnityEngine.Rendering;

namespace Camera
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Player target = null;

        // This is calculated at where the position of the camera is on the Z Plane on Awake
        private float _zOffset = 0;
        private GameObject _lastHitObject = null;

        #region -----[ Unity Lifecycle Events ]------------------------------------

        private void Awake()
        {
            _zOffset = transform.position.z;
        }

        private void Start()
		{
			FindObjectOfType<InputManager>().CameraAngle = transform.rotation.eulerAngles.x;

			// Since we always need a target, throw out some errors if it's null
			if (target == null) Debug.LogError("[FollowCamera]: Start - No target is set for the camera to follow, please set a target!");
        }

        private void Update()
        {
            //NB: We expect a target, so we aren't doing a null check here

            MoveCamera();
            CheckForWallObstruction();
        }

        #endregion

        #region -----[ Private Functions ]-------------------------------------

        private void MoveCamera()
        {
            // Keep the camera steady
            var position = transform.position;
            var targetPosition = target.transform.position;

            var newPosition = new Vector3(targetPosition.x, position.y, targetPosition.z + _zOffset);

            // Update the cameras position
            transform.position = newPosition;
        }

        private void CheckForWallObstruction()
        {
            var playerPosition = transform.position;
            var forwardDirection = transform.TransformDirection(Vector3.forward);

            var rayHit = new RaycastHit();
            var ray = new Ray(playerPosition, forwardDirection);
            var rayLength = 5f;

            // NB: Left his here so if we need it later, we can use it.
//            var endPoint = forwardDirection * rayLength;
//            Debug.DrawLine(playerPosition, endPoint, Color.red);

            if (Physics.Raycast(ray, out rayHit, rayLength))
            {
                if (rayHit.transform.tag.Equals("Wall"))
                {
                    _lastHitObject = rayHit.transform.gameObject;
                    _lastHitObject.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.ShadowsOnly;
                }
            }
            else
            {
                if (_lastHitObject != null)
                {
                    _lastHitObject.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.On;
                    // Clean up 
                    _lastHitObject = null;
                }
            }
        }

        #endregion
    }
}