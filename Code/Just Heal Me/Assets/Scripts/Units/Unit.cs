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
	private Color StunnedColor = Color.blue;

	protected float TimeOfLastAttack = 0f;

	public GameObject HealthBarContainer;

	private float TimeLastHealWasReceived = 0f;
	private float HealColorDuration = 0.5f;

	private float TimeStunStarted = 0f;
	private float StunDuration = 0f;
	private bool _wasStunned = false;

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

		if (_wasStunned && !IsStunned())
		{
			OnUnstun();
		}

		_wasStunned = IsStunned();
	}

	#endregion

	#region -----[ Private Functions ]------------------------------------------

	private void UpdateAnimatorValues()
	{
		Animator.SetFloat("HorizontalSpeed", Mathf.Abs(GetVelocity().x));

		if (IsStunned())
		{
			Animator.speed = 0;
		}
		else
		{
			Animator.speed = 1;
		}
	}

	private void UpdateColor()
	{
		if (IsStunned())
		{
			_spriteRenderer.color = StunnedColor;
		}
		else if (Time.timeSinceLevelLoad < TimeLastHealWasReceived + HealColorDuration)
		{
			_spriteRenderer.color = ReceivedHealColor;
		}
		else
		{
			_spriteRenderer.color = NormalColor;
		}
	}

	private void UpdateHealthBar()
	{
		HealthBarContainer.transform.localScale = new Vector3((float)CurrentHealth / MaxHealth, HealthBarContainer.transform.localScale.y, HealthBarContainer.transform.localScale.z);
	}

	#endregion

	#region -----[ Protected Functions ]------------------------------------------

	protected virtual Vector3 GetVelocity()
	{
		return new Vector3();
	}

	protected virtual void OnUnstun()
	{
	}

	#endregion

	#region -----[ Public Functions ]------------------------------------------

	public virtual void TakeDamage(int rawDamage)
	{
		TimeStunStarted = 0;

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

	public virtual void Stun(float stunDuration)
	{
		StunDuration = stunDuration;
		TimeStunStarted = Time.timeSinceLevelLoad;
	}

	public bool IsStunned()
	{
		return Time.timeSinceLevelLoad < TimeStunStarted + StunDuration;
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
