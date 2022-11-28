using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{

    public Transform CurSpine;
    public Transform CurSpine1;
    public Transform CurSpine2;
    public Transform CurNeck;
    public Transform CurHead;

    public Transform RefSpine;
    public Transform RefSpine1;
    public Transform RefSpine2;
    public Transform RefNeck;
    public Transform RefHead;

    private Quaternion prevSpineRot;
    private Quaternion prevSpine1Rot;
    private Quaternion prevSpine2Rot;
    private Quaternion prevNeckRot;
    private Quaternion prevHeadRot;

    public GameObject Agent;

    public float waitSec = 0.01f;



    void Start()
    {
        prevSpineRot = Quaternion.identity;
        prevSpine1Rot = Quaternion.identity;
        prevSpine2Rot = Quaternion.identity;
        prevNeckRot = Quaternion.identity;
        prevHeadRot = Quaternion.identity;

    }
    // Update is called once per frame
    void Update()
    {

        if(waitSec >= 0f)
        {
            waitSec -= Time.deltaTime;
            return;
        }

        if (prevSpineRot != Quaternion.identity)
        {
            CurSpine.rotation = RefSpine.rotation * Quaternion.Inverse(prevSpineRot) * CurSpine.rotation;
            CurSpine1.rotation = RefSpine1.rotation * Quaternion.Inverse(prevSpine1Rot) * CurSpine1.rotation;
            CurSpine2.rotation = RefSpine2.rotation * Quaternion.Inverse(prevSpine2Rot) * CurSpine2.rotation;
            CurNeck.rotation = RefNeck.rotation * Quaternion.Inverse(prevNeckRot) * CurNeck.rotation;
            CurHead.rotation = RefHead.rotation * Quaternion.Inverse(prevHeadRot) * CurHead.rotation;

            /*CurSpine.rotation = Quaternion.Euler(RefSpine.rotation.eulerAngles.x - prevSpineRot.eulerAngles.x, RefSpine.rotation.eulerAngles.y - prevSpineRot.eulerAngles.y, RefSpine.rotation.eulerAngles.z - prevSpineRot.eulerAngles.z) * CurSpine.rotation;
            CurSpine1.rotation = Quaternion.Euler(RefSpine1.rotation.eulerAngles.x - prevSpine1Rot.eulerAngles.x, RefSpine1.rotation.eulerAngles.y - prevSpine1Rot.eulerAngles.y, RefSpine1.rotation.eulerAngles.z - prevSpine1Rot.eulerAngles.z) * CurSpine1.rotation;
            CurSpine2.rotation = Quaternion.Euler(RefSpine2.rotation.eulerAngles.x - prevSpine2Rot.eulerAngles.x, RefSpine2.rotation.eulerAngles.y - prevSpine2Rot.eulerAngles.y, RefSpine2.rotation.eulerAngles.z - prevSpine2Rot.eulerAngles.z) * CurSpine2.rotation;
            CurNeck.rotation = Quaternion.Euler(RefNeck.rotation.eulerAngles.x - prevNeckRot.eulerAngles.x, RefNeck.rotation.eulerAngles.y - prevNeckRot.eulerAngles.y, RefNeck.rotation.eulerAngles.z - prevNeckRot.eulerAngles.z) * CurNeck.rotation;
            CurHead.rotation = Quaternion.Euler(RefHead.rotation.eulerAngles.x - prevHeadRot.eulerAngles.x, RefHead.rotation.eulerAngles.y - prevHeadRot.eulerAngles.y, RefHead.rotation.eulerAngles.z - prevHeadRot.eulerAngles.z) * CurHead.rotation;*/
        }
        else
        {
            //Agent.GetComponent<Animator>().enabled = false;
        }

        prevSpineRot = RefSpine.rotation;
        prevSpine1Rot = RefSpine1.rotation;
        prevSpine2Rot = RefSpine2.rotation;
        prevNeckRot = RefNeck.rotation;
        prevHeadRot = RefHead.rotation;

            /*CurSpine.rotation = RefSpine.rotation;
            CurSpine1.rotation = RefSpine1.rotation;
            CurSpine2.rotation = RefSpine2.rotation;
            CurNeck.rotation = RefNeck.rotation;
            CurHead.rotation = RefHead.rotation;*/



    }
}
