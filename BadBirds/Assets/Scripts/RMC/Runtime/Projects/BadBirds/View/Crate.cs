using RMC.Projects.Runtime.Managers;
using RMC.Projects.UpsetDucksGame.Model;
using UnityEngine;

namespace RMC.Projects.UpsetDucksGame.View
{
	public class Crate : MonoBehaviour
	{
		[SerializeField]
		private WorldItem _worldItem = null;

		[SerializeField]
		private SpriteRenderer _spriteRenderer = null;

		//Disable a harmless warning shown by the code editor
#pragma warning disable 0414 // The field is assigned but its value is never used
		[SerializeField]
		private Sprite _idleSprite = null;
#pragma warning restore
		
		[SerializeField]
		private Sprite _hitSprite = null;

		protected void Start()
		{
			_worldItem.OnHealthChange.AddListener(WorldItem_OnHealthChange);
		}

		private void WorldItem_OnHealthChange(float delta)
		{
			if (_worldItem.Health <= 0)
			{
				SoundManager.Instance.PlayAudioClip(BadBirdsConstants.WinSound);

				Controller.BadBirdsGame.Instance.DestroyCrate(this);
			}	
			else
			{
				if (delta > BadBirdsConstants.MinCrateHealthChangeForReaction)
				{
					_spriteRenderer.sprite = _hitSprite;
					SoundManager.Instance.PlayAudioClip(BadBirdsConstants.CollisionSound);
				}
			}
		}
	}
}