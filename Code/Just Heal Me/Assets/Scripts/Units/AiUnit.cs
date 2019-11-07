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

	protected Unit Target = null;

	float _initialSpeed = 0f;

	#region -----[ Unity Lifecycle ]-------------------------------------------

	public override void Awake()
	{
		Animator = Visuals.GetComponent<Animator>();
		Animation = Visuals.GetComponent<Animation>();
	}

	public override void Start()
	{
		base.Start();

		_navMeshAgent = GetComponent<NavMeshAgent>();

		if (_isBadGuy)
		{
			Allies = FindOtherUnitsOfTag("BadGuy");
			Enemies = FindOtherUnitsOfTag("GoodGuy");
			Enemies.AddRange(FindOtherUnitsOfTag("Player"));

			SetDestination(gameObject);
		}
		else
		{
			Allies = FindOtherUnitsOfTag("GoodGuy");
			Allies.AddRange(FindOtherUnitsOfTag("Player"));
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

		_initialSpeed = _navMeshAgent.speed;

		Visuals.transform.SetParent(null);
	}
	
	public override void Update()
	{
		base.Update();

		if (!IsDead())
		{
			UpdateSpeed();

			UpdateTarget();
			UpdateDestination();
			UpdateVisualFacingDirection();

			HandleAttacking();
		}

		Visuals.transform.position = transform.position;
	}

	#endregion

	#region -----[ Private Functions ]------------------------------------------

	private void UpdateSpeed()
	{
		if (IsStunned())
		{
			_navMeshAgent.speed = 0;
		}
		else
		{
			_navMeshAgent.speed = _initialSpeed;
		}
	}

	private void UpdateTarget()
	{
		if (IsStunned())
		{
			Target = null;
			return;
		}

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
			if (IsInAttackRangeOfTarget())
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

	private void UpdateVisualFacingDirection()
	{
		bool _facingRight = true;

		float horizontal = _navMeshAgent.velocity.x;

		if (Target != null && IsInAttackRangeOfTarget())
		{
			// we want to maintain our facing direction
			if (Target.transform.position.x < transform.position.x)
			{
				_facingRight = false;
			}
			else
			{
				_facingRight = true;
			}
		}
		else
		{
			// we want to maintain our facing direction
			if (horizontal < 0)
			{
				_facingRight = false;
			}
			else if (horizontal > 0)
			{
				_facingRight = true;
			}
		}

		// Which way are we running?
		Visuals.transform.rotation = Quaternion.Euler(_facingRight ? CameraAngle : CameraAngle * -1, _facingRight ? 0 : 180, 0);
	}

	private bool IsInAttackRangeOfTarget()
	{
		if (Target != null)
		{
			return AttackRange >= Vector3.Distance(transform.position, Target.transform.position);
		}

		return false;
	}

	private void HandleAttacking()
	{
		if (Target != null && !Target.IsDead())
		{
			if (IsInAttackRangeOfTarget())
			{
				if (TimeOfLastAttack + AttackSpeed < Time.timeSinceLevelLoad)
				{
					InitiateAttack();
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

	#endregion

	#region -----[ Protected Functions ]------------------------------------------

	protected override Vector3 GetVelocity()
	{
		return _navMeshAgent.velocity;
	}

	protected override void OnUnstun()
	{
		TimeOfLastAttack = Time.timeSinceLevelLoad;
	}

	protected virtual void InitiateAttack()
	{
		Target.TakeDamage(AttackPower);
		TimeOfLastAttack = Time.timeSinceLevelLoad;

		Animator.SetTrigger("Attack");
	}

	#endregion

	#region -----[ Public Functions ]------------------------------------------

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
	}

	#endregion
}
