using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PhotonPun2VRTemplate.Network.Components
{
    [RequireComponent(typeof(XRBaseInteractable)), RequireComponent(typeof(PhotonView))]
    public class ClientInteractableOwnership : MonoBehaviour
    {
        private XRBaseInteractable _interactable;
        private PhotonView _photonView;

        private void Awake()
        {
            _interactable = GetComponent<XRBaseInteractable>();
            _photonView = GetComponent<PhotonView>();
            if (_interactable == null)
            {
#if UNITY_EDITOR
                Debug.LogError("Interactable not found on " + gameObject.name);
#endif
                enabled = false;
            }
        }

        private void Start()
        {
            if (_photonView.OwnershipTransfer is not OwnershipOption.Takeover)
            {
                _photonView.OwnershipTransfer = OwnershipOption.Takeover;
            }
        }

        private void OnEnable()
        {
            _interactable.selectEntered.AddListener(TransferOwnership);
        }
        
        private void OnDisable()
        {
            _interactable.selectEntered.RemoveListener(TransferOwnership);
        }
        
        private void TransferOwnership(SelectEnterEventArgs args)
        {
            if (_photonView.IsMine)
            {
                return;
            }
            
            _photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
        }
    }
}
