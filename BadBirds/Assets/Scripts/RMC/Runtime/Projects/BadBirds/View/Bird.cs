using System.Collections;
using RMC.Projects.UpsetDucksGame.Model;
using UnityEngine;

namespace RMC.Projects.UpsetDucksGame.View
{
	public class Bird : MonoBehaviour
	{
		[SerializeField]
		private WorldItem _worldItem = null;

		[SerializeField]
		private SpriteRenderer _spriteRenderer = null;

		[SerializeField]
		private Sprite _idleSprite = null;

		[SerializeField]
		private Sprite _hitSprite = null;

		[SerializeField]
		private Sprite _deadSprite = null;

		protected void Start()
		{
			_worldItem.OnHealthChange.AddListener(WorldItem_OnHealthChange);
		}

		private void WorldItem_OnHealthChange(float delta)
		{
			if (!_worldItem.IsAlive)
			{
				return;
			}

			if (_worldItem.Health <= 0)
			{
				_spriteRenderer.sprite = _deadSprite;

			}
			else
			{
				if (delta > BadBirdsConstants.MinUpsetDuckHealthChangeForReaction)
				{
					StartCoroutine(SetSpriteTemporarilyCoroutine(_hitSprite));
				}
			}
		}

		private IEnumerator SetSpriteTemporarilyCoroutine(Sprite sprite)
		{
			_spriteRenderer.sprite = sprite;
			yield return new WaitForSeconds(BadBirdsConstants.UpsetDuckSpriteFlickerDelay);

			if (_worldItem.IsAlive)
			{
				_spriteRenderer.sprite = _idleSprite;
			}
		}
	}
}