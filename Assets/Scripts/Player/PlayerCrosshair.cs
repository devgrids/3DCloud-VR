using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Player
{
    [RequireComponent(typeof(PlayerComponent))]
    public class PlayerCrosshair : MonoBehaviour, IPlayerCrosshair
    {
        [SerializeField] private PlayerCrosshairConfiguration configuration;
        private PlayerComponent playerComponent;
        private Image crosshairObject;

        public PlayerCrosshairConfiguration Configuration { get => configuration; set => configuration = value; }

        private void Awake()
        {
            playerComponent = PlayerComponent.sharedInstance;
            crosshairObject = GetComponentInChildren<Image>();
        }
        
        private void Start()
        {
            if (Configuration != null) 
            {
                crosshairObject.sprite = Configuration.CrosshairImage;
                crosshairObject.color = Configuration.CrosshairColor;
                
                if (Configuration.LockCursor)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
            else
            {
                crosshairObject.gameObject.SetActive(false);
            }
        }
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject gameObject;
                if (GetGameObjectFromRaycast(1 << 5, 20.0f, out gameObject))
                {
                    ExecuteEventsFromGameObject(ref gameObject);
                }
            }
        }
        
        public void ExecuteEventsFromGameObject(ref GameObject gameObject)
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);

            IPointerClickHandler clickHandler = gameObject.GetComponent<IPointerClickHandler>();
            IPointerEnterHandler enterHandler = gameObject.GetComponent<IPointerEnterHandler>();
            IPointerExitHandler exitHandler = gameObject.GetComponent<IPointerExitHandler>();

            if (clickHandler == null || enterHandler == null || exitHandler == null) return;

            clickHandler.OnPointerClick(pointerEventData);
            enterHandler.OnPointerEnter(pointerEventData);
            exitHandler.OnPointerExit(pointerEventData);
        }

        public bool GetGameObjectFromRaycast(int layerMask, float maxDistance, out GameObject gameObject)
        {
            RaycastHit hit;
            gameObject = null;

            if (Physics.Raycast(playerComponent.Camera.transform.position, playerComponent.Camera.transform.TransformDirection(Vector3.forward), out hit, maxDistance, layerMask))
            {
                gameObject = hit.transform.gameObject;
                return true;
            }
            return false;
        }
    }
}