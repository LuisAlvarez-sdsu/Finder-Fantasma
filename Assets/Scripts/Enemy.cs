using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public Transform[] points;

    private int i = 0;
    private int direction = 1; // 1 for forward, -1 for backward
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (points.Length > 0)
            transform.position = points[0].position;
    }

    void Update()
    {
        if (points.Length < 2) return; // Need at least 2 points to patrol

        // 1. Move towards target
        transform.position = Vector2.MoveTowards(
            transform.position,
            points[i].position,
            speed * Time.deltaTime
        );

        // 2. Check if reached point
        if (Vector2.Distance(transform.position, points[i].position) < 0.05f)
        {
            // Reverse direction at the ends of the array
            if (i >= points.Length - 1)
            {
                direction = -1; // Start going backwards
            }
            else if (i <= 0)
            {
                direction = 1; // Start going forwards
            }

            i += direction;

            // 3. Flip based on next target's X position
            spriteRenderer.flipX = points[i].position.x < transform.position.x;
        }
    }
}
