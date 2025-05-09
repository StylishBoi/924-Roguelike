using UnityEngine;

public class ProjectileRotation : MonoBehaviour
{
    void Update ()
    {
        transform.Rotate(0f, 0f,100f*Time.deltaTime, Space.Self);
    }
}
