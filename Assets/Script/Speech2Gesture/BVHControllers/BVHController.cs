using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVHController : MonoBehaviour
{
    [SerializeField] public BVHAnimationLoader BVHAnimationLoader;

    [SerializeField] public Transform startJoint;

    // Start is called before the first frame update
    void Start()
    {
        var shownQueue = new Queue<Transform>();

        shownQueue.Enqueue(startJoint);

        Transform cur;

        int nodeCount = 0; 

        while (shownQueue.Count > 0)
        {
            print("in bvh " + nodeCount);
            cur = shownQueue.Dequeue();
            nodeCount++;
            //cur.name

            foreach (Transform child in cur)
            {
                shownQueue.Enqueue(child);
            }

        }

        BVHAnimationLoader.boneRenamingMap = new BVHAnimationLoader.FakeDictionary[nodeCount];

        shownQueue.Enqueue(startJoint);
        nodeCount = 0;

        while (shownQueue.Count > 0)
        {
            cur = shownQueue.Dequeue();
            BVHAnimationLoader.boneRenamingMap[nodeCount].targetName = cur.name;
            BVHAnimationLoader.boneRenamingMap[nodeCount].bvhName = cur.name.Split(':')[1];

            foreach (Transform child in cur)
            {
                shownQueue.Enqueue(child);
            }
            nodeCount++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
