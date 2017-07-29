using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using System.Text;
using System.IO;

public class MessageExchange : PunBehaviour
{

    PhotonView myPhotonView;
    PhotonStream stream;
    PhotonMessageInfo mess;
    Vector3 pos;
    GameObject ball;

	// Use this for initialization
	void Start () {
       // ball = 
        PhotonNetwork.ConnectUsingSettings("v1.0");
        RoomOptions op = new RoomOptions() { maxPlayers = 2 };
        PhotonNetwork.JoinOrCreateRoom("test", op, TypedLobby.Default);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            StreamReader reader = new StreamReader("message.txt");
            stream.SendNext(reader.ReadLine());
            stream.SendNext(reader.ReadLine());
            stream.SendNext(reader.ReadLine());
        }
        else
        {
            pos.x = (float)stream.ReceiveNext();
            pos.y = (float)stream.ReceiveNext();
            pos.z = (float)stream.ReceiveNext();


        }
    }
}
