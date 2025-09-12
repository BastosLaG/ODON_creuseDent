using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ODON
{    
    /// <summary>
    /// Drives blend shape values on multiple SkinnedMeshRenderer components in the ODON application.
    /// Allows smooth and direct control of blend shapes by name or index.
    /// </summary>
    [RequireComponent(typeof(SkinnedMeshRenderer))]
    public class BlendShapesDriver : MonoBehaviour
    {
        /// <summary>
        /// Reference to the main SkinnedMeshRenderer that drives blend shapes.
        /// </summary>
        [Header("References")]
        [SerializeField] private SkinnedMeshRenderer driver;

        /// <summary>
        /// List of SkinnedMeshRenderer components to be driven by the main driver.
        /// </summary>
        [SerializeField] private List<SkinnedMeshRenderer> meshToDrive;

        /// <summary>
        /// Speed of blend shape value transitions.
        /// </summary>
        [Header("Settings")]
        [SerializeField] private float _Speed = 10f;

        /// <summary>
        /// Indicates if a blend shape value is currently being transitioned.
        /// </summary>
        private bool inTranslation = false;

        /// <summary>
        /// Initializes the driver and collects SkinnedMeshRenderer components from child objects.
        /// </summary>
        private void Start()
        {
            driver = driver != null ? driver : GetComponent<SkinnedMeshRenderer>();
            if (driver == null)
            {
                Debug.LogError("No SkinnedMeshRenderer found on the object.", this);
            }

            foreach (Transform child in transform)
            {
                if (child.TryGetComponent<SkinnedMeshRenderer>(out var skinnedMeshRenderer))
                {
                    meshToDrive.Add(skinnedMeshRenderer);
                }
            }
        }

        /// <summary>
        /// Smoothly sets the blend shape value on both the driver and the driven objects.
        /// </summary>
        /// <param name="blendShapeName">The name of the blend shape to modify.</param>
        /// <param name="value">The target value for the blend shape.</param>
        public void GoToValue(string blendShapeName, float value)
        {
            float actualWeight = -1;
            int blenShapeIndex = FindBlendShapeIndexByName(driver, blendShapeName);
            if (blenShapeIndex >= 0) actualWeight = driver.GetBlendShapeWeight(blenShapeIndex);
            if (actualWeight >= 0 && !inTranslation)
            {
                inTranslation = true;
                StopAllCoroutines();
                StartCoroutine(TranslateBlendShapeValues(blendShapeName, actualWeight, value));
            }
        }

        /// <summary>
        /// Coroutine to smoothly transition blend shape values from startValue to endValue.
        /// </summary>
        /// <param name="blendShapeName">The name of the blend shape.</param>
        /// <param name="startValue">The starting value.</param>
        /// <param name="endValue">The target value.</param>
        /// <returns>IEnumerator for coroutine.</returns>
        private IEnumerator TranslateBlendShapeValues(string blendShapeName, float startValue, float endValue)
        {
            int step = startValue < endValue ? 1 : -1;
            while ((step > 0 && startValue < endValue) || (step < 0 && startValue > endValue))
            {
                startValue += Time.deltaTime * _Speed * step;
                SetBlendShapeValue(blendShapeName, startValue);
                yield return null;
            }
            SetBlendShapeValue(blendShapeName, endValue);
            inTranslation = false;
        }

        /// <summary>
        /// Directly sets the blend shape value by index on both the driver and the driven objects.
        /// </summary>
        /// <param name="index">The index of the blend shape.</param>
        /// <param name="value">The value to set.</param>
        public void SetBlendShapeValue(int index, float value)
        {
            SetBlendShapeValue(driver.sharedMesh.GetBlendShapeName(index), value);
        }

        /// <summary>
        /// Directly sets the blend shape value by name on both the driver and the driven objects.
        /// </summary>
        /// <param name="bsName">The name of the blend shape.</param>
        /// <param name="value">The value to set.</param>
        public void SetBlendShapeValue(string bsName, float value)
        {
            int blenShapeIndex = FindBlendShapeIndexByName(driver, bsName);
            if (blenShapeIndex >= 0) driver.SetBlendShapeWeight(blenShapeIndex, value);

            foreach (SkinnedMeshRenderer skm in meshToDrive)
            {
                int skmBlenShapeIndex = FindBlendShapeIndexByName(skm, bsName);
                if (skmBlenShapeIndex >= 0) skm.SetBlendShapeWeight(skmBlenShapeIndex, value);
            }
        }

        /// <summary>
        /// Finds the index of a blend shape by name on the given SkinnedMeshRenderer.
        /// </summary>
        /// <param name="skm">The SkinnedMeshRenderer to search.</param>
        /// <param name="nameToFind">The name of the blend shape to find.</param>
        /// <returns>The index of the blend shape if found, otherwise -1.</returns>
        private int FindBlendShapeIndexByName(SkinnedMeshRenderer skm, string nameToFind)
        {
            int blendShapeCount = driver.sharedMesh.blendShapeCount;
            for (int i = 0; i < blendShapeCount; i++)
            {
                if (skm.sharedMesh.GetBlendShapeName(i) == nameToFind)
                {
                    return i;
                }
            }
            return -1;
        }
    }

}