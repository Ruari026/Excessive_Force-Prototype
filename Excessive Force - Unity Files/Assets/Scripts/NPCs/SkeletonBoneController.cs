using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBoneController : MonoBehaviour
{
    [SerializeField] private List<SkeletonBoneDetails> skeletonBones;

    public GameObject GetBone(BoneType boneToGet)
    {
        foreach (SkeletonBoneDetails sbd in skeletonBones)
        {
            if (sbd.boneType == boneToGet)
            {
                return sbd.gameObject;
            }
        }

        Debug.LogError("ERROR: Could Not Find Bone (" + boneToGet + ") In Skeleton");
        return null;
    }
}

public enum BoneType
{ 
    HIPS,
    SPINE,
    HEAD,

    RIGHT_SHOULDER,
    RIGHT_ARM,
    RIGHT_FOREARM,
    RIGHT_HAND,

    LEFT_SHOULDER,
    LEFT_ARM,
    LEFT_FOREARM,
    LEFT_HAND,

    RIGHT_UPLEG,
    RIGHT_LEG,
    RIGHT_FOOT,

    LEFT_UPLEG,
    LEFT_LEG,
    LEFT_FOOT,
}