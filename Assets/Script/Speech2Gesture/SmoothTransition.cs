using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothTransition : MonoBehaviour
{

    [SerializeField] public Transform CopyJoint;
    [SerializeField] public float speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var targetRotation = CopyJoint.rotation;

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
    }
}
