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

	protected float TimeOfLastAttack = 0f;

	public GameObject HealthBarContainer;

	#region -----[ Unity Lifecycle ]-------------------------------------------

	public virtual void Start()
    {
		CurrentHealth = MaxHealth;
	}
	
    public virtual void Update()
    {
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
		Debug.Log(name + " took " + rawDamage + " damage.  Current Health: " + CurrentHealth);
	}

	public virtual void ReceiveHeal(int rawHealAmount)
	{
		if (CurrentHealth + rawHealAmount >= MaxHealth)
		{
			CurrentHealth = MaxHealth;
		}
		else
		{
			CurrentHealth += rawHealAmount;
		}

		UpdateHealthBar();
		Debug.Log(name + " was healed for " + rawHealAmount + " health.  Current Health: " + CurrentHealth);
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
