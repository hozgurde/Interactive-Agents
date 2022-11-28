using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftIKControllerBVH : MonoBehaviour
{

    public Transform LeftShoulder;
    public Transform LeftElbow;
    public Transform LeftHand;

    public GameObject TargetLeftElbow;
    public GameObject TargetLeftHand;

    public Transform BVHLeftShoulder;
    public Transform BVHLeftElbow;
    public Transform BVHLeftHand;

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
        Vector3 ShoulderDir = (BVHLeftElbow.position - BVHLeftShoulder.position);
        TargetLeftElbow.transform.position = Quaternion.Euler(ShoulderRotationX, ShoulderRotationY, ShoulderRotationZ) * ShoulderDir  + LeftShoulder.position;
        TargetLeftHand.transform.position = (BVHLeftHand.position - BVHLeftElbow.position) + TargetLeftElbow.transform.position;
        TargetLeftHand.transform.rotation = Quaternion.Euler(HandRotationX, HandRotationY, HandRotationZ);
        TargetLeftHand.transform.rotation = Quaternion.Euler(BVHLeftHand.rotation.eulerAngles.x, BVHLeftHand.rotation.eulerAngles.y, BVHLeftHand.rotation.eulerAngles.z) * TargetLeftHand.transform.rotation;
    }
}
