using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizedWallsChunkManager : MonoBehaviour
{
	List<GameObject> PotentialWalls = new List<GameObject>();
	
    void Awake()
	{
		foreach (Transform child in transform)
		{
			PotentialWalls.Add(child.gameObject);
		}

		for (int i = 0; i < PotentialWalls.Count; i++)
		{
			PotentialWalls[i].SetActive(false);
		}
	}

	public void RandomizeWalls(int seed)
	{
		for (int i = 0; i < PotentialWalls.Count; i++)
		{
			PotentialWalls[i].SetActive(false);
		}

		Random.InitState(seed);

		int numberOfWallsToHave = Random.Range(1, PotentialWalls.Count - 1);

		List<int> chosenWallIndeces = new List<int>();

		while (chosenWallIndeces.Count != numberOfWallsToHave)
		{
			int potentialWallIndex = Random.Range(0, PotentialWalls.Count);

			if (!chosenWallIndeces.Contains(potentialWallIndex))
			{
				chosenWallIndeces.Add(potentialWallIndex);
			}
		}

		for (int i = 0; i < chosenWallIndeces.Count; i++)
		{
			PotentialWalls[chosenWallIndeces[i]].SetActive(true);
		}
	}
}
