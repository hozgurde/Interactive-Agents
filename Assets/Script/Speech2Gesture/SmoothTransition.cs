using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothTransition : MonoBehaviour
{

    [SerializeField] public Transform CopyJoint;
    [SerializeField] public float speed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Slerp(transform.localRotation, CopyJoint.localRotation, speed * Time.deltaTime);
        transform.localPosition = Vector3.Slerp(transform.localPosition, CopyJoint.localPosition, speed * Time.deltaTime);
    }
}
