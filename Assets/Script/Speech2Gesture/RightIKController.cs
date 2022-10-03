using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using DitzelGames.FastIK;

public class RightIKController : MonoBehaviour
{
    public Transform RightShoulder;
    public Transform RightElbow;
    public Transform RightHand;

    public GameObject TargetRightElbow;
    public GameObject TargetRightHand;

    public AgentController agentController;

    [Header("Length Multiplier")]
    public float lowerArmMultiplier = 1f;
    public float upperArmMultiplier = 1f;

    [Header("Hand Rotation")]
    [Range(-180f, 180f)] public float HandRotZ = 90;
    [Range(-180f, 180f)] public float HandRotY = 0;
    [Range(-180f, 180f)] public float HandRotX = 0;

    [Header("Finger Rotation")]
    public bool OpenFingerRotation = true;
    public float maxFingerRotation = 30f;
    public float minFingerRotation = -15f;
    public float fingerRotation = 0f;
    public float changeInterval = 360f;
    public float transitionMultiplier = 5f;
    private bool canRotateFinger = true;

    [Header("Transition")]
    public bool OpenTransition = true;
    public float enterTransition = 2f;
    public float exitTransition = 5f;
    private bool isInTransition = true;

    private Vector3 initialHandPos;
    private Vector3 initialElbowPos;
    private float transitionTime = 0f;
    private Vector3 finalHandPos;
    private Vector3 finalElbowPos;

    //Transition in animation
    private Vector3 prevRightHand;
    private Vector3 prevRightElbow;

    // Unity Body Parts
    float U_UpperArmLength;
    float U_LowerArmLength;

    // STG Output Body Parts
    float STG_UpperArmLength;
    float STG_LowerArmLength;

    // Unity Coordinates *****************************************
    public const float X_MIN = 0.25f;

    public const float X_MAX = 0.45f;

    public const float Y_MIN = 0.94f;

    public const float Y_MAX = 1.7f;

    public const float Z_MIN = 0f;

    public const float Z_MAX = -0.4f;

    // ***********************************************************
    // Speech to Gesture Output Coordinates **********************
    public const float X_MIN_INPUT = 0f;

    public const float X_MAX_INPUT = 40f;

    public const float Y_MIN_INPUT = 50f;

    public const float Y_MAX_INPUT = 90f;

    public const float Z_MIN_INPUT = 0f;

    public const float Z_MAX_INPUT = 40f;

    // ***********************************************************
    float[][]
           e_e1_c0_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e_e1_c1_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e_e1_c2_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e_e1_c3_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e_e1_c4_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e_e1_c5_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e_e1_c6_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e_e1_c7_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e_e1_c8_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e_e1_c9_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e_e1_c0_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e_e1_c1_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e_e1_c2_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e_e1_c3_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e_e1_c4_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e_e1_c5_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e_e1_c6_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e_e1_c7_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e_e1_c8_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e_e1_c9_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e_e1_c0_z = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e_e1_c1_z = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e_e1_c2_z = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e_e1_c3_z = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e_e1_c4_z = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e_e1_c5_z = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e_e1_c6_z = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e_e1_c7_z = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e_e1_c8_z = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e_e1_c9_z = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        h_e1_c0_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        h_e1_c1_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        h_e1_c2_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        h_e1_c3_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        h_e1_c4_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        h_e1_c5_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        h_e1_c6_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        h_e1_c7_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        h_e1_c8_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        h_e1_c9_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        h_e1_c0_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        h_e1_c1_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        h_e1_c2_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        h_e1_c3_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        h_e1_c4_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        h_e1_c5_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        h_e1_c6_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        h_e1_c7_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        h_e1_c8_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        h_e1_c9_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        h_e1_c0_z = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        h_e1_c1_z = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        h_e1_c2_z = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        h_e1_c3_z = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        h_e1_c4_z = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        h_e1_c5_z = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        h_e1_c6_z = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        h_e1_c7_z = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        h_e1_c8_z = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        h_e1_c9_z = new float[][] { new float[0], new float[0], new float[0] };

    float[] xInputShoulder = new float[0];

    float[] yInputShoulder = new float[0];

    float[] xInputElbow = new float[0];

    float[] yInputElbow = new float[0];

    float[] zInputElbow = new float[0];

    float[] xInputHand = new float[0];

    float[] yInputHand = new float[0];

    float[] zInputHand = new float[0];

    int currentSize = 0;

    [Header("Animation Variables")]

    public int index = 0;

    public int c;

    public int l;

    float xAngle = 0;

    float yAngle = 0;

    float zAngle = 0;

    float timePassed = 0.0f;

    const float TIME_INTERVAL = 0.05f;

    Vector3 normalizeDeltaUpperArm(Vector3 input)
    {
        return input / STG_UpperArmLength * U_UpperArmLength;
    }

    Vector3 normalizeDeltaLowerArm(Vector3 input)
    {
        return input / STG_LowerArmLength * U_LowerArmLength;
    }

    float normalizeX(float input)
    {
        float ratio = (input - X_MIN_INPUT) / (X_MAX_INPUT - X_MIN_INPUT);
        return (X_MAX - X_MIN) * ratio + X_MIN;
    }

    float normalizeY(float input)
    {
        float ratio = (input - Y_MIN_INPUT) / (Y_MAX_INPUT - Y_MIN_INPUT);
        return (Y_MAX - Y_MIN) * ratio + Y_MIN;
    }

    float normalizeZ(float input)
    {
        float ratio = (input - Z_MIN_INPUT) / (Z_MAX_INPUT - Z_MIN_INPUT);
        return (Z_MAX - Z_MIN) * ratio + Z_MIN;
    }

    float angle(float x, float y, float z, int axis)
    {
        Vector3 v;

        if (axis == 1)
        {
            v = new Vector3(1f, 0f, 0f);
        }
        else if (axis == 2)
        {
            v = new Vector3(0f, 1f, 0f);
        }
        else
        {
            v = new Vector3(0f, 0f, 1f);
        }

        float angle = Vector3.Angle(new Vector3(x, y, z), v);

        return angle;
    }

    void Start()
    {

        c = -1;
        l = -1;
        string line;
        float f;

        RightHand.GetComponent<FastIKFabric>().enabled = true;

        isInTransition = OpenTransition;

        prevRightHand = RightHand.position;
        prevRightElbow = RightElbow.position;

        /*string path = "./Assets/Script/Speech2Gesture/s2gData/leftShoulderX.txt";

        StreamReader reader = new StreamReader(path);

        while ((line = reader.ReadLine()) != null)
        {
            //line = line.Replace('.', ',');
            //print(line);
            f = float.Parse(line);
            //print(f);
            Array.Resize(ref xInputShoulder, currentSize + 1);
            xInputShoulder[currentSize] = f;
            //print("in array " + xInputShoulder[currentSize]);
            currentSize++;
        }

        currentSize = 0;
        path = "./Assets/Script/Speech2Gesture/s2gData/leftShoulderY.txt";
        reader = new StreamReader(path);

        while ((line = reader.ReadLine()) != null)
        {
            //line = line.Replace('.', ',');
            f = float.Parse(line);
            Array.Resize(ref yInputShoulder, currentSize + 1);
            yInputShoulder[currentSize] = f;
            currentSize++;
        }*/


        string path = "./Assets/Script/Speech2Gesture/s2gData/rightElbowX.txt";

        StreamReader reader = new StreamReader(path);

        currentSize = 0;
        while ((line = reader.ReadLine()) != null)
        {
            //line = line.Replace('.', ',');
            //print(line);
            f = float.Parse(line);
            //print(f);
            Array.Resize(ref xInputElbow, currentSize + 1);
            xInputElbow[currentSize] = f;
            //print("in array " + xInputElbow[currentSize]);
            currentSize++;
        }

        currentSize = 0;
        path = "./Assets/Script/Speech2Gesture/s2gData/rightElbowY.txt";
        reader = new StreamReader(path);

        while ((line = reader.ReadLine()) != null)
        {
            //line = line.Replace('.', ',');
            f = float.Parse(line);
            Array.Resize(ref yInputElbow, currentSize + 1);
            yInputElbow[currentSize] = f;
            currentSize++;
        }

        currentSize = 0;
        path = "./Assets/Script/Speech2Gesture/s2gData/rightElbowZ.txt";
        reader = new StreamReader(path);

        while ((line = reader.ReadLine()) != null)
        {
            //line = line.Replace('.', ',');
            f = float.Parse(line);
            Array.Resize(ref zInputElbow, currentSize + 1);
            zInputElbow[currentSize] = f;
            currentSize++;
        }

        currentSize = 0;
        path = "./Assets/Script/Speech2Gesture/s2gData/rightHandX.txt";
        reader = new StreamReader(path);

        while ((line = reader.ReadLine()) != null)
        {
            //line = line.Replace('.', ',');
            //print(line);
            f = float.Parse(line);
            //print(f);
            Array.Resize(ref xInputHand, currentSize + 1);
            xInputHand[currentSize] = f;
            //print("in array " + xInputHand[currentSize]);
            currentSize++;
        }

        currentSize = 0;
        path = "./Assets/Script/Speech2Gesture/s2gData/rightHandY.txt";
        reader = new StreamReader(path);

        while ((line = reader.ReadLine()) != null)
        {
            //line = line.Replace('.', ',');
            f = float.Parse(line);
            Array.Resize(ref yInputHand, currentSize + 1);
            yInputHand[currentSize] = f;
            currentSize++;
        }

        currentSize = 0;
        path = "./Assets/Script/Speech2Gesture/s2gData/rightHandZ.txt";
        reader = new StreamReader(path);

        while ((line = reader.ReadLine()) != null)
        {
            //line = line.Replace('.', ',');
            f = float.Parse(line);
            Array.Resize(ref zInputHand, currentSize + 1);
            zInputHand[currentSize] = f;
            currentSize++;
        }

        float[][] arrXHand;
        float[][] arrYHand;
        float[][] arrZHand;
        float[][] arrXElbow;
        float[][] arrYElbow;
        float[][] arrZElbow;

        for (int c = 0; c < 10; c++)
        {
            if (c == 0)
            {
                arrXHand = h_e1_c0_x;
                arrYHand = h_e1_c0_y;
                arrZHand = h_e1_c0_z;
                arrXElbow = e_e1_c0_x;
                arrYElbow = e_e1_c0_y;
                arrZElbow = e_e1_c0_z;
            }
            else if (c == 1)
            {
                arrXHand = h_e1_c1_x;
                arrYHand = h_e1_c1_y;
                arrZHand = h_e1_c1_z;
                arrXElbow = e_e1_c1_x;
                arrYElbow = e_e1_c1_y;
                arrZElbow = e_e1_c1_z;
            }
            else if (c == 2)
            {
                arrXHand = h_e1_c2_x;
                arrYHand = h_e1_c2_y;
                arrZHand = h_e1_c2_z;
                arrXElbow = e_e1_c2_x;
                arrYElbow = e_e1_c2_y;
                arrZElbow = e_e1_c2_z;
            }
            else if (c == 3)
            {
                arrXHand = h_e1_c3_x;
                arrYHand = h_e1_c3_y;
                arrZHand = h_e1_c3_z;
                arrXElbow = e_e1_c3_x;
                arrYElbow = e_e1_c3_y;
                arrZElbow = e_e1_c3_z;
            }
            else if (c == 4)
            {
                arrXHand = h_e1_c4_x;
                arrYHand = h_e1_c4_y;
                arrZHand = h_e1_c4_z;
                arrXElbow = e_e1_c4_x;
                arrYElbow = e_e1_c4_y;
                arrZElbow = e_e1_c4_z;
            }
            else if (c == 5)
            {
                arrXHand = h_e1_c5_x;
                arrYHand = h_e1_c5_y;
                arrZHand = h_e1_c5_z;
                arrXElbow = e_e1_c5_x;
                arrYElbow = e_e1_c5_y;
                arrZElbow = e_e1_c5_z;
            }
            else if (c == 6)
            {
                arrXHand = h_e1_c6_x;
                arrYHand = h_e1_c6_y;
                arrZHand = h_e1_c6_z;
                arrXElbow = e_e1_c6_x;
                arrYElbow = e_e1_c6_y;
                arrZElbow = e_e1_c6_z;
            }
            else if (c == 7)
            {
                arrXHand = h_e1_c7_x;
                arrYHand = h_e1_c7_y;
                arrZHand = h_e1_c7_z;
                arrXElbow = e_e1_c7_x;
                arrYElbow = e_e1_c7_y;
                arrZElbow = e_e1_c7_z;
            }
            else if (c == 8)
            {
                arrXHand = h_e1_c8_x;
                arrYHand = h_e1_c8_y;
                arrZHand = h_e1_c8_z;
                arrXElbow = e_e1_c8_x;
                arrYElbow = e_e1_c8_y;
                arrZElbow = e_e1_c8_z;
            }
            else
            {
                arrXHand = h_e1_c9_x;
                arrYHand = h_e1_c9_y;
                arrZHand = h_e1_c9_z;
                arrXElbow = e_e1_c9_x;
                arrYElbow = e_e1_c9_y;
                arrZElbow = e_e1_c9_z;
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
                reader = new StreamReader(path + "/rightElbowX.txt");

                while ((line = reader.ReadLine()) != null)
                {
                    //line = line.Replace('.', ',');
                    f = float.Parse(line);
                    Array.Resize(ref arrXElbow[l], currentSize + 1);
                    arrXElbow[l][currentSize] = f;
                    currentSize++;
                }

                currentSize = 0;
                reader = new StreamReader(path + "/rightElbowY.txt");

                while ((line = reader.ReadLine()) != null)
                {
                    //line = line.Replace('.', ',');
                    f = float.Parse(line);
                    Array.Resize(ref arrYElbow[l], currentSize + 1);
                    arrYElbow[l][currentSize] = f;
                    currentSize++;
                }

                currentSize = 0;
                reader = new StreamReader(path + "/rightElbowZ.txt");

                while ((line = reader.ReadLine()) != null)
                {
                    //line = line.Replace('.', ',');
                    f = float.Parse(line);
                    Array.Resize(ref arrZElbow[l], currentSize + 1);
                    arrZElbow[l][currentSize] = f;
                    currentSize++;
                }

                currentSize = 0;
                reader = new StreamReader(path + "/rightHandX.txt");

                while ((line = reader.ReadLine()) != null)
                {
                    //line = line.Replace('.', ',');
                    f = float.Parse(line);
                    Array.Resize(ref arrXHand[l], currentSize + 1);
                    arrXHand[l][currentSize] = f;
                    currentSize++;
                }

                currentSize = 0;
                reader = new StreamReader(path + "/rightHandY.txt");

                while ((line = reader.ReadLine()) != null)
                {
                    //line = line.Replace('.', ',');
                    f = float.Parse(line);
                    Array.Resize(ref arrYHand[l], currentSize + 1);
                    arrYHand[l][currentSize] = f;
                    currentSize++;
                }

                currentSize = 0;
                reader = new StreamReader(path + "/rightHandZ.txt");

                while ((line = reader.ReadLine()) != null)
                {
                    //line = line.Replace('.', ',');
                    f = float.Parse(line);
                    Array.Resize(ref arrZHand[l], currentSize + 1);
                    arrZHand[l][currentSize] = f;
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

        //print(c);

        if (c != -1 && l != -1 && l < 3)//temp
        {
            if (c == 0)
            {
                xInputHand = h_e1_c0_x[l];
                yInputHand = h_e1_c0_y[l];
                zInputHand = h_e1_c0_z[l];
                xInputElbow = e_e1_c0_x[l];
                yInputElbow = e_e1_c0_y[l];
                zInputElbow = e_e1_c0_z[l];
            }
            else if (c == 1)
            {
                xInputHand = h_e1_c1_x[l];
                yInputHand = h_e1_c1_y[l];
                zInputHand = h_e1_c1_z[l];
                xInputElbow = e_e1_c1_x[l];
                yInputElbow = e_e1_c1_y[l];
                zInputElbow = e_e1_c1_z[l];
            }
            else if (c == 2)
            {
                xInputHand = h_e1_c2_x[l];
                yInputHand = h_e1_c2_y[l];
                zInputHand = h_e1_c2_z[l];
                xInputElbow = e_e1_c2_x[l];
                yInputElbow = e_e1_c2_y[l];
                zInputElbow = e_e1_c2_z[l];
            }
            else if (c == 3)
            {
                xInputHand = h_e1_c3_x[l];
                yInputHand = h_e1_c3_y[l];
                zInputHand = h_e1_c3_z[l];
                xInputElbow = e_e1_c3_x[l];
                yInputElbow = e_e1_c3_y[l];
                zInputElbow = e_e1_c3_z[l];
            }
            else if (c == 4)
            {
                xInputHand = h_e1_c4_x[l];
                yInputHand = h_e1_c4_y[l];
                zInputHand = h_e1_c4_z[l];
                xInputElbow = e_e1_c4_x[l];
                yInputElbow = e_e1_c4_y[l];
                zInputElbow = e_e1_c4_z[l];
            }
            else if (c == 5)
            {
                xInputHand = h_e1_c5_x[l];
                yInputHand = h_e1_c5_y[l];
                zInputHand = h_e1_c5_z[l];
                xInputElbow = e_e1_c5_x[l];
                yInputElbow = e_e1_c5_y[l];
                zInputElbow = e_e1_c5_z[l];
            }
            else if (c == 6)
            {
                xInputHand = h_e1_c6_x[l];
                yInputHand = h_e1_c6_y[l];
                zInputHand = h_e1_c6_z[l];
                xInputElbow = e_e1_c6_x[l];
                yInputElbow = e_e1_c6_y[l];
                zInputElbow = e_e1_c6_z[l];
            }
            else if (c == 7)
            {
                xInputHand = h_e1_c7_x[l];
                yInputHand = h_e1_c7_y[l];
                zInputHand = h_e1_c7_z[l];
                xInputElbow = e_e1_c7_x[l];
                yInputElbow = e_e1_c7_y[l];
                zInputElbow = e_e1_c7_z[l];
            }
            else if (c == 8)
            {
                xInputHand = h_e1_c8_x[l];
                yInputHand = h_e1_c8_y[l];
                zInputHand = h_e1_c8_z[l];
                xInputElbow = e_e1_c8_x[l];
                yInputElbow = e_e1_c8_y[l];
                zInputElbow = e_e1_c8_z[l];
            }
            else
            {
                xInputHand = h_e1_c9_x[l];
                yInputHand = h_e1_c9_y[l];
                zInputHand = h_e1_c9_z[l];
                xInputElbow = e_e1_c9_x[l];
                yInputElbow = e_e1_c9_y[l];
                zInputElbow = e_e1_c9_z[l];
            }
        }

        Vector3 wanted_elbow_pos = new Vector3();
        Vector3 d_hand_U = new Vector3();

        if (index < xInputHand.Length)
        {

            if (index == 0 && transitionTime <= 0)
            {
                U_UpperArmLength = (RightShoulder.position - RightElbow.position).magnitude;
                U_LowerArmLength = (RightHand.position - RightElbow.position).magnitude;

                initialHandPos = RightHand.position;
                initialElbowPos = RightElbow.position;
                prevRightHand = initialHandPos;
                prevRightElbow = initialElbowPos;
                isInTransition = OpenTransition;
                if (isInTransition)
                {
                    changeInterval = changeInterval * transitionMultiplier;

                }
                transitionTime = enterTransition;
                RightHand.GetComponent<FastIKFabric>().enabled = true;
            }

            //Find Hand Position

            Vector3 d_hand_STG = new Vector3(xInputHand[index], yInputHand[index], -zInputHand[index]);

            if (index == 0)
            {
                STG_LowerArmLength = d_hand_STG.magnitude;
            }

            //print("STG la length: " + STG_LowerArmLength);
            //print("U la length: " + U_LowerArmLength);
            d_hand_U = d_hand_STG.normalized * U_LowerArmLength * lowerArmMultiplier;


            //Find Elbow Position

            Vector3 d_elbow_STG = new Vector3(xInputElbow[index], yInputElbow[index], -zInputElbow[index]);

            if (index == 0)
            {
                STG_UpperArmLength = d_elbow_STG.magnitude;
            }

            //print("STG ua length: " + STG_UpperArmLength);
            //print("U ua length: " + U_UpperArmLength);
            Vector3 d_elbow_U = d_elbow_STG.normalized * U_UpperArmLength * upperArmMultiplier;


            //Transform Elbow

            //p3
            wanted_elbow_pos = RightShoulder.position + d_elbow_U;
            //t
            Vector3 shoulder_hand_vector = RightShoulder.position - TargetRightHand.transform.position;
            //m
            Vector3 shoulder_elbow_vector = wanted_elbow_pos - RightShoulder.position;
            //k
            float pos_mult = -(shoulder_hand_vector.x * shoulder_elbow_vector.x + shoulder_hand_vector.y * shoulder_elbow_vector.y + shoulder_hand_vector.z * shoulder_elbow_vector.z) /
                (shoulder_hand_vector.x * shoulder_hand_vector.x + shoulder_hand_vector.y * shoulder_hand_vector.y + shoulder_hand_vector.z * shoulder_hand_vector.z);
            //d1
            Vector3 projection_s_h = pos_mult * TargetRightHand.transform.position + (1 - pos_mult) * RightShoulder.position;

            //new pos
            TargetRightElbow.transform.position = wanted_elbow_pos;//+ 10 * (wanted_elbow_pos - projection_s_h);


            //Transform Hand
            TargetRightHand.transform.position = wanted_elbow_pos + d_hand_U;

            float animationTransition = Mathf.Clamp(timePassed / TIME_INTERVAL, 0, 1);

            if (!OpenTransition && index == 0)
            {
                prevRightHand = TargetRightHand.transform.position;
                prevRightElbow = TargetRightElbow.transform.position;
            }

            TargetRightHand.transform.position = animationTransition * TargetRightHand.transform.position + prevRightHand * (1 - animationTransition);
            TargetRightElbow.transform.position = animationTransition * TargetRightElbow.transform.position + prevRightElbow * (1 - animationTransition);

            //Start Transition
            if (index == xInputHand.Length - 1 && !isInTransition && OpenTransition)
            {
                finalElbowPos = TargetRightElbow.transform.position;
                finalHandPos = TargetRightHand.transform.position;
                transitionTime = exitTransition;
                isInTransition = OpenTransition;
                //canRotateFinger = false;
                changeInterval = changeInterval * transitionMultiplier;
            }
        }

        //Transitioning
        if (isInTransition && transitionTime > 0f)
        {
            transitionTime -= Time.deltaTime;
            float transitionRatio;
            if (index < xInputHand.Length - 1)
            {
                transitionRatio = transitionTime / enterTransition;
                TargetRightHand.transform.position = initialHandPos * transitionRatio + TargetRightHand.transform.position * (1 - transitionRatio);
                TargetRightElbow.transform.position = initialElbowPos * transitionRatio + TargetRightElbow.transform.position * (1 - transitionRatio);
            }
            else
            {
                transitionRatio = 1 - transitionTime / exitTransition;
                TargetRightHand.transform.position = initialHandPos * transitionRatio + finalHandPos * (1 - transitionRatio);
                TargetRightElbow.transform.position = initialElbowPos * transitionRatio + finalElbowPos * (1 - transitionRatio);
            }
        }
        else if (isInTransition && transitionTime <= 0f)
        {
            //canRotateFinger = OpenFingerRotation;
            changeInterval = changeInterval / transitionMultiplier;
            isInTransition = false;

            if (index >= xInputHand.Length - 1)
                RightHand.GetComponent<FastIKFabric>().enabled = false;
        }

        //Rotate Fingers
        //if (canRotateFinger)
        if (OpenFingerRotation)
        {
            Vector3 d_ua = (RightElbow.position - RightShoulder.position).normalized;
            Vector3 d_la = (RightHand.position - RightElbow.position).normalized;
            float angle_between = Vector3.Angle(d_ua, d_la);
            angle_between = angle_between % changeInterval;
            if (angle_between > changeInterval / 2)
            {
                fingerRotation = (1 - (angle_between - (changeInterval / 2)) / (changeInterval / 2)) * (maxFingerRotation - minFingerRotation) + minFingerRotation;
            }
            else
            {
                fingerRotation = (angle_between / (changeInterval / 2)) * (maxFingerRotation - minFingerRotation) + minFingerRotation;
            }

        }



        //print(wanted_elbow_pos);
        //print(d_elbow_U);
        //print(d_hand_U);

        /*
        TargetLeftElbow.transform.position = LeftShoulder.position + d_elbow_U;
        */

        TargetRightHand.transform.rotation = RightElbow.rotation;
        TargetRightHand.transform.Rotate(new Vector3(0, 0, 1), HandRotZ);
        TargetRightHand.transform.Rotate(new Vector3(0, 1, 0), HandRotY);
        TargetRightHand.transform.Rotate(new Vector3(1, 0, 0), HandRotX);

        agentController.fingerRotationR = fingerRotation;
        /*float rotationValueY =
            angle(normalizeX(xInput[index]), 0, normalizeZ(zInput[index]), 2);
        transform.Rotate(new Vector3(0f, rotationValueY - yAngle, 0f));
        yAngle = rotationValueY;

        // Rotation on X axis
        float rotationValueX =
            angle(0, normalizeY(yInput[index]), normalizeZ(zInput[index]), 2);
        transform.Rotate(new Vector3(rotationValueX - xAngle, 0f, 0f));
        xAngle = rotationValueX;

        // Rotation on Z axis
        float rotationValueZ =
            angle(normalizeX(xInput[index]), normalizeY(yInput[index]), 0, 2);
        transform.Rotate(new Vector3(0f, 0f, rotationValueZ - zAngle));
        zAngle = rotationValueZ;*/


        timePassed += Time.deltaTime;

        if (timePassed >= TIME_INTERVAL)
        {
            timePassed = 0.0f;
            index++;
            prevRightHand = wanted_elbow_pos + d_hand_U;
            prevRightElbow = wanted_elbow_pos;
        }

    }
}
