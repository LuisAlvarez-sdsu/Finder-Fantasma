using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform cameraTransform;
    public float parallaxEffect = 0.3f;

    private float startPosX;
    private float spriteWidth;

    void Start()
    {
        startPosX = transform.position.x;
        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float camX = cameraTransform.position.x * parallaxEffect;
        transform.position = new Vector3(
            startPosX + camX,
            transform.position.y,
            transform.position.z
        );

        // Infinite scrolling part
        float distance = cameraTransform.position.x - transform.position.x;

        if (distance > spriteWidth)
        {
            startPosX += spriteWidth * 2;
        }
        else if (distance < -spriteWidth)
        {
            startPosX -= spriteWidth * 2;
        }
    }
}