using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
	[SerializeField] int Level;
	[SerializeField] string LevelName;
	[SerializeField] int Seed;

	List<RandomizedWallsChunkManager> WallChunks = new List<RandomizedWallsChunkManager>();

	bool _setLevelOnStart = false;

	void Awake()
	{
		Managers.GameManager gameManager = FindObjectOfType<Managers.GameManager>();
		if (gameManager == null)
		{
			_setLevelOnStart = true;
			SceneManager.LoadScene("Managers", LoadSceneMode.Additive);
		}

		GameObject[] wallChunks = GameObject.FindGameObjectsWithTag("WallChunk");
		for (int i = 0; i < wallChunks.Length; i++)
		{
			WallChunks.Add(wallChunks[i].GetComponent<RandomizedWallsChunkManager>());
		}
	}

	void Start()
	{
		if (_setLevelOnStart)
		{
			Managers.GameManager.Instance.SetLevel(Level);
		}

		Managers.GameManager.Instance.SetAreaNameText(LevelName);

		RandomizeLevel();
	}
	
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Return))
		{
			RandomizeLevel();
		}
    }

	public void RandomizeLevel()
	{
		Seed = Random.Range(0, int.MaxValue);

		for (int i = 0; i < WallChunks.Count; i++)
		{
			WallChunks[i].RandomizeWalls(Seed + i);
		}
	}
}
