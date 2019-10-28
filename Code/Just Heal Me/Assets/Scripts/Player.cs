using Scriptable;
using UnityEngine;

public class Player : Unit
{
    [SerializeField] private PlayerData playerData;

    public float Speed => playerData.walkSpeed;

    public CharacterController CharacterController { get; private set; }

    private UnityEngine.Camera _camera;

	#region -----[ Unity Lifecycle ]-------------------------------------------

	public override void Awake()
    {
		base.Awake();

        CharacterController = GetComponent<CharacterController>();
    }

    public override void Start()
	{
		base.Start();
	}

	#endregion

	#region -----[ Protected Functions ]------------------------------------------

	protected override Vector3 GetVelocity()
	{
		return CharacterController.velocity;
	}

	#endregion

	#region -----[ Public Functions ]------------------------------------------

	public override void SetupUnitAngles(float CameraAngle)
	{
		// Update the intial rotation of the player
		var angles = transform.rotation.eulerAngles;
		angles.x = CameraAngle;
		transform.rotation = Quaternion.Euler(angles);

		// Now change our Y position by 25%
		var position = transform.position;
		position.y -= (position.y * .25f);
		transform.position = position;
	}

	#endregion
}