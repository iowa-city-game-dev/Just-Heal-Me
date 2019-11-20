using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
	enum MainMenuState
	{
		Default,
		Credits
	}

	MainMenuState State = MainMenuState.Default;

	public float StateTransitionSpeed;

	public GameObject ThroughTreeSprite;
	private float BackgroundStartingY;
	public float BackgroundEndingY;

	private float BackgroundDefaultX;
	public float BackgroundCreditsX;

	public float BackgroundMovingSpeed;

	private bool BackgroundIsMovingDown = false;

	public List<GameObject> DefaultItems = new List<GameObject>();
	private float DefaultItemsStartingX;
	private float DefaultItemsCreditsX;

	public List<GameObject> CreditsItems = new List<GameObject>();
	private float CreditsItemsCreditsX;
	private float CreditsItemsDefaultX;

	// Start is called before the first frame update
	void Start()
	{
		BackgroundStartingY = ThroughTreeSprite.transform.position.y;
		BackgroundDefaultX = ThroughTreeSprite.transform.position.x;

		DefaultItemsStartingX = DefaultItems[0].transform.position.x;
		DefaultItemsCreditsX = DefaultItemsStartingX - 1000f;

		CreditsItemsCreditsX = CreditsItems[0].transform.position.x;
		CreditsItemsDefaultX = CreditsItemsCreditsX + 1000f;

		for (int i = 0; i < CreditsItems.Count; i++)
		{
			CreditsItems[i].transform.position = new Vector3(CreditsItemsDefaultX, CreditsItems[i].transform.position.y, CreditsItems[i].transform.position.z);
		}
	}

    // Update is called once per frame
    void Update()
    {
		float backgroundPosX = BackgroundDefaultX;
		float backgroundPosY = ThroughTreeSprite.transform.position.y;
		float defaultItemsPosX = DefaultItemsStartingX;
		float creditsItemsPosX = CreditsItemsDefaultX;

		if (State == MainMenuState.Credits)
		{
			backgroundPosX = BackgroundCreditsX;
			defaultItemsPosX = DefaultItemsCreditsX;

			creditsItemsPosX = CreditsItemsCreditsX;
		}

		if (ThroughTreeSprite.transform.position.y < BackgroundEndingY)
		{
			BackgroundIsMovingDown = false;
		}
		else if (ThroughTreeSprite.transform.position.y > BackgroundStartingY)
		{
			BackgroundIsMovingDown = true;
		}

		if (BackgroundIsMovingDown)
		{
			backgroundPosY -= BackgroundMovingSpeed;
		}
		else
		{
			backgroundPosY += BackgroundMovingSpeed;
		}

		ThroughTreeSprite.transform.position = new Vector3(ThroughTreeSprite.transform.position.x, backgroundPosY, ThroughTreeSprite.transform.position.z);
		ThroughTreeSprite.transform.position = Vector3.Lerp(ThroughTreeSprite.transform.position,
															new Vector3(backgroundPosX, ThroughTreeSprite.transform.position.y, ThroughTreeSprite.transform.position.z),
															StateTransitionSpeed);

		for (int i = 0; i < DefaultItems.Count; i++)
		{
			DefaultItems[i].transform.position = Vector3.Lerp(DefaultItems[i].transform.position,
														new Vector3(defaultItemsPosX, DefaultItems[i].transform.position.y, DefaultItems[i].transform.position.z),
														StateTransitionSpeed);
		}

		for (int i = 0; i < CreditsItems.Count; i++)
		{
			CreditsItems[i].transform.position = Vector3.Lerp(CreditsItems[i].transform.position,
														new Vector3(creditsItemsPosX, CreditsItems[i].transform.position.y, CreditsItems[i].transform.position.z),
														StateTransitionSpeed);
		}
	}

	public void StartGame()
	{
		SceneManager.LoadScene("Managers");
		SceneManager.LoadScene("Town", LoadSceneMode.Additive);
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	public void Credits()
	{
		State = MainMenuState.Credits;
	}

	public void Back()
	{
		State = MainMenuState.Default;
	}
}
