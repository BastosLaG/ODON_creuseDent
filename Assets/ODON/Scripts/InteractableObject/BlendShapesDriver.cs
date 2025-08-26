using System.Collections;
using UnityEngine;

public class BlendShapesDriver : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer driver;
    [SerializeField] private SkinnedMeshRenderer[] meshToDrive;


    private bool inTranslation = false;

    private void Update()
    {
        // GoToValue("Open", Random.Range(0,100));
    }


    public void GoToValue(string blendShapeName, float value)
    {
        float actualWeight = -1;
        int blenShapeIndex = FindBlendShapeIndexByName(driver, blendShapeName);
        if (blenShapeIndex >= 0) actualWeight = driver.GetBlendShapeWeight(blenShapeIndex);
        if (actualWeight >= 0 && !inTranslation)
        {
            inTranslation = true;
            StartCoroutine(TranslateBlendShapeValues(blendShapeName, actualWeight, value));
        }
    }

    private IEnumerator TranslateBlendShapeValues(string blendShapeName, float startValue, float endValue)
    {
        int step = startValue < endValue ? 1 : -1;
        while ((step > 0 && startValue < endValue) || (step < 0 && startValue > endValue))
        {
            startValue += Time.deltaTime * 1000 * step;
            SetBlendShapeValue(blendShapeName, startValue);
            yield return new WaitForEndOfFrame();
        }
        SetBlendShapeValue(blendShapeName, endValue);
        inTranslation = false;
    }


    public void SetBlendShapeValue(int index, float value)
    {
        SetBlendShapeValue(driver.sharedMesh.GetBlendShapeName(index), value);
    }
    public void SetBlendShapeValue(string index, float value)
    {
        int blenShapeIndex = FindBlendShapeIndexByName(driver, index);
        if (blenShapeIndex >= 0) driver.SetBlendShapeWeight(blenShapeIndex, value);

        foreach (SkinnedMeshRenderer skm in meshToDrive)
        {
            int skmBlenShapeIndex = FindBlendShapeIndexByName(skm, index);
            if (skmBlenShapeIndex >= 0) skm.SetBlendShapeWeight(skmBlenShapeIndex, value);
        }
    }

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
