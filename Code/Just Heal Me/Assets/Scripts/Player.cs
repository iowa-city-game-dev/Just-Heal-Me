using Scriptable;
using UnityEngine;

public class Player : Unit
{
	[SerializeField]
	private int HealSpellManaCost;
	[SerializeField]
	private int StunSpellManaCost;

	[SerializeField]
	private int HealthRegenAmount;
	[SerializeField]
	private int ManaRegenAmount;
	[SerializeField]
	private float HealthRegenRate;
	[SerializeField]
	private float ManaRegenRate;

	private float _modulusTimeOfLastHealthRegenCheck = 0f;
	private float _modulusTimeOfLastManaRegenCheck = 0f;

	[SerializeField]
	private PlayerData playerData;

    public float Speed => playerData.walkSpeed;

    public CharacterController CharacterController { get; private set; }

    private UnityEngine.Camera _camera;

	[HideInInspector]
	public Unit StunnedUnit;

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

	public override void Update()
	{
		base.Update();

		if (Time.timeSinceLevelLoad % HealthRegenRate < _modulusTimeOfLastHealthRegenCheck && CurrentHealth < MaxHealth)
		{
			if (CurrentHealth + HealthRegenAmount > MaxHealth)
			{
				CurrentHealth = MaxHealth;
			}
			else
			{
				CurrentHealth += HealthRegenAmount;
			}

			UpdateHealthBar();
		}
		_modulusTimeOfLastHealthRegenCheck = Time.timeSinceLevelLoad % HealthRegenRate;


		if (Time.timeSinceLevelLoad % ManaRegenRate < _modulusTimeOfLastManaRegenCheck && CurrentMana < MaxMana)
		{
			if (CurrentMana + ManaRegenAmount > MaxMana)
			{
				CurrentMana = MaxMana;
			}
			else
			{
				CurrentMana += ManaRegenAmount;
			}

			UpdateManaBar();
		}
		_modulusTimeOfLastManaRegenCheck = Time.timeSinceLevelLoad % ManaRegenRate;
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

	public bool CanStun()
	{
		if (StunnedUnit != null && !StunnedUnit.IsStunned())
		{
			StunnedUnit = null;
		}

		return StunnedUnit == null;
	}

	public void HealUnit(Unit unit)
	{
		if (unit != null && !unit.IsDead() && unit.IsGoodGuy())
		{
			if (CurrentMana > HealSpellManaCost)
			{
				CurrentMana -= HealSpellManaCost;
				UpdateManaBar();

				unit.ReceiveHeal(GetHealingPower());
			}
		}
	}

	public void StunUnit(Unit unit)
	{
		if (unit != null && !unit.IsDead() && unit.IsBadGuy() && CanStun())
		{
			if (CurrentMana > StunSpellManaCost)
			{
				CurrentMana -= StunSpellManaCost;
				UpdateManaBar();

				unit.Stun(3.0f);
			}
		}
	}

	#endregion
}