using UnityEngine;

public class ToothEvent : MonoBehaviour
{
    [SerializeField] Material _cramponMat;

    private TriggerEvent _tEvent;

    private bool _cramponplaced = false;

    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
        _tEvent = GetComponent<TriggerEvent>();
        //STAY
        _tEvent.GetEvents().Item2.AddListener(delegate { ShowMesh(true); });
        //EXIT
        _tEvent.GetEvents().Item3.AddListener(delegate { ShowMesh(false); });
    }

    private void ShowMesh(bool isShowed)
    {
        if (!_cramponplaced)
        {
            GetComponent<MeshRenderer>().enabled = isShowed;
            
            GameObject coll = _tEvent.GetTriggerColl();
            if (_tEvent.GetTriggerColl() != null && !_tEvent.GetTriggerColl().GetComponent<SetObjectGrabable>().ItemIsSelected)
            {
                _cramponplaced = true;
                ChangeTexture(true);
                Destroy(_tEvent.GetTriggerColl().gameObject);
            }
        }
    }
    private void ChangeTexture(bool isPlaced)
    {
        print("here"+ isPlaced);
        if (isPlaced) GetComponent<MeshRenderer>().material = _cramponMat;
        GetComponent<MeshRenderer>().enabled = true;
    }
}
