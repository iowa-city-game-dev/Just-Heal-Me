using System;
using UnityEngine;

namespace Camera
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Player target = null;

        // This is calculated at where the position of the camera is on the Z Plane on Awake
        private float _zOffset = 0;

        #region -----[ Unity Lifecycle Events ]------------------------------------

        private void Awake()
        {
            _zOffset = transform.position.z;
        }

        private void Start()
        {
            // Since we always need a target, throw out some errors if it's null
            if (target == null) Debug.LogError("[FollowCamera]: Start - No target is set for the camera to follow, please set a target!");
        }

        private void Update()
        {
            //NB: We expect a target, so we aren't doing a null check here
            
            // Keep the camera steady
            var position = transform.position;
            var targetPosition = target.transform.position;

            var newPosition = new Vector3(targetPosition.x, position.y, targetPosition.z + _zOffset);

            // Update the cameras position
            transform.position = newPosition;
        }

        #endregion
    }
}