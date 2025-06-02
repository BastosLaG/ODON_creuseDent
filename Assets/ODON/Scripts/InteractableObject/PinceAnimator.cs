using UnityEngine;

public class PinceAnimator : MonoBehaviour
{
    [SerializeField] private Transform cramponAnchor;
    private Animator animator;
    private Transform crampon = null;
    private void Start()
    {
        animator = GetComponent<Animator>();
        PinceClose();
    }

    public void PinceOpen(){
        animator.SetBool("Close", false);
        animator.SetBool("Open", true);
        if (crampon != null) {
            AttachCrampon();
        }
    }
    public void PinceClose(){
        animator.SetBool("Close", true);
        animator.SetBool("Open", false);
        if (crampon != null)
        {
            DetachCrampon();
        }
    }

    private void AttachCrampon()
    {
        crampon.GetComponent<Rigidbody>().isKinematic = true;
        crampon.GetComponent<Animator>().SetBool("Open", true);
        crampon.SetParent(transform);
        crampon.position = cramponAnchor.position;
        crampon.rotation = cramponAnchor.rotation;
        crampon.GetComponent<CramponPreview>().StartToCompareDistance();
    }
    private void DetachCrampon()
    {
        crampon.SetParent(null);
        crampon.GetComponent<Animator>().SetBool("Close", true);
        crampon.GetComponent<Rigidbody>().isKinematic = false;
        crampon.GetComponent<CramponPreview>().PoseCrampon();
        crampon = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Crampon"))
        {
            crampon = other.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.name.Contains("Crampon"))
        {
            crampon = null;
        }
    }
}
