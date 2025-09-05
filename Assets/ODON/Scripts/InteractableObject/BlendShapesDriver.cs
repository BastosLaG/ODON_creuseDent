using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class BlendShapesDriver : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SkinnedMeshRenderer driver;
    [SerializeField] private List<SkinnedMeshRenderer> meshToDrive;

    [Header("Settings")]
    [SerializeField] private float _Speed = 10f;


    private bool inTranslation = false;

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

    // //TEST FUNCTION TO REMOVE
    // private void Update()
    // {
    //     Debug.LogWarning("Remove this function", driver);
    //     GoToValue("Open", Random.Range(0, 100));
    // }

    /// <summary>
    /// Smoothly set the blendShape value on both the driver and the driven object.
    /// </summary>
    /// <param name="blendShapeName"></param>
    /// <param name="value"></param>
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
    /// Directly set the blendShape value on both the driver and the driven object.
    /// </summary>
    /// <param name="index">blendShape index</param>
    /// <param name="value"></param>
    public void SetBlendShapeValue(int index, float value)
    {
        SetBlendShapeValue(driver.sharedMesh.GetBlendShapeName(index), value);
    }

    /// <summary>
    /// Directly set the blendShape value on both the driver and the driven object.
    /// </summary>
    /// <param name="bsName">blendShape name</param>
    /// <param name="value"></param>
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
    /// Find the BlendShape on the object that matches a given name.
    /// </summary>
    /// <param name="skm">SkinnedMeshRenderer of the object with Blenshapes</param>
    /// <param name="nameToFind"></param>
    /// <returns></returns>
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
