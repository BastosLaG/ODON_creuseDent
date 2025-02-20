using UnityEngine;

public class BeltSystem : MonoBehaviour
{
    [SerializeField] private Vector3 localOffset = Vector3.zero;
    [SerializeField] private Vector3 localEulerRotation = Vector3.zero;
    public void SetBeltItem(GameObject item)
    {
        item.GetComponent<Rigidbody>().isKinematic = true;
        item.transform.parent = transform;
        item.transform.localPosition = localOffset;
        item.transform.eulerAngles = localEulerRotation;
    }
    public void RemoveBeltItem(GameObject item)
    {
        item.GetComponent<Rigidbody>().isKinematic = false;
        item.transform.parent = null;
    }
}
