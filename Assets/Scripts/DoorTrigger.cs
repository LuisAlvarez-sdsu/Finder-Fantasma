using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [Header("Door Animation")]
    public Animation doorAnimation;   // The Animation component on the door
    public string openClipName;       // Name of the clip that opens the door
    public string closeClipName;      // Optional: clip to close the door

    private bool isOpen = false;

    public void OpenDoor()
    {
        if (!isOpen && doorAnimation != null)
        {
            doorAnimation.Play(openClipName);
            isOpen = true;
        }
    }

    public void CloseDoor()
    {
        if (isOpen && doorAnimation != null && !string.IsNullOrEmpty(closeClipName))
        {
            doorAnimation.Play(closeClipName);
            isOpen = false;
        }
    }
}