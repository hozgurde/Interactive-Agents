using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightIKControllerBVH : MonoBehaviour
{

    public Transform RightShoulder;
    public Transform RightElbow;
    public Transform RightHand;

    public GameObject TargetRightElbow;
    public GameObject TargetRightHand;

    public Transform BVHRightShoulder;
    public Transform BVHRightElbow;
    public Transform BVHRightHand;

    public float ShoulderRotationX = 0f;
    public float ShoulderRotationY = 0f;
    public float ShoulderRotationZ = 0f;

    public float HandRotationX = 0f;
    public float HandRotationY = 0f;
    public float HandRotationZ = 0f;




    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 ShoulderDir = (BVHRightElbow.position - BVHRightShoulder.position);
        TargetRightElbow.transform.position = Quaternion.Euler(ShoulderRotationX, ShoulderRotationY, ShoulderRotationZ) * ShoulderDir + RightShoulder.position;
        TargetRightHand.transform.position = (BVHRightHand.position - BVHRightElbow.position) + TargetRightElbow.transform.position;
        TargetRightHand.transform.rotation = Quaternion.Euler(HandRotationX, HandRotationY, HandRotationZ);
        TargetRightHand.transform.rotation = Quaternion.Euler(BVHRightHand.rotation.eulerAngles.x, BVHRightHand.rotation.eulerAngles.y, BVHRightHand.rotation.eulerAngles.z) * TargetRightHand.transform.rotation;
    }
}
