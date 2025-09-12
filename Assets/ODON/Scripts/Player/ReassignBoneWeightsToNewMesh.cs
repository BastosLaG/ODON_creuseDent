using UnityEngine;

[ExecuteInEditMode]
/// <summary>
/// Réassigne dynamiquement les poids d’os d’un <see cref="SkinnedMeshRenderer"/> à une nouvelle armature.
/// </summary>
/// <remarks>
/// Ce script est conçu pour être exécuté en mode Édition ou en mode Lecture grâce à l’attribut <see cref="ExecuteInEditMode"/>.
/// Il permet de transférer les liaisons d’os d’un mesh skinné vers une nouvelle hiérarchie d’armature.
/// </remarks>
public class ReassignBoneWeightsToNewMesh : MonoBehaviour
{
    /// <summary>
    /// Référence vers la nouvelle armature contenant la hiérarchie de bones.
    /// </summary>
    [SerializeField] 
    public Transform _newArmature;

    /// <summary>
    /// Nom du bone racine de la nouvelle armature (par défaut : "Hips").
    /// </summary>
    [SerializeField] 
    public string _rootBoneName = "Hips";

    /// <summary>
    /// Déclenche le processus de réassignation des bones lorsqu’il est défini sur <c>true</c>.
    /// </summary>
    public bool _reassign;

    /// <summary>
    /// Appelé à chaque frame en mode Édition ou Lecture.  
    /// Si <see cref="_reassign"/> est activé, appelle <see cref="Reassign"/> pour transférer les bones.
    /// </summary>
    private void Update()
    {
        if (_reassign)
            Reassign();
        _reassign = false;
    }

    /// <summary>
    /// Réassigne les bones du <see cref="SkinnedMeshRenderer"/> attaché à ce GameObject
    /// vers ceux de la nouvelle armature définie par <see cref="_newArmature"/>.
    /// </summary>
    /// <remarks>
    /// - Vérifie la présence de l’armature et du bone racine.  
    /// - Met à jour <see cref="SkinnedMeshRenderer.rootBone"/> et <see cref="SkinnedMeshRenderer.bones"/>.  
    /// - Affiche des messages d’erreur si les éléments nécessaires ne sont pas trouvés.  
    /// </remarks>
    private void Reassign()
    {
        if (!_newArmature)
        {
            Debug.Log("No new armature assigned");
            return;
        }

        var rootBone = FindRecursive(_newArmature, _rootBoneName);
        if (!rootBone)
        {
            Debug.Log("Root bone not found");
            return;
        }

        var meshRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
        if (!meshRenderer)
        {
            Debug.Log("SkinnedMeshRenderer not found");
            return;
        }

        // Réassigne le bone racine
        meshRenderer.rootBone = rootBone;

        var newArmatureBones = _newArmature.GetComponentsInChildren<Transform>();
        var meshRendererBones = meshRenderer.bones;

        // Réassigne chaque bone correspondant par nom
        for (var i = 0; i < meshRendererBones.Length; i++)
        {
            foreach (var bone in newArmatureBones)
            {
                if (meshRendererBones[i].name != bone.name) continue;
                meshRendererBones[i] = bone;
                break;
            }
        }

        meshRenderer.bones = meshRendererBones;
        Debug.Log("Bones reassigned successfully.");
    }

    /// <summary>
    /// Recherche récursive d’un bone par nom dans une hiérarchie de <see cref="Transform"/>.
    /// </summary>
    /// <param name="parent">Transform racine à partir duquel effectuer la recherche.</param>
    /// <param name="targetName">Nom du bone recherché.</param>
    /// <returns>Le <see cref="Transform"/> trouvé, ou <c>null</c> s’il n’existe pas.</returns>
    private static Transform FindRecursive(Transform parent, string targetName)
    {
        if (parent.name == targetName)
            return parent;

        foreach (Transform child in parent)
        {
            var result = FindRecursive(child, targetName);
            if (result) return result;
        }

        return null;
    }
}
