using UnityEngine;



public class Grenade : MonoBehaviour
{
    public Rigidbody grenadePinBody;

    // Start is called before the first frame update
    private void Start()
    {
        grenadePinBody.isKinematic = true;
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void OnPinSelected()
    {
        print("Pin selected");
        grenadePinBody.WakeUp();
        grenadePinBody.isKinematic = false;
        grenadePinBody.transform.SetParent(transform);
    }
}
