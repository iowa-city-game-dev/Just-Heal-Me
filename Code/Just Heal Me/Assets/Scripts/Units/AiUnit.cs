using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiUnit : Unit
{
	private NavMeshAgent _navMeshAgent;

	private GameObject NavMeshDestination;
	private Vector3 NavMeshDestinationPosition;

	private List<Unit> Allies = new List<Unit>();
	private List<Unit> Enemies = new List<Unit>();

	GameObject EndOfLevel;

	public GameObject Visuals;

	float CameraAngle = 0f;

	Unit Target = null;

	public override void Start()
	{
		base.Start();

		_navMeshAgent = GetComponent<NavMeshAgent>();

		SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
		if (_isBadGuy)
		{
			renderer.color = Color.red;

			Allies = FindOtherUnitsOfTag("BadGuy");
			Enemies = FindOtherUnitsOfTag("GoodGuy");

			SetDestination(gameObject);
		}
		else
		{
			renderer.color = new Color(135f / 255f, 206f / 255f, 235f / 255f);

			Allies = FindOtherUnitsOfTag("GoodGuy");
			Enemies = FindOtherUnitsOfTag("BadGuy");

			EndOfLevel = GameObject.FindGameObjectWithTag("Goal");

			if (EndOfLevel != null)
			{
				SetDestination(EndOfLevel);
			}
			else
			{
				SetDestination(gameObject);
			}
		}

		Visuals.transform.SetParent(null);
		HealthBarContainer.transform.SetParent(null);
	}
	
	public override void Update()
	{
		if (!IsDead())
		{
			UpdateTarget();
			UpdateDestination();

			HandleAttacking();
		}

		Visuals.transform.position = transform.position;
		HealthBarContainer.transform.position = transform.position;
	}

	private void UpdateTarget()
	{
		float distanceToEnemy = 0f;
		Unit enemyToGoAfter = null;
		float shortestDistanceToLosTarget = float.MaxValue;

		if (!_isBadGuy)
		{
			if (Target != null)
			{
				shortestDistanceToLosTarget = Vector3.Distance(transform.position, Target.transform.position);
			}
			else if (EndOfLevel != null)
			{
				shortestDistanceToLosTarget = Vector3.Distance(transform.position, EndOfLevel.transform.position);
			}
		}

		Ray ray;
		RaycastHit hit;

		for (int i = 0; i < Enemies.Count; i++)
		{
			if (!Enemies[i].IsDead())
			{
				distanceToEnemy = Vector3.Distance(transform.position, Enemies[i].transform.position);
				if (distanceToEnemy <= shortestDistanceToLosTarget)
				{
					ray = new Ray(transform.position, Enemies[i].transform.position - transform.position);
					if (Physics.Raycast(ray, out hit))
					{
						if (hit.collider.name == Enemies[i].name)
						{
							shortestDistanceToLosTarget = distanceToEnemy;
							enemyToGoAfter = Enemies[i];
						}
					}
				}
			}
		}

		Target = enemyToGoAfter;
	}

	private void UpdateDestination()
	{
		if (Target == null)
		{
			if (_isBadGuy)
			{
				if (NavMeshDestination.name != gameObject.name)
				{
					SetDestination(gameObject);
				}
			}
			else
			{
				if (EndOfLevel != null)
				{
					if (NavMeshDestination.name != EndOfLevel.name)
					{
						SetDestination(EndOfLevel);
					}
				}
				else
				{
					SetDestination(gameObject);
				}
			}
		}
		else if (Target != null && !Target.IsDead())
		{
			float dist = Vector3.Distance(transform.position, Target.transform.position);
			if (dist < AttackRange)
			{
				if (NavMeshDestination.name != gameObject.name)
				{
					SetDestination(gameObject);
				}
			}
			else if (NavMeshDestination.name != Target.name)
			{
				SetDestination(Target.gameObject);
			}
		}

		if (NavMeshDestination.transform.position != NavMeshDestinationPosition)
		{
			SetDestination(NavMeshDestination);
		}
	}

	private void HandleAttacking()
	{
		if (Target != null && !Target.IsDead())
		{
			float dist = Vector3.Distance(transform.position, Target.transform.position);
			if (dist <= AttackRange)
			{
				if (TimeOfLastAttack + AttackSpeed < Time.timeSinceLevelLoad)
				{
					Target.TakeDamage(AttackPower);
					TimeOfLastAttack = Time.timeSinceLevelLoad;
				}
			}
		}
	}

	private void SetDestination(GameObject newGoal)
	{
		NavMeshDestination = newGoal;

		NavMeshDestinationPosition = NavMeshDestination.transform.position;

		_navMeshAgent.SetDestination(NavMeshDestinationPosition);
	}

	private List<Unit> FindOtherUnitsOfTag(string tag)
	{
		GameObject[] unitGameObjects = GameObject.FindGameObjectsWithTag(tag);

		List<Unit> units = new List<Unit>();

		for (int i = 0; i < unitGameObjects.Length; i++)
		{
			units.Add(unitGameObjects[i].GetComponent<Unit>());
		}

		return units;
	}

	public override void SetupUnitAngles(float cameraAngle)
	{
		CameraAngle = cameraAngle;

		// Update the initial rotation of the player
		var angles = Visuals.transform.rotation.eulerAngles;
		angles.x = CameraAngle;
		Visuals.transform.rotation = Quaternion.Euler(angles);

		// Now change our Y position by 25%
		var position = Visuals.transform.position;
		position.y -= (position.y * .25f);
		Visuals.transform.position = position;

		// Update the initial rotation of the player
		angles = HealthBarContainer.transform.rotation.eulerAngles;
		angles.x = CameraAngle;
		Visuals.transform.rotation = Quaternion.Euler(angles);

		// Now change our Y position by 25%
		position = HealthBarContainer.transform.position;
		position.y -= (position.y * .25f);
		Visuals.transform.position = position;
	}
}
