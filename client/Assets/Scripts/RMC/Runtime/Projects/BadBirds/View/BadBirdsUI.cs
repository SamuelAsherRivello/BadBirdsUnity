using RMC.Projects.Runtime.Managers;
using RMC.Projects.UpsetDucksGame.Model;
using UnityEngine;
using UnityEngine.UI;

namespace RMC.Projects.Runtime.BadBirdsGame.View
{
	public class BadBirdsUI : MonoBehaviour
	{
		/// <summary>
		/// Setup "Singleton" Design Pattern
		/// See http://bit.ly/Unity_Singleton
		/// </summary>
		private static BadBirdsUI _instance;
		public static BadBirdsUI Instance { get { return _instance; } }

		[SerializeField]
		private Text _asteroidsText = null;

		[SerializeField]
		private Text _scoreText = null;

		[SerializeField]
		private Text _resultText = null;

		protected void Awake()
		{
			_instance = this;
		}

		public void ShowAsteroids(int value)
		{
			_asteroidsText.text = string.Format("Asteroids: {0:00}", value);
		}

		public void ShowScore(int currentScore, int totalScore)
		{
			_scoreText.text = string.Format("Score: {0:00}/{1:00}", currentScore, totalScore);
		}

		public void ShowResult(bool isWin)
		{
			if (isWin)
			{
				_resultText.text = string.Format("You Win!");
				SoundManager.Instance.PlayAudioClip(BadBirdsConstants.WinSound);
			}
			else
			{
				_resultText.text = string.Format("You Lose!");
				SoundManager.Instance.PlayAudioClip(BadBirdsConstants.LoseSound);
			}
		}
	}
}