using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVHAnimationControllerFixed : MonoBehaviour
{
    public bool UseFixedGestures = false;

    public int c;

    public int l;

    public AgentController agentController;

    private BVHAnimationLoader bVHAnimationLoader;

    private string currentBVHFile;

    private const string startPath = "Assets/Script/Speech2Gesture/ZeggsData/E1_C";

    private bool inAnimation = false;

    public Transform startJoint;

    private List<Transform> mainList;

    private List<Quaternion> initialQuaternions;

    private string fixedGesturePath = "Assets/Script/Speech2Gesture/ZeggsData/AnimNo1";

    // Start is called before the first frame update
    void Start()
    {
        c = -1;
        l = -1;
        bVHAnimationLoader = gameObject.GetComponent<BVHAnimationLoader>();
        currentBVHFile = "Assets/Script/Speech2Gesture/ZeggsData/audio_test_label_001_Neutral_0_x_1_0.bvh";

        mainList = new List<Transform>();
        mainList.Add(startJoint);
        initialQuaternions = new List<Quaternion>();
        initialQuaternions.Add(startJoint.rotation);
        int index = 0;

        Transform curMain;

        while (index < mainList.Count)
        {
            curMain = mainList[index];
            index++;

            foreach (Transform child in curMain)
            {
                mainList.Add(child);
                initialQuaternions.Add(child.rotation);
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!inAnimation && c != -1 && l != -1 && !currentBVHFile.Equals(startPath + c.ToString() + "/E1_C" + c.ToString() + "_L" + (2*l + 1).ToString() + "_A_TTS.bvh"))
        {
            inAnimation = true;
            agentController.BVHMovementControl(inAnimation);
            currentBVHFile = startPath + c.ToString() + "/E1_C" + c.ToString() + "_L" + (2 * l + 1).ToString() + "_A_TTS.bvh";
            bVHAnimationLoader.filename = currentBVHFile;

            bVHAnimationLoader.parseFile();
            bVHAnimationLoader.loadAnimation();

            /*for(int i = 0; i < mainList.Count; i++)
            {
                initialQuaternions[i] = mainList[i].rotation;
            }*/

            
        }

        if (inAnimation && !this.GetComponent<Animation>().isPlaying)
        {
            inAnimation = false;
            agentController.BVHMovementControl(inAnimation);
            /*for(int i = 0; i < mainList.Count; i++)
            {
                mainList[i].rotation = initialQuaternions[i];
            }*/
        }
    }
}
