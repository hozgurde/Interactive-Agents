using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraverseChildrenForSmoothLook : MonoBehaviour
{
    [SerializeField] public Transform mainStartJoint;
    [SerializeField] public Transform shownStartJoint;
    /*[SerializeField] public bool IsInFinger = false;
    [SerializeField] public float ThumbRotX = 0f;
    [SerializeField] public float ThumbRotY = 0f;
    [SerializeField] public float ThumbRotZ = 0f;
    [SerializeField] public float RestRotX = 0f;
    [SerializeField] public float RestRotY = 0f;
    [SerializeField] public float RestRotZ = 0f;*/




    // Start is called before the first frame update
    void Start()
    {
        var mainQueue = new Queue<Transform>();
        var shownQueue = new Queue<Transform>();

        mainQueue.Enqueue(mainStartJoint);
        shownQueue.Enqueue(shownStartJoint);

        Transform curShown;
        Transform curMain;
        SmoothTransition smoothTransition;

        while(mainQueue.Count > 0)
        {
            curShown = shownQueue.Dequeue();
            curMain = mainQueue.Dequeue();

            foreach(Transform child in curShown)
            {
                shownQueue.Enqueue(child);
            }

            foreach(Transform child in curMain)
            {
                mainQueue.Enqueue(child);
            }

            smoothTransition = curShown.gameObject.AddComponent<SmoothTransition>();
            smoothTransition.CopyJoint = curMain;
           
        }

        


    }

    // Update is called once per frame
    void Update()
    {

        /*if (IsInFinger)
        {

            var mainQueue = new Queue<Transform>();
            var shownQueue = new Queue<Transform>();

            Transform curShown;
            Transform curMain;


            foreach (Transform child in shownStartJoint)
            {
                shownQueue.Enqueue(child);
            }

            foreach (Transform child in mainStartJoint)
            {
                mainQueue.Enqueue(child);
            }

            while (mainQueue.Count > 0)
            {
                curShown = shownQueue.Dequeue();
                curMain = mainQueue.Dequeue();

                if (curShown.name.Contains("Thumb"))
                {
                    curShown.rotation = Quaternion.Euler(ThumbRotX, ThumbRotY, ThumbRotZ) * curMain.rotation;
                }
                else
                {
                    curShown.rotation = Quaternion.Euler(RestRotX, RestRotY, RestRotZ) * curMain.rotation;
                }

            }

            

        }*/


    }
}
