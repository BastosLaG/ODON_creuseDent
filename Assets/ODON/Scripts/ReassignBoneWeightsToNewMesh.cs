using UnityEngine;

[ExecuteInEditMode]
public class ReassignBoneWeightsToNewMesh : MonoBehaviour
{
   [SerializeField] public Transform _newArmature;
   [SerializeField] public string _rootBoneName = "Hips";
   public bool _reassign;

   private void Update()
   {
       if (_reassign)
           Reassign();
       _reassign = false;
   }

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

       // Reassign root bone
       meshRenderer.rootBone = rootBone;

       var newArmatureBones = _newArmature.GetComponentsInChildren<Transform>();
       var meshRendererBones = meshRenderer.bones;

       // Reassign bones
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