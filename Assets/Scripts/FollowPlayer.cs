using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private float followSpeed;
    public Transform target;
    void Start()
    {
        followSpeed = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) {
            transform.position = new Vector3(0, 0, -10f);
            return;
        }

        Vector3 destPos = target.position;
        destPos.z = -10f;
        transform.position = Vector3.Slerp(transform.position, destPos, followSpeed * Time.deltaTime);
    }
}
