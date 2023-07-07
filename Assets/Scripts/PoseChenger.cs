using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseChenger : MonoBehaviour ,IItemChenger
{
    [SerializeField] private List<GameObject> Items;
    [SerializeField] private Transform head;
    public void SetItem(int poseIndex)
    {
        foreach (var item in Items)
        {
            item.SetActive(false);
        }
        Items[poseIndex].SetActive(true);
        Transform heapPoint = Items[poseIndex].transform.GetChild(0);
        head.position = heapPoint.position;
        head.rotation = heapPoint.rotation;
        head.localScale = heapPoint.lossyScale;
        PlayerMovement.instance.DenBodySprite = Items[poseIndex].GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        SetItem(PlayerPrefs.GetInt("CurrentPose", 0));
    }
}

public interface IItemChenger
{
    public void SetItem(int itemIndex);
}
