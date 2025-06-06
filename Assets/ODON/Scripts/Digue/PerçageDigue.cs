using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PerçageDigue : MonoBehaviour
{
    public bool isTheGoodDigue = false;
    public bool isActionPressed = false;

    private BoxCollider col;
    private ArmatureDigueBehaviour armatureDigueBehaviour;

    void Awake()
    {
        col = GetComponent<BoxCollider>();
        if (col == null)
        {
            Debug.LogError("No Collider found on " + gameObject.name + ". Adding one now.");
            col = gameObject.AddComponent<BoxCollider>();
        }
        col.isTrigger = true;
        col.size = new Vector3(0.2f,0.2f,0.2f);

        armatureDigueBehaviour = GetComponentInChildren<ArmatureDigueBehaviour>();
        if (armatureDigueBehaviour == null)
        {
            Debug.LogError("Your dam requires ArmatureDigueBehaviour");
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PinceDigue") && isTheGoodDigue && isActionPressed)
        {
            Debug.Log("La digue est percée");
            armatureDigueBehaviour.InitJoints();
            col.enabled = false;
        }
    }

    public void SetActionPress(bool boolean){
        isActionPressed = boolean;
    }
    public void SetIsTheGoodDigue(bool boolean){
        isTheGoodDigue = boolean;
    }

    public bool GetActionPress(){
        return isActionPressed;
    }
    public bool GetIsTheGoodDigue(){
        return isTheGoodDigue;
    }

}
