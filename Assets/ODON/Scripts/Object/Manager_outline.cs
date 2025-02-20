using System;
using UnityEngine;

public class Manager_outline : MonoBehaviour
{
    public void UpdateOutline(int etape)
    {
        switch (etape)
        {
            case 0:
                print(etape);
                var outline = GameObject.Find("Porte_doc").AddComponent<Outline>();
                outline.OutlineMode = Outline.Mode.OutlineAndSilhouette;
                outline.OutlineColor = Color.yellow;
                outline.OutlineWidth = 10f;
                break;
            case 1:
                print(etape);
                break;
            case 2:
                print(etape);
                break;
            case 3:
                print(etape);
                break;
            case 4:
                print(etape);
                break;
            case 5:
                print(etape);
                break;
            case 6:
                print(etape);
                break;
            default:
                print(etape);
                break;
        }
    }
}
