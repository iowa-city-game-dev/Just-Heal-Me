using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedUnit : AiUnit
{
	[SerializeField]
	GameObject ArrowObject;

	#region -----[ Unity Lifecycle ]-------------------------------------------

	public override void Start()
    {
		base.Start();

		ArrowObject.transform.parent = null;
		ArrowObject.SetActive(false);
	}
	
    public override void Update()
    {
		base.Update();
	}

	#endregion

	#region -----[ Protected Functions ]------------------------------------------

	protected override void InitiateAttack()
	{
		if (!ArrowObject.activeInHierarchy)
		{
			ArrowObject.SetActive(true);
			ArrowObject.transform.position = transform.position;
			ArrowObject.GetComponent<Arrow>().Fire(Target.gameObject, AttackPower);
			
			TimeOfLastAttack = Time.timeSinceLevelLoad;

			Animator.SetTrigger("Attack");
		}
	}

	#endregion
}
