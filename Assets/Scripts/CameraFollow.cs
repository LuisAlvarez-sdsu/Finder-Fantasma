using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 2f, -10f);
    private bool followPlayer = true;

    void LateUpdate()
    {
        if (followPlayer && player != null)
        {
            transform.position = player.position + offset;
        }
    }

    public void StopFollowing()
    {
        followPlayer = false;
    }

    public void ResumeFollowing()
    {
        followPlayer = true;
    }
}