using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This is a camera script made by Haravin (Daniel Valcour).
//This script is public domain, but credit is appreciated!

//[RequireComponent(typeof(Camera))]
public class StolenFollowCamera : MonoBehaviour
{
    public GameObject target;
    public float rotateSpeed = 5;
    public float heightOffset;
    Vector3 offset;

    void Start()
    {
        offset = transform.parent.transform.position - transform.position - new Vector3(0, heightOffset, 0);
    }

    void LateUpdate()
    {
        transform.parent.transform.position = target.transform.position;

        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        transform.parent.transform.Rotate(0, horizontal, 0);

        float desiredAngle = transform.parent.transform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
        transform.parent.transform.position = transform.parent.transform.position - (rotation * offset);

        transform.LookAt(target.transform);
    }
}
