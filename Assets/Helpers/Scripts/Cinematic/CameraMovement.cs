using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement instance;

    [SerializeField] private float trackSpeed = 5f;

    private float originZ;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        originZ = transform.position.z;
    }

    public IEnumerator TargetObject(Transform target)
    {
        while (true)
        {
            if (target != null)
            {
                Vector3 targetPosition = new(target.position.x, target.position.y, originZ);
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * trackSpeed);
            }
            yield return null;
        }
    }
}