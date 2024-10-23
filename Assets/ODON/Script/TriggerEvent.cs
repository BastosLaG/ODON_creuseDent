using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField] private LayerMask _triggerLayers;
    [SerializeField] private UnityEvent _triggerEnterEvent, _triggerStayEvent, _triggerExitEvent;
    private GameObject _triggerColl = null;

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & _triggerLayers) != 0)
        {
            _triggerEnterEvent.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (((1 << other.gameObject.layer) & _triggerLayers) != 0)
        {
            print("stay");
            _triggerColl = other.gameObject;
            _triggerStayEvent.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & _triggerLayers) != 0)
        {
            print("exited");
            _triggerExitEvent.Invoke();
        }
    }

    public GameObject GetTriggerColl()
    {
        return _triggerColl;
    }

    public (UnityEvent, UnityEvent, UnityEvent) GetEvents()
    {
        return new ( _triggerEnterEvent, _triggerStayEvent, _triggerExitEvent);
    }
}
