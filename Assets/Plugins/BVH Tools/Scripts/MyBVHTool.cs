using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MyBVHTool
{
    BVHParser bp;

    public MyBVHTool(BVHParser bp)
    {
        this.bp = bp;
    }

    public void PrintBVH(string space, BVHParser.BVHBone bone)
    {
        Debug.Log(space + bone.name);
        foreach(var child in bone.children)
        {
            PrintBVH(space + "-", child);
        }
    }

    public void InsertNames(string insert, BVHParser.BVHBone bone)
    {
        bone.name = insert + bone.name;
        foreach (var child in bone.children)
        {
            InsertNames(insert, child);
        }
    }

    public void ChangeName(string prevName, string newName, BVHParser.BVHBone bone)
    {
        if (bone.name.Equals(prevName))
        {
            bone.name = newName;
        }
        else
        {
            foreach(var child in bone.children)
            {
                ChangeName(prevName, newName, child);
            }
        }
    }

    public void DeleteBone(string name, BVHParser.BVHBone bone)
    {
        foreach(var child in bone.children)
        {
            if (child.name.Equals(name))
            {
                foreach(var secondChild in child.children)
                {
                    bone.children.Add(secondChild);
                }
                bone.children.Remove(child);
                return;
            }
            else
            {
                DeleteBone(name, child);
            }
        }
        
    }

    public void MergeBone(string name, BVHParser.BVHBone bone)
    {
        foreach (var child in bone.children)
        {
            if (child.name.Equals(name))
            {
                foreach (var secondChild in child.children)
                {
                    bone.children.Add(secondChild);
                }
                bone.offsetX = (bone.offsetX + child.offsetX) / 2;
                bone.offsetY = (bone.offsetY + child.offsetY) / 2;
                bone.offsetZ = (bone.offsetZ + child.offsetZ) / 2;


                for(int i = 0; i < bp.frames; i++)
                {
                    if (bone.channels[0].enabled)
                    {
                        bone.channels[0].values[i] = bone.channels[0].values[i] + child.channels[0].values[i];
                    }

                    if (bone.channels[1].enabled)
                    {
                        bone.channels[1].values[i] = bone.channels[1].values[i] + child.channels[1].values[i];
                    }

                    if (bone.channels[2].enabled)
                    {
                        bone.channels[2].values[i] = bone.channels[2].values[i] + child.channels[2].values[i];
                    }

                    if (bone.channels[3].enabled)
                    {
                        bone.channels[3].values[i] = bone.channels[3].values[i] + child.channels[3].values[i];
                    }

                    if (bone.channels[4].enabled)
                    {
                        bone.channels[4].values[i] = bone.channels[4].values[i] + child.channels[4].values[i];
                    }

                    if (bone.channels[5].enabled)
                    {
                        bone.channels[5].values[i] = bone.channels[5].values[i] + child.channels[5].values[i];
                    }

                }

                bone.children.Remove(child);
                return;
            }
            else
            {
                MergeBone(name, child);
            }
        }
    }
}
