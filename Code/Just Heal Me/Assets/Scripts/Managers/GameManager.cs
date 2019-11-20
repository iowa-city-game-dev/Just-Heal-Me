using Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
		public int CurrentLevel = -1;

		public SoundManager Sound;

		string _areaName = "";

		Text _textAreaName;
		Text TextAreaName
		{
			get
			{
				if (_textAreaName == null)
				{
					_textAreaName = GameObject.FindGameObjectWithTag("TextAreaName").GetComponent<Text>();
				}

				return _textAreaName;
			}
		}

		public void SetLevel(int level)
		{
			CurrentLevel = level;

			UpdateTextAreaName();
		}

		public void SetAreaNameText(string newAreaNameText)
		{
			_areaName = newAreaNameText;

			UpdateTextAreaName();
		}

		public void UpdateTextAreaName()
		{
			string tempAreaName = _areaName;

			if (tempAreaName != "Town")
			{
				tempAreaName += " " + (CurrentLevel + 1);
			}

			TextAreaName.text = tempAreaName;
		}

		public void MoveToNextLevel()
		{
			if (CurrentLevel == -1 && !SceneManager.GetSceneByName("Town").isLoaded)
			{
				CurrentLevel = 0;
			}

			if (CurrentLevel == -1)
			{
				SceneManager.UnloadSceneAsync("Town");
				SceneManager.LoadScene("Game", LoadSceneMode.Additive);
			}
			else
			{
				PrepareForNextLevel();
			}

			SetLevel(CurrentLevel + 1);
		}

		private void PrepareForNextLevel()
		{
			FindObjectOfType<LevelManager>().RandomizeLevel();

			GameObject player = GameObject.FindGameObjectWithTag("Player");
			player.GetComponent<Unit>().Reset();

			GameObject[] goodGuys = GameObject.FindGameObjectsWithTag("GoodGuy");
			for (int i = 0; i < goodGuys.Length; i++)
			{
				goodGuys[i].GetComponent<Unit>().Reset();
			}

			GameObject[] badGuys = GameObject.FindGameObjectsWithTag("BadGuy");
			for (int i = 0; i < badGuys.Length; i++)
			{
				badGuys[i].GetComponent<Unit>().Reset();
			}
		}
	}
}
