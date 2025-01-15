using Unity.VisualScripting;
using UnityEngine;

public class PinceBrewer : MonoBehaviour
{
    // public Transform attachmentPointLeft; // Point d'attache sur votre objet
    // public Transform attachmentPointRight; // Point d'attache sur votre objet

    // public void OnTriggerEnter(Collider other)
    // {
    //     int layerMaskCrampon = LayerMask.NameToLayer("CramponNA");
    //     Debug.Log("Layer Mask Crampon = " + layerMaskCrampon);

    //     if (other.gameObject.layer == layerMaskCrampon)
    //     {
    //         Debug.Log("Je touche un crampon sans ailette");

    //         Transform cramponTransform = other.transform; // Assuming 'other' is the detected Crampon
    //         foreach (Transform child in cramponTransform)
    //         {
    //             Debug.Log("Child name: " + child.name);
    //             if (child.name == "Armature")
    //             {
    //                 Transform ArmatureTransforme = child.transform;
    //                 foreach (Transform ArmChild in ArmatureTransforme){
    //                     Debug.Log("Armature child name: " + ArmChild.name);
    //                     if (ArmChild.name == "Right"){
    //                         ArmChild.SetParent(attachmentPointRight);
    //                         ArmChild.localPosition = Vector3.zero;
    //                         ArmChild.localRotation = Quaternion.identity;
    //                     }
    //                     else if (ArmChild.name == "Left"){
    //                         ArmChild.SetParent(attachmentPointLeft);
    //                         ArmChild.localPosition = Vector3.zero;
    //                         ArmChild.localRotation = Quaternion.identity;
    //                     }
    //                 }
    //             }
    //         }

    //         // if (armature != null)
    //         // {
    //         //     // Changer le parent de l'armature pour qu'elle soit attachée à "attachmentPoint"
    //         //     armature.SetParent(attachmentPointLeft);

    //         //     // Réinitialiser la position locale pour aligner correctement l'armature
    //         //     armature.localPosition = Vector3.zero;
    //         //     armature.localRotation = Quaternion.identity;

    //         //     Debug.Log("Armature attachée avec succès !");
    //         // }
    //         // else
    //         // {
    //         //     Debug.LogWarning("Aucune armature trouvée sur l'objet détecté.");
    //         // }
    //     }
    // }
}
