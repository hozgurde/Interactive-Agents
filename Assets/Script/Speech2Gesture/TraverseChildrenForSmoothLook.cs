using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraverseChildrenForSmoothLook : MonoBehaviour
{
    [SerializeField] public Transform mainStartJoint;
    [SerializeField] public Transform shownStartJoint;

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
        
    }
}
