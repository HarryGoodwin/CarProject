using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public GameObject PlayerPrefab;
	public Camera mainCamera;
	public GameObject spawnPoint1;
	public GameObject spawnPoint2;
	
	public void StartGame()
	{
		GameObject spawnPosition = (PhotonNetwork.player.ID == 1) ? spawnPoint1 : spawnPoint2; // just for testing
		GameObject playerCar = PhotonNetwork.Instantiate(PlayerPrefab.name, spawnPosition.transform.position, Quaternion.identity, 0);

		CarController controller = playerCar.GetComponent<CarController>();
		controller.enabled = true;
	}
}
