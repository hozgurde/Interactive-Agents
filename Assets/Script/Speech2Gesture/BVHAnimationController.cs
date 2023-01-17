using DitzelGames.FastIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVHAnimationController : MonoBehaviour
{
    public bool UseFixedGestures = false;

    public int c;

    public int l;

    public AgentController agentController;

    private BVHAnimationLoader bVHAnimationLoader;

    public BVHAnimationLoader fixed_bVHAnimationLoader;

    private string currentBVHFile;

    private const string startPath = "Assets/Script/Speech2Gesture/ZeggsData/E1_C";

    private bool inAnimation = false;

    public Transform startJoint;

    private List<Transform> mainList;

    private List<Quaternion> initialQuaternions;

    private string fixedGesturePath = "Assets/Script/Speech2Gesture/ZeggsData/AnimNo1.bvh";

    public Transform[] hand_transforms;
    [SerializeField]
    public float[] hand_duration;
    private int hand_currentTransform = 0;
    private float hand_startTime;

    public GameObject HandIKTarget;
    public GameObject HandPoleIKTarget;
    public Transform fixedPoleTarget;

    public Transform[] body_transforms;
    [SerializeField]
    public float[] body_duration;
    private int body_currentTransform = 0;
    private float body_startTime;

    public GameObject BodyIKTarget;

    public FastIKFabric BodyIK;
    public FastIKFabric LeftHandIK;

    // Start is called before the first frame update
    void Start()
    {
        c = -1;
        l = -1;
        bVHAnimationLoader = gameObject.GetComponent<BVHAnimationLoader>();
        currentBVHFile = "Assets/Script/Speech2Gesture/ZeggsData/audio_test_label_001_Neutral_0_x_1_0.bvh";

        if (UseFixedGestures)
        {
            fixed_bVHAnimationLoader.filename = fixedGesturePath;
            fixed_bVHAnimationLoader.respectBVHTime = false;
            fixed_bVHAnimationLoader.frameRate = 24;
            fixed_bVHAnimationLoader.parseFile();
        }

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
            if(UseFixedGestures && l == 2)
            {
                hand_startTime = Time.time;
                body_startTime = Time.time;
                agentController.BVHMovementControl(inAnimation, true);
                BodyIK.enabled = true;
                LeftHandIK.enabled = false;
            }
            else if (UseFixedGestures && (c == 0 && l == 7 ||
                       c == 1 && l == 8 ||
                       c == 2 && l == 7 ||
                       c == 3 && l == 7 ||
                       c == 4 && l == 7 ||
                       c == 5 && l == 7 ||
                       c == 6 && l == 7 ||
                       c == 7 && l == 7 ||
                       c == 8 && l == 9 ||
                       c == 9 && l == 7))
            {
                hand_startTime = Time.time;
                body_startTime = Time.time;
                agentController.BVHMovementControl(inAnimation, true);
                BodyIK.enabled = true;
                LeftHandIK.enabled = false;
                hand_currentTransform = hand_transforms.Length - 1;
                body_currentTransform = body_transforms.Length - 1;
            }
            else if (UseFixedGestures)
            {
                fixed_bVHAnimationLoader.playAnimation();
            }
            

            /*for(int i = 0; i < mainList.Count; i++)
            {
                initialQuaternions[i] = mainList[i].rotation;
            }*/

            
        }

        if (inAnimation && !this.GetComponent<Animation>().isPlaying)
        {

          
            inAnimation = false;
                

            if (UseFixedGestures && l == 2)
            {
                
            }
            else if (UseFixedGestures && (c == 0 && l == 7 ||
                       c == 1 && l == 8 ||
                       c == 2 && l == 7 ||
                       c == 3 && l == 7 ||
                       c == 4 && l == 7 ||
                       c == 5 && l == 7 ||
                       c == 6 && l == 7 ||
                       c == 7 && l == 7 ||
                       c == 8 && l == 9 ||
                       c == 9 && l == 7))
            {

            }
            else if (UseFixedGestures)
            {
                agentController.BVHMovementControl(inAnimation);
                fixed_bVHAnimationLoader.stopAnimation();
            }
            else
            {
                agentController.BVHMovementControl(inAnimation);
            }

            


            /*for(int i = 0; i < mainList.Count; i++)
            {
                mainList[i].rotation = initialQuaternions[i];
            }*/
        }

        if (UseFixedGestures && l == 2)
        {
            float hand_timePassed = Time.time - hand_startTime;
            float body_timePassed = Time.time - body_startTime;
            float hand_t = hand_timePassed / hand_duration[hand_currentTransform];
            float body_t = body_timePassed / body_duration[body_currentTransform];

            HandIKTarget.transform.position = Vector3.Lerp(hand_transforms[hand_currentTransform].position, hand_transforms[hand_currentTransform + 1].position, hand_t);
            HandIKTarget.transform.rotation = Quaternion.Lerp(hand_transforms[hand_currentTransform].rotation, hand_transforms[hand_currentTransform + 1].rotation, hand_t);

            BodyIKTarget.transform.position = Vector3.Lerp(body_transforms[body_currentTransform].position, body_transforms[body_currentTransform + 1].position, body_t);
            BodyIKTarget.transform.rotation = Quaternion.Lerp(body_transforms[body_currentTransform].rotation, body_transforms[body_currentTransform + 1].rotation, body_t);

            HandPoleIKTarget.transform.position = fixedPoleTarget.position;

            //print(HandIKTarget.transform.rotation.eulerAngles);
            if (hand_timePassed >= hand_duration[hand_currentTransform])
            {
                hand_currentTransform++;
                if (hand_currentTransform >= hand_transforms.Length - 1)
                {
                    inAnimation = false;
                    agentController.BVHMovementControl(inAnimation, false);
                    // Your comment line here
                    hand_currentTransform = 0;
                    BodyIK.enabled = false;
                }
                hand_startTime = Time.time;
            }

            if (body_timePassed >= body_duration[body_currentTransform])
            {
                body_currentTransform++;
                if (body_currentTransform >= body_transforms.Length - 1)
                {
                    // Your comment line here
                    body_currentTransform = 0;
                    BodyIK.enabled = false;
                }
                body_startTime = Time.time;
            }
        }
        else if (UseFixedGestures && (c == 0 && l == 7 ||
                                       c == 1 && l == 8 ||
                                       c == 2 && l == 7 ||
                                       c == 3 && l == 7 ||
                                       c == 4 && l == 7 ||
                                       c == 5 && l == 7 ||
                                       c == 6 && l == 7 ||
                                       c == 7 && l == 7 ||
                                       c == 8 && l == 9 ||
                                       c == 9 && l == 7))
        {

            if(hand_currentTransform > 0 && body_currentTransform > 0)
            {
                float hand_timePassed = Time.time - hand_startTime;
                float body_timePassed = Time.time - body_startTime;
                float hand_t = hand_timePassed / hand_duration[hand_currentTransform - 1];
                float body_t = body_timePassed / body_duration[body_currentTransform - 1];

                HandIKTarget.transform.position = Vector3.Lerp(hand_transforms[hand_currentTransform].position, hand_transforms[hand_currentTransform - 1].position, hand_t);
                HandIKTarget.transform.rotation = Quaternion.Lerp(hand_transforms[hand_currentTransform].rotation, hand_transforms[hand_currentTransform - 1].rotation, hand_t);

                BodyIKTarget.transform.position = Vector3.Lerp(body_transforms[body_currentTransform].position, body_transforms[body_currentTransform - 1].position, body_t);
                BodyIKTarget.transform.rotation = Quaternion.Lerp(body_transforms[body_currentTransform].rotation, body_transforms[body_currentTransform - 1].rotation, body_t);

                HandPoleIKTarget.transform.position = fixedPoleTarget.position;

                //print(HandIKTarget.transform.rotation.eulerAngles);
                if (hand_timePassed >= hand_duration[hand_currentTransform - 1])
                {
                    hand_currentTransform--;
                    if (hand_currentTransform <= 0)
                    {
                        inAnimation = false;
                        agentController.BVHMovementControl(inAnimation, false);
                        // Your comment line here
                        hand_currentTransform = 0;
                        BodyIK.enabled = false;
                    }
                    hand_startTime = Time.time;
                }

                if (body_timePassed >= body_duration[body_currentTransform - 1])
                {
                    body_currentTransform--;
                    if (body_currentTransform <= 0)
                    {
                        inAnimation = false;
                        agentController.BVHMovementControl(inAnimation, false);
                        // Your comment line here
                        body_currentTransform = 0;
                        BodyIK.enabled = false;
                    }
                    body_startTime = Time.time;
                }
            }
            
        }
    }
}
