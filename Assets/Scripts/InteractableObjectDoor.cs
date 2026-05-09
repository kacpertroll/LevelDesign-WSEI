using UnityEngine;

public class InteractableObjectDoor : MonoBehaviour
{
    public bool isOpen = false;
    public Vector3 openRotation = new Vector3(0, 90, 0);
    public Vector3 closeRotation = new Vector3(0, 0, 0);

    public void OnInteract()
    {
        Debug.Log("Interacted with " + gameObject.name);

        if (!isOpen)
        {
            transform.Rotate(openRotation);
            isOpen = true;
        }
        else
        {
            transform.Rotate(closeRotation);
            isOpen = false;
        }
    }
}