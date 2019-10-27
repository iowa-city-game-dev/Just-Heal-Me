using Scriptable;
using UnityEngine;

public class Player : Unit
{
    [SerializeField] private PlayerData playerData;

    public float Speed => playerData.walkSpeed;

    public CharacterController CharacterController { get; private set; }

    public Animator Animator { get; private set; }

    public Animation Animation { get; private set; }


    private UnityEngine.Camera _camera;
    
    #region -----[ Unity Lifecycle ]-------------------------------------------

    private void Awake()
    {
        CharacterController = GetComponent<CharacterController>();
        Animator = GetComponent<Animator>();
        Animation = GetComponent<Animation>();
    }

    public override void Start()
	{
		base.Start();
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