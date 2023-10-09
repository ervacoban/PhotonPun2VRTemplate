using Photon.Pun;
using PhotonPun2VRTemplate.XR;
using UnityEngine;
using UnityEngine.XR;

namespace PhotonPun2VRTemplate.Network.Client
{
    public class NetworkClient : MonoBehaviour
    {
        [SerializeField] private Transform clientHead, clientLeftHand, clientRightHand;
        [SerializeField] private Animator leftHandAnimator, rightHandAnimator;
        private Transform _xrOriginHead, _xrOriginLeftHand, _xrOriginRightHand;
        private PhotonView _photonView;
        private bool _isXrOriginInitialized;

        private void Awake()
        {
            _isXrOriginInitialized = false;
            _photonView = GetComponent<PhotonView>();
        }

        private void Start()
        {
            if (_photonView.IsMine)
            {
                foreach (var item in GetComponentsInChildren<Renderer>())
                {
                    item.enabled = false;
                }

                InitializeXrOrigin();
            }
        }

        private void InitializeXrOrigin()
        {
            var xrOriginReferences = XROriginReferences.Instance;
            _xrOriginHead = xrOriginReferences.GetXrOriginHead();
            _xrOriginLeftHand = xrOriginReferences.GetXrOriginLeftHand();
            _xrOriginRightHand = xrOriginReferences.GetXrOriginRightHand();
            _isXrOriginInitialized = true;
        }

        private void Update()
        {
            if (!_photonView.IsMine)
            {
                return;
            }
            
            if (!_isXrOriginInitialized)
            {
                InitializeXrOrigin();
                return;
            }

            LocalPlayer();
        }

        private void LocalPlayer()
        {
            try
            {
                MapPosition(clientHead, _xrOriginHead);
                MapPosition(clientLeftHand, _xrOriginLeftHand);
                MapPosition(clientRightHand, _xrOriginRightHand);
                UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.LeftHand), leftHandAnimator);
                UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.RightHand), rightHandAnimator);
            }
            catch
            {
                _isXrOriginInitialized = false;
            }
        }

        private void MapPosition(Transform target, Transform rigTransform)
        {
            target.position = rigTransform.position;
            target.rotation = rigTransform.rotation;
        }

        private static readonly int Trigger = Animator.StringToHash("Trigger");
        private static readonly int Grip = Animator.StringToHash("Grip");
        
        private void UpdateHandAnimation(InputDevice targetDevice, Animator handAnimator)
        {
            if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out var triggerValue))
            {
                handAnimator.SetFloat(Trigger, triggerValue);
            }
            else
            {
                handAnimator.SetFloat(Trigger, 0);
            }

            if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out var gripValue))
            {
                handAnimator.SetFloat(Grip, gripValue);
            }
            else
            {
                handAnimator.SetFloat(Grip, 0);
            }
        }
    }
}
