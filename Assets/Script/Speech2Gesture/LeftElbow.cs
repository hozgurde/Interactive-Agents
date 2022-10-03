using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LeftElbow : MonoBehaviour
{



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
        e1_c0_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e1_c1_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e1_c2_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e1_c3_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e1_c4_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e1_c5_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e1_c6_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e1_c7_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e1_c8_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e1_c9_x = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e1_c0_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e1_c1_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e1_c2_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e1_c3_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e1_c4_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e1_c5_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e1_c6_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e1_c7_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e1_c8_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e1_c9_y = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e1_c0_z = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e1_c1_z = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e1_c2_z = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e1_c3_z = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e1_c4_z = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e1_c5_z = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e1_c6_z = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e1_c7_z = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e1_c8_z = new float[][] { new float[0], new float[0], new float[0] };

    float[][]
        e1_c9_z = new float[][] { new float[0], new float[0], new float[0] };

    float[] xInput = new float[0];

    float[] yInput = new float[0];

    float[] zInput = new float[0];

    int currentSize = 0;

    public int index = 0;

    public int c;

    public int l;

    float xAngle = 0;

    float yAngle = 0;

    float zAngle = 0;

    float timePassed = 0.0f;

    const float TIME_INTERVAL = 0.05f;
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

        string path = "./Assets/Script/Speech2Gesture/s2gData/leftElbowX.txt";

        StreamReader reader = new StreamReader(path);

        while ((line = reader.ReadLine()) != null)
        {
            //line = line.Replace('.', ',');
            print(line);
            f = float.Parse(line);
            print(f);
            Array.Resize(ref xInput, currentSize + 1);
            xInput[currentSize] = f;
            print("in array "+xInput[currentSize]);
            currentSize++;
        }

        currentSize = 0;
        path = "./Assets/Script/Speech2Gesture/s2gData/leftElbowY.txt";
        reader = new StreamReader(path);

        while ((line = reader.ReadLine()) != null)
        {
            //line = line.Replace('.', ',');
            f = float.Parse(line);
            Array.Resize(ref yInput, currentSize + 1);
            yInput[currentSize] = f;
            currentSize++;
        }

        currentSize = 0;
        path = "./Assets/Script/Speech2Gesture/s2gData/leftElbowZ.txt";
        reader = new StreamReader(path);

        while ((line = reader.ReadLine()) != null)
        {
            //line = line.Replace('.', ',');
            f = float.Parse(line);
            Array.Resize(ref zInput, currentSize + 1);
            zInput[currentSize] = f;
            currentSize++;
        }

        float[][] arrX;
        float[][] arrY;
        float[][] arrZ;

        for (int c = 0; c < 10; c++)
        {
            if (c == 0)
            {
                arrX = e1_c0_x;
                arrY = e1_c0_y;
                arrZ = e1_c0_z;
            }
            else if (c == 1)
            {
                arrX = e1_c1_x;
                arrY = e1_c1_y;
                arrZ = e1_c1_z;
            }
            else if (c == 2)
            {
                arrX = e1_c2_x;
                arrY = e1_c2_y;
                arrZ = e1_c2_z;
            }
            else if (c == 3)
            {
                arrX = e1_c3_x;
                arrY = e1_c3_y;
                arrZ = e1_c3_z;
            }
            else if (c == 4)
            {
                arrX = e1_c4_x;
                arrY = e1_c4_y;
                arrZ = e1_c4_z;
            }
            else if (c == 5)
            {
                arrX = e1_c5_x;
                arrY = e1_c5_y;
                arrZ = e1_c5_z;
            }
            else if (c == 6)
            {
                arrX = e1_c6_x;
                arrY = e1_c6_y;
                arrZ = e1_c6_z;
            }
            else if (c == 7)
            {
                arrX = e1_c7_x;
                arrY = e1_c7_y;
                arrZ = e1_c7_z;
            }
            else if (c == 8)
            {
                arrX = e1_c8_x;
                arrY = e1_c8_y;
                arrZ = e1_c8_z;
            }
            else
            {
                arrX = e1_c9_x;
                arrY = e1_c9_y;
                arrZ = e1_c9_z;
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
                reader = new StreamReader(path + "/leftHandX.txt");

                while ((line = reader.ReadLine()) != null)
                {
                    //line = line.Replace('.', ',');
                    f = float.Parse(line);
                    Array.Resize(ref arrX[l], currentSize + 1);
                    arrX[l][currentSize] = f;
                    currentSize++;
                }

                currentSize = 0;
                reader = new StreamReader(path + "/leftHandY.txt");

                while ((line = reader.ReadLine()) != null)
                {
                    //line = line.Replace('.', ',');
                    f = float.Parse(line);
                    Array.Resize(ref arrY[l], currentSize + 1);
                    arrY[l][currentSize] = f;
                    currentSize++;
                }

                currentSize = 0;
                reader = new StreamReader(path + "/leftHandZ.txt");

                while ((line = reader.ReadLine()) != null)
                {
                    //line = line.Replace('.', ',');
                    f = float.Parse(line);
                    Array.Resize(ref arrZ[l], currentSize + 1);
                    arrZ[l][currentSize] = f;
                    currentSize++;
                }
            }
        }

        reader.Close();
    }

    // Update is called once per frame
    void Update()
    {
        
        print("LALALA: " + xInput[0] + "\n");

        print(c);

        if (c != -1 && l != -1)
        {
            print("in c");
            if (c == 0)
            {
                xInput = e1_c0_x[l];
                yInput = e1_c0_y[l];
                zInput = e1_c0_z[l];
            }
            else if (c == 1)
            {
                xInput = e1_c1_x[l];
                yInput = e1_c1_y[l];
                zInput = e1_c1_z[l];
            }
            else if (c == 2)
            {
                xInput = e1_c2_x[l];
                yInput = e1_c2_y[l];
                zInput = e1_c2_z[l];
            }
            else if (c == 3)
            {
                xInput = e1_c3_x[l];
                yInput = e1_c3_y[l];
                zInput = e1_c3_z[l];
            }
            else if (c == 4)
            {
                xInput = e1_c4_x[l];
                yInput = e1_c4_y[l];
                zInput = e1_c4_z[l];
            }
            else if (c == 5)
            {
                xInput = e1_c5_x[l];
                yInput = e1_c5_y[l];
                zInput = e1_c5_z[l];
            }
            else if (c == 6)
            {
                xInput = e1_c6_x[l];
                yInput = e1_c6_y[l];
                zInput = e1_c6_z[l];
            }
            else if (c == 7)
            {
                xInput = e1_c7_x[l];
                yInput = e1_c7_y[l];
                zInput = e1_c7_z[l];
            }
            else if (c == 8)
            {
                xInput = e1_c8_x[l];
                yInput = e1_c8_y[l];
                zInput = e1_c8_z[l];
            }
            else
            {
                xInput = e1_c9_x[l];
                yInput = e1_c9_y[l];
                zInput = e1_c9_z[l];
            }
        }

        if (index >= xInput.Length)
        {
            return;
        }
        else
        {
            print(Time.deltaTime);
        }
/*
        // Rotation on Y axis
        float rotationValueY =
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
        zAngle = rotationValueZ;
*/
        //Change the position
        Transform ThisTransform = GetComponent<Transform>();

        ThisTransform.position =
            new Vector3(normalizeX(xInput[index]),
                normalizeY(yInput[index]),
                normalizeZ(zInput[index]));

        timePassed += Time.deltaTime;

        if(timePassed >= TIME_INTERVAL)
        {
            timePassed = 0.0f;
            index++;
        }
        
    }
}
