using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
	GameObject TargetObject;
	int AttackPower;

	#region -----[ Unity Lifecycle ]-------------------------------------------
	
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (TargetObject != null)
		{
			transform.position = Vector3.MoveTowards(transform.position, TargetObject.transform.position, 0.65f);
			transform.LookAt(TargetObject.transform.position);

			float dist = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(TargetObject.transform.position.x, TargetObject.transform.position.z));
			if (dist < 0.01f)
			{
				TargetObject.GetComponent<Unit>().TakeDamage(AttackPower);
				gameObject.SetActive(false);
				//transform.parent = other.transform;
			}
		}
	}

	#endregion

	#region -----[ Public Functions ]------------------------------------------

	public void Fire(GameObject targetObject, int attackPower)
	{
		//transform.parent = null;
		TargetObject = targetObject;
		AttackPower = attackPower;
	}

	#endregion
}
