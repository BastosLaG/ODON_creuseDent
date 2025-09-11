using UnityEngine;

public class TakeGlove : MonoBehaviour
{
    [SerializeField] private Animation[] gloveAnim;

    // Intervalle et délai configurables depuis l'inspecteur
    [SerializeField] private float startDelay = 1f;
    [SerializeField] private float DestroyRate = 2f;

    private void PlayTakeGlove()
    {
        foreach (Animation anim in gloveAnim)
        {
            anim.Play();
        }
    }

    public void TakeGloves()
    {
        PlayTakeGlove();
        Invoke(nameof(PlayTakeGlove), startDelay);
        Invoke(nameof(Destroy), DestroyRate);
    }
}