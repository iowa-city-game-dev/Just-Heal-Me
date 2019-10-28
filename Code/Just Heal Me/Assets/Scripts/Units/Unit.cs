using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, IUnit
{
	[SerializeField] protected int MaxHealth;
	[SerializeField] protected int CurrentHealth;

	[SerializeField] protected int HealingPower;
	[SerializeField] protected int AttackPower;

	[SerializeField] protected bool _isBadGuy;

	[SerializeField] protected float AttackRange;
	[SerializeField] protected float AttackSpeed;

	public Animator Animator { get; protected set; }
	public Animation Animation { get; protected set; }


	protected SpriteRenderer _spriteRenderer;
	protected Color NormalColor;
	private Color ReceivedHealColor = Color.yellow;

	protected float TimeOfLastAttack = 0f;

	public GameObject HealthBarContainer;

	private float TimeLastHealWasReceived = 0f;
	private float HealColorDuration = 0.5f;

	#region -----[ Unity Lifecycle ]-------------------------------------------

	public virtual void Awake()
	{
		Animator = GetComponent<Animator>();
		Animation = GetComponent<Animation>();
	}

	public virtual void Start()
    {
		CurrentHealth = MaxHealth;

		_spriteRenderer = GetComponent<SpriteRenderer>();

		if (_spriteRenderer == null)
		{
			_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		}

		if (_spriteRenderer != null)
		{
			NormalColor = _spriteRenderer.color;
		}
	}
	
    public virtual void Update()
    {
		UpdateAnimatorValues();
		UpdateColor();
	}

	#endregion

	#region -----[ Private Functions ]------------------------------------------

	private void UpdateAnimatorValues()
	{
		Animator.SetFloat("HorizontalSpeed", Mathf.Abs(GetVelocity().x));
	}

	private void UpdateColor()
	{
		if (_spriteRenderer.color == ReceivedHealColor)
		{
			if (Time.timeSinceLevelLoad > TimeLastHealWasReceived + HealColorDuration)
			{
				_spriteRenderer.color = NormalColor;
			}
		}
	}

	#endregion

	#region -----[ Protected Functions ]------------------------------------------

	protected virtual Vector3 GetVelocity()
	{
		return new Vector3();
	}

	#endregion

	#region -----[ Public Functions ]------------------------------------------

	public virtual void TakeDamage(int rawDamage)
	{
		if (CurrentHealth - rawDamage <= 0)
		{
			CurrentHealth = 0;
		}
		else
		{
			CurrentHealth -= rawDamage;
		}

		UpdateHealthBar();
	}

	public virtual void ReceiveHeal(int rawHealAmount)
	{
		if (CurrentHealth != MaxHealth)
		{
			_spriteRenderer.color = ReceivedHealColor;
			TimeLastHealWasReceived = Time.timeSinceLevelLoad;

			if (CurrentHealth + rawHealAmount >= MaxHealth)
			{
				CurrentHealth = MaxHealth;
			}
			else
			{
				CurrentHealth += rawHealAmount;
			}

			UpdateHealthBar();
		}
	}

	private void UpdateHealthBar()
	{
		HealthBarContainer.transform.localScale = new Vector3((float)CurrentHealth / MaxHealth, HealthBarContainer.transform.localScale.y, HealthBarContainer.transform.localScale.z);
	}

	public virtual int GetCurrentHealth()
	{
		return CurrentHealth;
	}

	public virtual int GetHealingPower()
	{
		return HealingPower;
	}

	public virtual int GetAttackPower()
	{
		return AttackPower;
	}

	public virtual bool IsDead()
	{
		return CurrentHealth == 0;
	}

	public virtual bool IsBadGuy()
	{
		return _isBadGuy;
	}

	public virtual bool IsGoodGuy()
	{
		return !_isBadGuy;
	}

	public virtual void SetupUnitAngles(float CameraAngle)
	{
	}

	#endregion
}
