using UnityEngine;

public class CameraScript : MonoBehaviour
{

    private Transform player;
    public Vector3 offset;

    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player").TryGetComponent(out Transform outTarget))
        {
            player = outTarget;
        }
    }
    void FixedUpdate()
    {
        // Camera follows the player with specified offset position
        transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y,
            offset.z); 
    }
}
