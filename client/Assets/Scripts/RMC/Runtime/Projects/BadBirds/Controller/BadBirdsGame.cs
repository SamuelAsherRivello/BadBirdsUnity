using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RMC.Projects.Runtime.Managers;
using RMC.Projects.Runtime.BadBirdsGame.View;
using RMC.Projects.UpsetDucksGame.Model;
using RMC.Projects.UpsetDucksGame.View;
using UnityEngine;

namespace RMC.Projects.UpsetDucksGame.Controller
{
	public class BadBirdsGame : MonoBehaviour
	{
		/// <summary>
		/// Setup "Singleton" Design Pattern
		/// See http://bit.ly/Unity_Singleton
		/// </summary>
		private static BadBirdsGame _instance;
		public static BadBirdsGame Instance { get { return _instance; } }

		public int Score
		{
			get
			{
				return _score;
			}
			set
			{
				_score = value;
				if (BadBirdsUI.Instance != null)
				{
					BadBirdsUI.Instance.ShowScore(_score, _upsetDuckCount);
				}
				
			}
		}

		public int Asteroids
		{
			get
			{
				return _asteroids;
			}
			set
			{
				_asteroids = value;
				if (BadBirdsUI.Instance != null)
				{
					BadBirdsUI.Instance.ShowAsteroids(_asteroids);
				}
			}
		}

		[SerializeField]
		private GameObject _explosionPrefab = null;

		[SerializeField]
		private GameObject _worldItemParent = null;

		public List<WorldItem> _worldItems = new List<WorldItem>();

		private int _asteroids = 0;
		private int _score = 0;
		private bool _isGameOver = false;
		private int _upsetDuckCount = 0;
		private Asteroid _currentAsteroid = null;

		protected void Awake()
		{
			_instance = this;
		}

		protected void Start()
		{
			
			Physics.sleepThreshold = BadBirdsConstants.PhysicsSleepThreshold;

			// Reset values
			_isGameOver = false;
			_currentAsteroid = null;
			_upsetDuckCount = 0;

			// Create new list of worldItems
			_worldItems = _worldItemParent.GetComponentsInChildren<WorldItem>().ToList();
			foreach (WorldItem worldItem in _worldItems)
			{
				if (worldItem.gameObject.tag == BadBirdsConstants.BirdTag)
				{
					// Count how many upsetducks so we know how many we must 'hit'
					_upsetDuckCount++;
				}
			}

			Score = 0;
			Asteroids = BadBirdsConstants.MaxAsteroidsPerGame;

			// Start the catapult
			AddAsteroid();
		}

		protected void Update()
		{
			if (_isGameOver)
			{
				return;
			}

			if (_currentAsteroid != null)
			{
				if (_currentAsteroid.IsReleased && 
					_currentAsteroid.Rigidbody2D.IsSleeping())
				{
					_currentAsteroid = null;
					AddAsteroid();
				}
			}

			foreach (WorldItem worldItem in _worldItems)
			{
				if (worldItem.gameObject.tag == BadBirdsConstants.BirdTag)
				{
					
					if (worldItem.IsAlive && worldItem.Health <= 0)
					{
						worldItem.IsAlive = false;
						Score += BadBirdsConstants.PointsPerUpsetDuck;
					}
				}
			}

			if (Score >= _upsetDuckCount)
			{
				if (BadBirdsUI.Instance != null)
				{
					BadBirdsUI.Instance.ShowResult(true);
				}
				_isGameOver = true;
			}
		}

		private void AddAsteroid()
		{
			if (Asteroids > 0)
			{
				if (Catapult.Instance != null)
				{
					_currentAsteroid = Catapult.Instance.AddAsteroid();
				}
			}
			else
			{
				BadBirdsUI.Instance.ShowResult(false);
				_isGameOver = true;
			}
		}

		public void DestroyCrate(Crate crate)
		{
			/////////////////////////////
			//1. Play Sound
			/////////////////////////////
			SoundManager.Instance.PlayAudioClip(BadBirdsConstants.ExplosionSound);

			/////////////////////////////
			//2. Create Explosion
			/////////////////////////////
			GameObject explosion = Instantiate(_explosionPrefab);
			explosion.transform.position = crate.transform.position;

			_worldItems.Remove(crate.gameObject.GetComponent<WorldItem>());

			StartCoroutine(DestroyGameObjectAfterXSeconds(crate.gameObject, 
				BadBirdsConstants.CrateDestroyDelay));

			StartCoroutine(DestroyGameObjectAfterXSeconds(explosion.gameObject, 
				BadBirdsConstants.ExplosionDestroyDelay));
		}

		private IEnumerator DestroyGameObjectAfterXSeconds(GameObject go, float seconds)
		{
			/////////////////////////////
			//3. Destroy Explosion after a few seconds
			/////////////////////////////
			yield return new WaitForSeconds(seconds);
			Destroy(go);
		}
	}
}