using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVHAnimationController : MonoBehaviour
{

    public int c;

    public int l;

    private BVHAnimationLoader bVHAnimationLoader;

    private string currentBVHFile;

    private const string startPath = "Assets/Script/Speech2Gesture/ZeggsData/E1_C";

    // Start is called before the first frame update
    void Start()
    {
        c = -1;
        l = -1;
        bVHAnimationLoader = gameObject.GetComponent<BVHAnimationLoader>();
        currentBVHFile = "Assets/Script/Speech2Gesture/ZeggsData/audio_test_label_001_Neutral_0_x_1_0.bvh";
    }

    // Update is called once per frame
    void Update()
    {
        if (c != -1 && l != -1 && l < 3 && !currentBVHFile.Equals(startPath + c.ToString() + "/E1_C" + c.ToString() + "_L" + (2*l + 1).ToString() + "_A_TTS.bvh"))
        {
            currentBVHFile = startPath + c.ToString() + "/E1_C" + c.ToString() + "_L" + (2 * l + 1).ToString() + "_A_TTS.bvh";
            bVHAnimationLoader.filename = currentBVHFile;
            bVHAnimationLoader.parseFile();
            bVHAnimationLoader.loadAnimation();
        }
    }
}
