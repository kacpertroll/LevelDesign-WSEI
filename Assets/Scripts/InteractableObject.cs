using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public bool isDoor = false;
    public bool isLocked = false;

    public bool isOpen = false;
    public Vector3 openRotation = new Vector3(0, 90, 0);
    public Vector3 closeRotation = new Vector3(0, 0, 0);

    public void OnInteract()
    {
        Debug.Log("Interacted with " + gameObject.name);

        if (isDoor)
        {
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
        else return;
    }
}