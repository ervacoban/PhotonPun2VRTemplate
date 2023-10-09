using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace PhotonPun2VRTemplate.Network.Client
{
    public class NetworkClientSpawner : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject networkClient;

        [ContextMenu("Connect To Server")]
        public void ConnectToServer()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            var roomOptions = new RoomOptions
            {
                MaxPlayers = 20,
                IsVisible = true,
                IsOpen = true,
                PlayerTtl = 1000
            };
            PhotonNetwork.JoinOrCreateRoom("PhotonPun2VRTemplate", roomOptions, TypedLobby.Default);
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            PhotonNetwork.Instantiate(networkClient.name, Vector3.zero, Quaternion.identity);
        }
    }
}
