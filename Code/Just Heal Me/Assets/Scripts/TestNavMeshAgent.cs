using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestNavMeshAgent : MonoBehaviour
{
	NavMeshAgent _navMeshAgent;

	private GameObject Goal;
	public GameObject Goal0;
	public GameObject Goal1;
	private Vector3 GoalPosition;

    // Start is called before the first frame update
    void Start()
	{
		_navMeshAgent = GetComponent<NavMeshAgent>();

		SetGoal(Goal0);
	}

    // Update is called once per frame
    void Update()
    {
		if (GoalPosition != Goal.transform.position)
		{
			GoalPosition = Goal.transform.position;

			_navMeshAgent.SetDestination(GoalPosition);
		}
	}

	private void OnTriggerEnter(Collider coll)
	{
		if (coll.CompareTag("Goal"))
		{
			if (Goal.name == "Goal0")
			{
				SetGoal(Goal1);
			}
			else if (Goal.name == "Goal1")
			{
				SetGoal(Goal0);
			}
		}
	}

	private void SetGoal(GameObject newGoal)
	{
		Goal = newGoal;

		GoalPosition = Goal.transform.position;

		_navMeshAgent.SetDestination(GoalPosition);
	}
}
