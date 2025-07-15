using ODON.InteractableObject;
using UnityEngine;

namespace ODON
{
    public class HeadInteractor : MonoBehaviour
    {
        public float m_MaxDistance = 10f;
        public LayerMask m_Mask = -1;

        [Header("UI")]
        [SerializeField] private Canvas canvas;
        private UI.UIGazeReticle reticle;

        [Header("Teleportation")]
        [SerializeField] private Transform player;

        private bool actionComplete = false;
        private Transform currentTarget;
        private Transform lastHoveredTarget = null;

        void Start()
        {
            if (canvas != null)
                reticle = canvas.GetComponentInChildren<UI.UIGazeReticle>();

            if (reticle == null)
            {
                reticle = GetComponent<UI.UIGazeReticle>();
                if (reticle == null)
                    Debug.LogError("No reticle found!");
            }

            if (player == null)
                Debug.LogWarning("Player transform is not assigned.");
        }

        void FixedUpdate()
        {
            Vector3 direction = transform.forward;
            Ray ray = new(transform.position, direction);

            if (Physics.Raycast(ray, out RaycastHit hit, m_MaxDistance, m_Mask, QueryTriggerInteraction.Ignore))
            {
                if (lastHoveredTarget != hit.transform)
                {
                    Debug.Log($"Hover Begin: {hit.transform.name}");
                    lastHoveredTarget = hit.transform;
                    lastHoveredTarget.GetComponent<InteractAction>()?.HeadHoverEventBegin();
                }

                Debug.DrawRay(ray.origin, ray.direction * m_MaxDistance, Color.green);
                reticle.ActiveProgressReticle(true);
                currentTarget = hit.transform;

                if (!actionComplete && reticle.ReticleProgress.fillAmount >= 1.0f)
                {
                    actionComplete = true;

                    // implement all logic here
                    if (currentTarget.CompareTag("Teleporter") && player != null)
                    {
                        TeleportTo(currentTarget.position);
                    }
                    else if (currentTarget.CompareTag("Grab"))
                    {
                        currentTarget.GetComponent<InteractAction>().HeadInteract();
                    }
                    else
                    {
                        Debug.LogError($"No Action set for object : {currentTarget.name}");
                    }
                }
            }
            else
            {
                if (lastHoveredTarget != null)
                {
                    lastHoveredTarget.GetComponent<InteractAction>()?.HeadHoverEventEnd();
                    lastHoveredTarget = null;
                }
                Debug.DrawRay(ray.origin, ray.direction * m_MaxDistance, Color.red);
                reticle.ActiveProgressReticle(false);
                actionComplete = false;
                currentTarget = null;
            }
        }

        void TeleportTo(Vector3 destination)
        {
            player.position = destination;
        }
    }
}
