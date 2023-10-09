using Photon.Pun;
using UnityEngine;

namespace PhotonPun2VRTemplate.Network.Scene
{
    public class NetworkObjectSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject networkObjectPrefab;
        [SerializeField] private Transform spawnPoint;
        
        public void SpawnNetworkObject()
        {
            PhotonNetwork.Instantiate(networkObjectPrefab.name, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
