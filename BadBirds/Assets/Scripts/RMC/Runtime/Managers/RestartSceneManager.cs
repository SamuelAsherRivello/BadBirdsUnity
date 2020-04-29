using UnityEngine;
using UnityEngine.SceneManagement;

namespace RMC.Projects.Controllers
{
	/// <summary>
	/// Press the Spacebar to restart the scene.
	/// </summary>
	public class RestartSceneManager : MonoBehaviour
	{
		protected void Update()
		{
			// Restart Scene		------------------------------------
			if (Input.GetKey(KeyCode.Space))
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			}
		}
	}
}