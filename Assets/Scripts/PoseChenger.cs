using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseChenger : MonoBehaviour
{
    [SerializeField] private List<GameObject> poseSprites;
    [SerializeField] private Transform head;
    [SerializeField] private int startPose;
    public static PoseChenger instance;
    public void SetPose(int poseIndex)
    {
        foreach (var item in poseSprites)
        {
            item.SetActive(false);
        }
        poseSprites[poseIndex].SetActive(true);
        Transform heapPoint = poseSprites[poseIndex].transform.GetChild(0);
        head.position = heapPoint.position;
        head.rotation = heapPoint.rotation;
        head.localScale = heapPoint.lossyScale;
        PlayerMovement.instance.DenBodySprite = poseSprites[poseIndex].GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        instance = this;
        SetPose(PlayerPrefs.GetInt("CurrentPose", 0));
    }
}
