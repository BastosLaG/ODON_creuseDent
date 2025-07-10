using UnityEngine;
using UnityEngine.UI;

namespace ODON.UI
{
    public class UIGazeReticle : MonoBehaviour
    {

        [SerializeField] private Image reticleProgress;
        public Image ReticleProgress => reticleProgress;
        public float waitTime = 3.0f;


        void Start()
        {
            reticleProgress.enabled = false;
        }

        void FixedUpdate()
        {
            if (reticleProgress.enabled == true)
            {
                reticleProgress.fillAmount += 1.0f / waitTime * Time.deltaTime;
            }
            else
            {
                reticleProgress.fillAmount = 0f;
            }
        }

        public void ActiveProgressReticle(bool activable)
        {
            reticleProgress.enabled = activable;
        }
    }
}