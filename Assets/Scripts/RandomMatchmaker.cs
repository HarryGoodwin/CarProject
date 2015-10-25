using UnityEngine;
using System.Collections;

public class RandomMatchmaker : Photon.PunBehaviour 
{
	public GameManager gameManger;
	string roomName = "No Room";

	void Awake()
	{
		//PhotonNetwork.logLevel = NetworkLogLevel.Full;
		if (!PhotonNetwork.connected)
		{
			PhotonNetwork.ConnectUsingSettings("0.1");
		}
		
		PhotonNetwork.playerName = PlayerPrefs.GetString("playerName", "Guest" + Random.Range(1, 9999));
	}
	
	void OnGUI()
	{
		GUILayout.BeginVertical();
		
		if (!PhotonNetwork.connected)
		{
			ShowConnectingGUI();
			return;
		}
		
		GUI.contentColor = Color.black;
		GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString ());

		addPlayerNameGUI();
		addRoomInfoGUI();
		GUILayout.EndVertical();	
	}

	void addRoomInfoGUI()
	{
		GUILayout.Label ("Room:" + roomName);
		GUILayout.Label ("Players in room: " + PhotonNetwork.playerList.Length);
	}

	void addPlayerNameGUI()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label("Player name:", GUILayout.Width(150));
		PhotonNetwork.playerName = GUILayout.TextField(PhotonNetwork.playerName);
		if (GUI.changed) //Save name
		{
			PlayerPrefs.SetString("playerName", PhotonNetwork.playerName);
		}
		
		GUILayout.EndHorizontal();
	}

	public override void OnJoinedLobby()
	{
		PhotonNetwork.JoinRandomRoom();
	}

	void OnPhotonRandomJoinFailed()
	{
		Debug.Log("Can't join random room!");
		PhotonNetwork.CreateRoom(null);
	}

	public override void OnJoinedRoom ()
	{
		Debug.Log("Joined room!");
		roomName = PhotonNetwork.room.name;

		Debug.Log("Starting game");
		Debug.Log ("Actor ID = " + PhotonNetwork.player.ID);
		gameManger.StartGame();
	}

	void ShowConnectingGUI()
	{
		GUI.contentColor = Color.black;
		GUILayout.Label("Connecting to Photon server.");
		GUILayout.EndArea();
	}

	public override void OnPhotonPlayerConnected (PhotonPlayer newPlayer)
	{

	}
}
