using UnityEngine;
using System.Collections;

public class RandomMatchmaker : Photon.PunBehaviour 
{
	void Start()
	{
		PhotonNetwork.ConnectUsingSettings("0.1");
	}
	
	void OnGUI()
	{
		if (Debug.isDebugBuild) 
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

	}

	void addRoomInfoGUI()
	{
		GUILayout.Label ("Room:" + PhotonNetwork.room.name);
		foreach (var player in PhotonNetwork.playerList)
		{
			GUILayout.Label ("Players in room: " + player.name);
		}
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
		//if null is passed a random GUID room ID is created HG
	}

	void ShowConnectingGUI()
	{
		GUI.contentColor = Color.black;
		GUILayout.Label("Connecting to Photon server.");
		GUILayout.EndArea();
	}
}
