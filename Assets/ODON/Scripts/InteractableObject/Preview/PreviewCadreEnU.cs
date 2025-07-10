using UnityEngine;
using System.Collections;

namespace ODON.InteractableObject
{    
    public class CadreEnUPreview : MonoBehaviour, Interface.IInteractWithHeadInteractor
    {
        [SerializeField] private Transform cadreEnUFinalTransform;
        [SerializeField] private Material cadreEnUFinalMat;

        private Coroutine cadreEnUPreview = null;

        // A appeller quand on attrape l'object pour afficher la pr�visualisation
        public void StartToCompareDistance()
        {
            // ??= -> Change la valeur si elle est nulle, sinon, laisse la valeur par d�faut �quivaut � "if (cadreEnUPreview == null)...".
            cadreEnUPreview ??= StartCoroutine(CompareCadreDistances(0.5f));
        }

        private IEnumerator CompareCadreDistances(float compareInterval)
        {
            while (true)
            {
                CompareDistance();
                yield return new WaitForSeconds(compareInterval);
            }
        }

        // Regarde qu'elle preview est la plus proche du object et lui met la pr�visualisation de l'object
        private void CompareDistance()
        {
            float minCadreDistance = Vector3.Distance(transform.position, cadreEnUFinalTransform.position);
            if (minCadreDistance < 0.5f)
            {
                cadreEnUFinalTransform.gameObject.SetActive(true);
            }
        }

        // Active l'objet qui est plac� � l'endroit voulu (quand on relache la gachette)
        public void PlaceObject()
        {
            float minCadreDistance = Vector3.Distance(transform.position, cadreEnUFinalTransform.position);
            if (minCadreDistance < 0.5f)
            {
                cadreEnUFinalTransform.GetComponent<MeshRenderer>().material = cadreEnUFinalMat;
                Destroy(gameObject);
            }
        }

        public void OnHeadInteract()
        {
            // Todo : Preview
            throw new System.NotImplementedException();
        }
    }
}