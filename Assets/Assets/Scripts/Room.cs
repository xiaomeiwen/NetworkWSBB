using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using UnityEngine.UI;

public class Room : MonoBehaviour {

    public GameObject loginPanel;
    public GameObject lobbyPanel;
    public GameObject roomPanel;

    public Button loginButton;
    public Button exitButton;
    public Button createRoomButton;
    public Button leaveRoomButton;
    public Button readyButton;
    
    public Text username;

    void Start ()
    {
        SetLoginPanelActive();

        if(!PhotonNetwork.connected)
        {
            username.text = PlayerPrefs.GetString("Username");
        }
    }

    public void SetLoginPanelActive ()
    {
        lobbyPanel.SetActive(false);
        if (roomPanel != null)
        {
            roomPanel.SetActive(false);
        }

        loginPanel.SetActive(true);
        loginButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);        
    }

    public void SetLobbyPanelActive ()
    {
        loginPanel.SetActive(false);
        loginButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);

        lobbyPanel.SetActive(true);
    }

    public void SetRoomPanelActive ()
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
    }
    /// 
    /// Login Panel part
    /// 
    public void ClickLoginButton ()
    {
        if (username.text == "")
        {
            username.text = "Guest" + Random.Range(1, 9999);
        }
        PhotonNetwork.player.name = username.text;
        PlayerPrefs.SetString("Username", username.text);

        if (!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings("1.0");

        SetLobbyPanelActive();
    }

    public void ClickExitButton ()
    {
        Application.Quit();
    }

    public override void OnJoinedLobby ()
    {
        SetLobbyPanelActive();
    }

    public override void OnConnectionFail (DisconnectCause cause)
    {
        SetLoginPanelActive();
        Debug.Log("Network Error" + cause);
    }
    /// 
    /// Lobby Panel part
    /// 
    public void ClickCreateRoomButton ()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.maxPlayers = 2;         //maximum capacity is 2 per room
        PhotonNetwork.CreateRoom(PhotonNetwork.player.name, roomOptions, TypedLobby.Default);
        SetRoomPanelActive();
    }

    public void ClickJoinRoomButton(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
        SetRoomPanelActive();
    }
    public override void OnJoinedRoom ()
    {
        foreach (PhotonPlayer pp in PhotonNetwork.otherPlayers )
        {
            if(pp.name.Equals(PhotonNetwork.playerName))
            {
                string newName = PhotonNetwork.playerName + "_2";
                PhotonNetwork.playerName = newName;
            }            
        }

        SetRoomPanelActive();
    }

    //Implement later after discussion
    //public override void OnReceivedRoomListUpdate()
    //{

    //}


    /// 
    /// Room Panel part
    /// 



}
