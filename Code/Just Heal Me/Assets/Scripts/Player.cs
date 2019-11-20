using Scriptable;
using System.Collections;
using UnityEngine;

public class Player : Unit
{
	[SerializeField]
	private int HealSpellManaCost;
	[SerializeField]
	private float HealSpellCastTime;
	[SerializeField]
	private int StunSpellManaCost;
	[SerializeField]
	private float StunSpellCastTime;
	[SerializeField]
	private int ReviveSpellManaCost;
	[SerializeField]
	private float ReviveSpellCastTime;

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

	public GameObject CastBarContainer;
	private float CurrentCastTime = 0f;
	private float TimeCurrentSpellStartedBeingCast = 0f;
	
	Coroutine SpellCastingCoroutine;

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

		CheckForCancellingCast();
		UpdateCastBar();
	}

	#endregion

	#region -----[ private Functions ]------------------------------------------

	private void SetCurrentCastValues(float newCastTime)
	{
		TimeCurrentSpellStartedBeingCast = Time.timeSinceLevelLoad;
		CurrentCastTime = newCastTime;
	}

	private void ResetCurrentCastValues()
	{
		TimeCurrentSpellStartedBeingCast = Time.timeSinceLevelLoad;
		CurrentCastTime = 0f;
	}

	private void CancelCurrentCasting()
	{
		ResetCurrentCastValues();

		StopCoroutine(SpellCastingCoroutine);
	}

	private void UpdateCastBar()
	{
		if (IsCastingSpell())
		{
			CastBarContainer.transform.localScale = new Vector3((Time.timeSinceLevelLoad - TimeCurrentSpellStartedBeingCast) / CurrentCastTime, CastBarContainer.transform.localScale.y, CastBarContainer.transform.localScale.z);
		}
		else
		{
			CastBarContainer.transform.localScale = new Vector3(0f, CastBarContainer.transform.localScale.y, CastBarContainer.transform.localScale.z);
		}
	}

	private bool CanCastSpell(int manaCost, float castTime)
	{
		if (CurrentMana < manaCost)
		{
			return false;
		}

		if (IsCastingSpell())
		{
			return false;
		}

		if (GetVelocity().magnitude > 0f && castTime > 0f)
		{
			return false;
		}

		return true;
	}

	private void CheckForCancellingCast()
	{
		if (IsCastingSpell() && GetVelocity().magnitude > 0f)
		{
			CancelCurrentCasting();
		}
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
			if (CanCastSpell(HealSpellManaCost, HealSpellCastTime))
			{
				SpellCastingCoroutine = StartCoroutine(CoroutineHealUnit(unit));
			}
		}
	}

	IEnumerator CoroutineHealUnit(Unit unit)
	{
		SetCurrentCastValues(HealSpellCastTime);
		
		Managers.GameManager.Instance.Sound.PlayCastingHealSound();

		yield return new WaitForSeconds(HealSpellCastTime);
		
		CurrentMana -= HealSpellManaCost;
		UpdateManaBar();

		unit.ReceiveHeal(GetHealingPower());
	}

	public void StunUnit(Unit unit)
	{
		if (unit != null && !unit.IsDead() && unit.IsBadGuy() && CanStun())
		{
			if (CanCastSpell(StunSpellManaCost, StunSpellCastTime))
			{
				SpellCastingCoroutine = StartCoroutine(CoroutineStunUnit(unit));
			}
		}
	}

	IEnumerator CoroutineStunUnit(Unit unit)
	{
		SetCurrentCastValues(StunSpellCastTime);

		yield return new WaitForSeconds(StunSpellCastTime);
		
		CurrentMana -= StunSpellManaCost;
		UpdateManaBar();

		unit.Stun(3.0f);
	}

	public void ReviveUnit(Unit unit)
	{
		if (unit != null && unit.IsDead() && unit.IsGoodGuy())
		{
			if (CanCastSpell(ReviveSpellManaCost, ReviveSpellCastTime))
			{
				SpellCastingCoroutine = StartCoroutine(CoroutineReviveUnit(unit));
			}
		}
	}

	IEnumerator CoroutineReviveUnit(Unit unit)
	{
		SetCurrentCastValues(ReviveSpellCastTime);

		yield return new WaitForSeconds(ReviveSpellCastTime);

		CurrentMana -= ReviveSpellManaCost;
		UpdateManaBar();

		unit.Revive();

		ReviveSpellCastTime += 2f;
	}

	public bool IsCastingSpell()
	{
		return TimeCurrentSpellStartedBeingCast + CurrentCastTime > Time.timeSinceLevelLoad;
	}

	public override void Reset()
	{
		base.Reset();

		CharacterController.enabled = false;
		CharacterController.transform.position = StartingPosition;
		CharacterController.enabled = true;
	}

	#endregion
}