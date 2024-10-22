using UnityEngine;

public class Interactions_tester : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void OnFirstHoverEnter()
    {
        //print("first hover");
    }
    public void OnLastHoverExited()
    {
        //print("last hover");
    }
    public void OnHoverEntered()
    {
        //print("hover entered");
    }
    public void OnHoverExited()
    {
        //print("hover entered");
    }
    public void OnFirstSelectEnter()
    {
        print("first select");
    }
    public void OnLastSelectExited()
    {
        print("last select");
    }
    public void OnSelectEntered()
    {
        print("Select entered");
    }
    public void OnSelectExited()
    {
        print("Select entered");
    }
    public void OnFirstFocusEnter()
    {
        print("first Focus");
    }
    public void OnLastFocusExited()
    {
        print("last Focus");
    }
    public void OnFocusEntered()
    {
        print("Focus entered");
    }
    public void OnFocusExited()
    {
        print("Focus entered");
    }
    public void OnActivated()
    {
        print("Activated");
    }
    public void OnDesactivated()
    {
        print("Desactivated");
    }
}
