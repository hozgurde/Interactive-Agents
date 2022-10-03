using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using DitzelGames.FastIK;

public class BodyIKController : MonoBehaviour
{

    public Transform Spine;
    public Transform Spine1;
    public Transform Spine2;

    public GameObject TargetBody;
    public GameObject PoleBody;

    [Header("Length Multiplier")]
    public float waistMultiplier = 1f;
    public float backMultiplier = 1f;

    [Header("Back Rotation")]
    [Range(-180f, 180f)] public float BackRotZ = 0;
    [Range(-180f, 180f)] public float BackRotY = 0;
    [Range(-180f, 180f)] public float BackRotX = 0;

    [Header("Transition")]
    public bool OpenTransition = true;
    public float enterTransition = 2f;
    public float exitTransition = 5f;
    private bool isInTransition = true;

    private Vector3 initialWaistPos;
    private Vector3 initialBackPos;
    private float transitionTime = 0f;
    private Vector3 finalWaistPos;
    private Vector3 finalBackPos;

    //Transition in animation
    private Vector3 prevWaist;
    private Vector3 prevBack;

    // Unity Body Parts
    float U_BackLength;
    float U_WaistLength;

    // STG Output Body Parts
    float STG_BackLength;
    float STG_WaistLength;

    //*******************************************************************

    float[][]
        b_e1_c0_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        b_e1_c1_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        b_e1_c2_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        b_e1_c3_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        b_e1_c4_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        b_e1_c5_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        b_e1_c6_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        b_e1_c7_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        b_e1_c8_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        b_e1_c9_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        b_e1_c0_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        b_e1_c1_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        b_e1_c2_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        b_e1_c3_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        b_e1_c4_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        b_e1_c5_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        b_e1_c6_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        b_e1_c7_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        b_e1_c8_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        b_e1_c9_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        w_e1_c0_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        w_e1_c1_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        w_e1_c2_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        w_e1_c3_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        w_e1_c4_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        w_e1_c5_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        w_e1_c6_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        w_e1_c7_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        w_e1_c8_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        w_e1_c9_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        w_e1_c0_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        w_e1_c1_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        w_e1_c2_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        w_e1_c3_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        w_e1_c4_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        w_e1_c5_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        w_e1_c6_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        w_e1_c7_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        w_e1_c8_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        w_e1_c9_y = new float[][] { new float[0], new float[0], new float[0] };

    float[] xInputWaist = new float[0];

    float[] yInputWaist = new float[0];

    float[] xInputBack = new float[0];

    float[] yInputBack = new float[0];

    int currentSize = 0;

    [Header("Animation Variables")]

    public int index = 0;

    public int c;

    public int l;

    float timePassed = 0.0f;

    const float TIME_INTERVAL = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        c = -1;
        l = -1;
        string line;
        float f;

        isInTransition = OpenTransition;

        prevWaist = Spine1.position;
        prevBack = Spine2.position;

        string path = "./Assets/Script/Speech2Gesture/s2gData/waistX.txt";

        StreamReader reader = new StreamReader(path);

        currentSize = 0;
        while ((line = reader.ReadLine()) != null)
        {
            //line = line.Replace('.', ',');
            //print(line);
            f = float.Parse(line);
            //print(f);
            Array.Resize(ref xInputWaist, currentSize + 1);
            xInputWaist[currentSize] = f;
            //print("in array " + xInputBack[currentSize]);
            currentSize++;
        }

        currentSize = 0;
        path = "./Assets/Script/Speech2Gesture/s2gData/waistY.txt";
        reader = new StreamReader(path);

        while ((line = reader.ReadLine()) != null)
        {
            //line = line.Replace('.', ',');
            f = float.Parse(line);
            Array.Resize(ref yInputWaist, currentSize + 1);
            yInputWaist[currentSize] = f;
            currentSize++;
        }

        currentSize = 0;
        path = "./Assets/Script/Speech2Gesture/s2gData/backX.txt";
        reader = new StreamReader(path);

        while ((line = reader.ReadLine()) != null)
        {
            //line = line.Replace('.', ',');
            //print(line);
            f = float.Parse(line);
            //print(f);
            Array.Resize(ref xInputBack, currentSize + 1);
            xInputBack[currentSize] = f;
            //print("in array " + xInputWaist[currentSize]);
            currentSize++;
        }

        currentSize = 0;
        path = "./Assets/Script/Speech2Gesture/s2gData/backY.txt";
        reader = new StreamReader(path);

        while ((line = reader.ReadLine()) != null)
        {
            //line = line.Replace('.', ',');
            f = float.Parse(line);
            Array.Resize(ref yInputBack, currentSize + 1);
            yInputBack[currentSize] = f;
            currentSize++;
        }

        float[][] arrXWaist;
        float[][] arrYWaist;
        float[][] arrXBack;
        float[][] arrYBack;

        for (int c = 0; c < 10; c++)
        {
            if (c == 0)
            {
                arrXWaist = w_e1_c0_x;
                arrYWaist = w_e1_c0_y;
                arrXBack = b_e1_c0_x;
                arrYBack = b_e1_c0_y;
            }
            else if (c == 1)
            {
                arrXWaist = w_e1_c1_x;
                arrYWaist = w_e1_c1_y;
                arrXBack = b_e1_c1_x;
                arrYBack = b_e1_c1_y;
            }
            else if (c == 2)
            {
                arrXWaist = w_e1_c2_x;
                arrYWaist = w_e1_c2_y;
                arrXBack = b_e1_c2_x;
                arrYBack = b_e1_c2_y;
            }
            else if (c == 3)
            {
                arrXWaist = w_e1_c3_x;
                arrYWaist = w_e1_c3_y;
                arrXBack = b_e1_c3_x;
                arrYBack = b_e1_c3_y;
            }
            else if (c == 4)
            {
                arrXWaist = w_e1_c4_x;
                arrYWaist = w_e1_c4_y;
                arrXBack = b_e1_c4_x;
                arrYBack = b_e1_c4_y;
            }
            else if (c == 5)
            {
                arrXWaist = w_e1_c5_x;
                arrYWaist = w_e1_c5_y;
                arrXBack = b_e1_c5_x;
                arrYBack = b_e1_c5_y;
            }
            else if (c == 6)
            {
                arrXWaist = w_e1_c6_x;
                arrYWaist = w_e1_c6_y;
                arrXBack = b_e1_c6_x;
                arrYBack = b_e1_c6_y;
            }
            else if (c == 7)
            {
                arrXWaist = w_e1_c7_x;
                arrYWaist = w_e1_c7_y;
                arrXBack = b_e1_c7_x;
                arrYBack = b_e1_c7_y;
            }
            else if (c == 8)
            {
                arrXWaist = w_e1_c8_x;
                arrYWaist = w_e1_c8_y;
                arrXBack = b_e1_c8_x;
                arrYBack = b_e1_c8_y;
            }
            else
            {
                arrXWaist = w_e1_c9_x;
                arrYWaist = w_e1_c9_y;
                arrXBack = b_e1_c9_x;
                arrYBack = b_e1_c9_y;
            }
        

            for (int l = 0; l < 3; l++)
            {
                path =
                    "./Assets/Script/Speech2Gesture/data/E1_C" +
                    c +
                    "/E1_C" +
                    c +
                    "_L" +
                    (2 * l + 1) +
                    "_A_TTS";

                currentSize = 0;
                reader = new StreamReader(path + "/backX.txt");

                while ((line = reader.ReadLine()) != null)
                {
                    //line = line.Replace('.', ',');
                    f = float.Parse(line);
                    Array.Resize(ref arrXBack[l], currentSize + 1);
                    arrXBack[l][currentSize] = f;
                    currentSize++;
                }

                currentSize = 0;
                reader = new StreamReader(path + "/backY.txt");

                while ((line = reader.ReadLine()) != null)
                {
                    //line = line.Replace('.', ',');
                    f = float.Parse(line);
                    Array.Resize(ref arrYBack[l], currentSize + 1);
                    arrYBack[l][currentSize] = f;
                    currentSize++;
                }

                currentSize = 0;
                reader = new StreamReader(path + "/waistX.txt");

                while ((line = reader.ReadLine()) != null)
                {
                    //line = line.Replace('.', ',');
                    f = float.Parse(line);
                    Array.Resize(ref arrXWaist[l], currentSize + 1);
                    arrXWaist[l][currentSize] = f;
                    currentSize++;
                }

                currentSize = 0;
                reader = new StreamReader(path + "/waistY.txt");

                while ((line = reader.ReadLine()) != null)
                {
                    //line = line.Replace('.', ',');
                    f = float.Parse(line);
                    Array.Resize(ref arrYWaist[l], currentSize + 1);
                    arrYWaist[l][currentSize] = f;
                    currentSize++;
                }
            }
        }

    //Relative Size Calculation


    reader.Close();

    }

    // Update is called once per frame
    void Update()
    {
        if (c != -1 && l != -1 && l < 3)//temp
        {
            print("in c");
            if (c == 0)
            {
                xInputWaist = w_e1_c0_x[l];
                yInputWaist = w_e1_c0_y[l];
                xInputBack = b_e1_c0_x[l];
                yInputBack = b_e1_c0_y[l];
            }
            else if (c == 1)
            {
                xInputWaist = w_e1_c1_x[l];
                yInputWaist = w_e1_c1_y[l];
                xInputBack = b_e1_c1_x[l];
                yInputBack = b_e1_c1_y[l];
            }
            else if (c == 2)
            {
                xInputWaist = w_e1_c2_x[l];
                yInputWaist = w_e1_c2_y[l];
                xInputBack = b_e1_c2_x[l];
                yInputBack = b_e1_c2_y[l];
            }
            else if (c == 3)
            {
                xInputWaist = w_e1_c3_x[l];
                yInputWaist = w_e1_c3_y[l];
                xInputBack = b_e1_c3_x[l];
                yInputBack = b_e1_c3_y[l];
            }
            else if (c == 4)
            {
                xInputWaist = w_e1_c4_x[l];
                yInputWaist = w_e1_c4_y[l];
                xInputBack = b_e1_c4_x[l];
                yInputBack = b_e1_c4_y[l];
            }
            else if (c == 5)
            {
                xInputWaist = w_e1_c5_x[l];
                yInputWaist = w_e1_c5_y[l];
                xInputBack = b_e1_c5_x[l];
                yInputBack = b_e1_c5_y[l];
            }
            else if (c == 6)
            {
                xInputWaist = w_e1_c6_x[l];
                yInputWaist = w_e1_c6_y[l];
                xInputBack = b_e1_c6_x[l];
                yInputBack = b_e1_c6_y[l];
            }
            else if (c == 7)
            {
                xInputWaist = w_e1_c7_x[l];
                yInputWaist = w_e1_c7_y[l];
                xInputBack = b_e1_c7_x[l];
                yInputBack = b_e1_c7_y[l];
            }
            else if (c == 8)
            {
                xInputWaist = w_e1_c8_x[l];
                yInputWaist = w_e1_c8_y[l];
                xInputBack = b_e1_c8_x[l];
                yInputBack = b_e1_c8_y[l];
            }
            else
            {
                xInputWaist = w_e1_c9_x[l];
                yInputWaist = w_e1_c9_y[l];
                xInputBack = b_e1_c9_x[l];
                yInputBack = b_e1_c9_y[l];
            }
        }

        Vector3 wanted_waist_pos = new Vector3();
        Vector3 d_back_U = new Vector3();

        if (index < xInputWaist.Length)
        {
            if (index == 0)
            {
                U_WaistLength = (Spine1.position - Spine.position).magnitude;
                U_BackLength = (Spine2.position - Spine1.position).magnitude;
            }

            if (index == 0 && transitionTime <= 0)
            {
                initialWaistPos = Spine1.position;
                initialBackPos = Spine2.position;
                isInTransition = true;
                transitionTime = enterTransition;
                Spine2.GetComponent<FastIKFabric>().enabled = true;
            }

            //Find Back Position
            Vector3 prev_d_back_U = Spine2.position - Spine1.position;

            float z_xy_ratio = prev_d_back_U.z / Mathf.Sqrt(prev_d_back_U.x * prev_d_back_U.x + prev_d_back_U.y * prev_d_back_U.y);

            Vector3 d_back_STG = new Vector3(xInputBack[index], yInputBack[index], z_xy_ratio * Mathf.Sqrt(xInputBack[index] * xInputBack[index] + yInputBack[index] * yInputBack[index]));

            if (index == 0)
            {
                STG_BackLength = d_back_STG.magnitude;
            }

            //print("STG la length: " + STG_backLength);
            //print("U la length: " + U_backLength);

            d_back_U = d_back_STG.normalized * U_BackLength * backMultiplier;


            //Find Waist Position
            Vector3 prev_d_waist_U = Spine1.position - Spine.position;

            z_xy_ratio = prev_d_waist_U.z / Mathf.Sqrt(prev_d_waist_U.x * prev_d_waist_U.x + prev_d_waist_U.y * prev_d_waist_U.y);

            Vector3 d_waist_STG = new Vector3(xInputWaist[index], yInputWaist[index], z_xy_ratio * Mathf.Sqrt(xInputWaist[index] * xInputWaist[index] + yInputWaist[index] * yInputWaist[index]));

            if (index == 0)
            {
                STG_WaistLength = d_waist_STG.magnitude;
            }

            //print("STG ua length: " + STG_WaistLength);
            //print("U ua length: " + U_WaistLength);
            

            

            Vector3 d_waist_U = d_waist_STG.normalized * U_WaistLength * waistMultiplier;


            //Transform Waist

            //p3
            wanted_waist_pos = Spine.position + d_waist_U;
            //t
            Vector3 shoulder_back_vector = Spine.position - TargetBody.transform.position;
            //m
            Vector3 shoulder_waist_vector = wanted_waist_pos - Spine.position;
            //k
            float pos_mult = -(shoulder_back_vector.x * shoulder_waist_vector.x + shoulder_back_vector.y * shoulder_waist_vector.y + shoulder_back_vector.z * shoulder_waist_vector.z) /
                (shoulder_back_vector.x * shoulder_back_vector.x + shoulder_back_vector.y * shoulder_back_vector.y + shoulder_back_vector.z * shoulder_back_vector.z);
            //d1
            Vector3 projection_s_h = pos_mult * TargetBody.transform.position + (1 - pos_mult) * Spine.position;

            //new pos
            PoleBody.transform.position = wanted_waist_pos;//+ 10 * (wanted_waist_pos - projection_s_h);

            //Transform Back
            TargetBody.transform.position = wanted_waist_pos + d_back_U;

            float animationTransition = Mathf.Clamp(timePassed / TIME_INTERVAL, 0, 1);

            TargetBody.transform.position = animationTransition * TargetBody.transform.position + prevBack * (1 - animationTransition);
            PoleBody.transform.position = animationTransition * PoleBody.transform.position + prevWaist * (1 - animationTransition);

            //Start Transition
            if (index == xInputBack.Length - 1 && !isInTransition)
            {
                finalWaistPos = PoleBody.transform.position;
                finalBackPos = TargetBody.transform.position;
                transitionTime = exitTransition;
                isInTransition = OpenTransition;
            }

        }

        //Transitioning
        if (isInTransition && transitionTime > 0f)
        {
            transitionTime -= Time.deltaTime;
            float transitionRatio;
            if (index < xInputBack.Length - 1)
            {
                transitionRatio = transitionTime / enterTransition;
                TargetBody.transform.position = initialBackPos * transitionRatio + TargetBody.transform.position * (1 - transitionRatio);
                PoleBody.transform.position = initialWaistPos * transitionRatio + PoleBody.transform.position * (1 - transitionRatio);
            }
            else
            {
                transitionRatio = 1 - transitionTime / exitTransition;
                TargetBody.transform.position = initialBackPos * transitionRatio + finalBackPos * (1 - transitionRatio);
                PoleBody.transform.position = initialWaistPos * transitionRatio + finalWaistPos * (1 - transitionRatio);
            }
        }
        else if (isInTransition && transitionTime <= 0f)
        {
            isInTransition = false;
            if(index >= xInputBack.Length - 1)
                Spine2.GetComponent<FastIKFabric>().enabled = false;
        }

        TargetBody.transform.rotation = Spine1.rotation;
        TargetBody.transform.Rotate(new Vector3(0, 0, 1), BackRotZ);
        TargetBody.transform.Rotate(new Vector3(0, 1, 0), BackRotY);
        TargetBody.transform.Rotate(new Vector3(1, 0, 0), BackRotX);



        timePassed += Time.deltaTime;

        if (timePassed >= TIME_INTERVAL)
        {
            timePassed = 0.0f;
            index++;
            prevBack = wanted_waist_pos + d_back_U;
            prevWaist = wanted_waist_pos;
        }
    }
}
