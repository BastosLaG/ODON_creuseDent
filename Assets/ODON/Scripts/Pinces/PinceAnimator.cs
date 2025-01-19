using UnityEngine;

public class PinceAnimator : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        PinceClose();
    }

    public void PinceOpen(){
        animator.SetBool("Close", false);
        animator.SetBool("Open", true);
    }
    public void PinceClose(){
        animator.SetBool("Close", true);
        animator.SetBool("Open", false);
    }
}
