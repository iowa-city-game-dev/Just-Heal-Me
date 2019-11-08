using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerUnit : AiUnit
{
	[SerializeField]
	float HealingRange;

	[SerializeField]
	protected float HealingSpeed;

	protected float TimeOfLastHeal = 0f;

	#region -----[ Unity Lifecycle ]-------------------------------------------

	protected override void UpdateUnitSpecifics()
	{
		UpdateTarget();
		HandleHealing();
	}

	#endregion

	#region -----[ Private Functions ]------------------------------------------

	private void UpdateTarget()
	{
		if (IsStunned())
		{
			Target = null;
			return;
		}
		
		Unit allyToHeal = null;
		int lowestHealth = int.MaxValue;

		Ray ray;
		RaycastHit hit;

		for (int i = 0; i < Allies.Count; i++)
		{
			if (!Allies[i].IsDead() && Allies[i].GetCurrentHealth() < Allies[i].GetMaxHealth())
			{
				if (Allies[i].GetCurrentHealth() <= lowestHealth)
				{
					ray = new Ray(transform.position, Allies[i].transform.position - transform.position);
					if (Physics.Raycast(ray, out hit))
					{
						if (hit.collider.name == Allies[i].name)
						{
							lowestHealth = Allies[i].GetCurrentHealth();
							allyToHeal = Allies[i];
						}
					}
				}
			}
		}

		Target = allyToHeal;
	}

	private void HandleHealing()
	{
		if (Target != null && !Target.IsDead() && Target.GetCurrentHealth() < Target.GetMaxHealth())
		{
			if (IsInHealingRangeOfTarget())
			{
				if (TimeOfLastHeal + HealingSpeed < Time.timeSinceLevelLoad)
				{
					InitiateHeal();
				}
			}
		}
	}

	private bool IsInHealingRangeOfTarget()
	{
		if (Target != null)
		{
			return HealingRange >= Vector3.Distance(transform.position, Target.transform.position);
		}

		return false;
	}

	protected virtual void InitiateHeal()
	{
		Target.ReceiveHeal(HealingPower);
		TimeOfLastHeal = Time.timeSinceLevelLoad;

		Animator.SetTrigger("Attack");
	}

	protected override void OnUnstun()
	{
		TimeOfLastHeal = Time.timeSinceLevelLoad;
	}

	#endregion
}
