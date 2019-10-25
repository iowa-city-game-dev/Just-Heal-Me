using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	[SerializeField] string LevelName;
	[SerializeField] int Seed;

	List<RandomizedWallsChunkManager> WallChunks = new List<RandomizedWallsChunkManager>();

	void Awake()
	{
		GameObject[] wallChunks = GameObject.FindGameObjectsWithTag("WallChunk");
		for (int i = 0; i < wallChunks.Length; i++)
		{
			WallChunks.Add(wallChunks[i].GetComponent<RandomizedWallsChunkManager>());
		}
	}

	void Start()
    {
		RandomizeLevel();
	}
	
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Return))
		{
			RandomizeLevel();
		}
    }

	private void RandomizeLevel()
	{
		Seed = Random.Range(0, int.MaxValue);

		for (int i = 0; i < WallChunks.Count; i++)
		{
			WallChunks[i].RandomizeWalls(Seed + i);
		}
	}
}
