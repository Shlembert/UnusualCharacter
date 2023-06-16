using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class SrollShop : MonoBehaviour
{
    [SerializeField] private List<Transform> items;
    [SerializeField] private float duration;
    [SerializeField] private Transform papa;

    private bool check_1 = true;
    private bool check_2 = true;

    private float distance;

    public void UpButton()
    {
        if (!check_1) return;

        check_1 = false;
        
        distance = items[0].position.y - items[1].position.y;

        foreach (var item in items)
        {
            item.DOMoveY(item.position.y + distance, duration, false).OnComplete(() => { CangePositionUp(); });
        }
    }

    private void CangePositionUp()
    {
        if (check_1) return;

        var child = transform.GetChild(0);
        child.SetParent(papa);

        var temp = items[0];
        items.Remove(items[0]);

        temp.gameObject.SetActive(false);
        temp.gameObject.SetActive(true);
        temp.position = new Vector2(temp.position.x, items[items.Count - 1].position.y + distance);
        items.Add(temp);

        child.SetParent(transform);
        check_1 = true;
    }

    public void DownButton()
    {
        if (!check_2) return;

        check_2 = false;

        distance = items[0].position.y - items[1].position.y;

        foreach (var item in items)
        {
            item.DOMoveY(item.position.y - distance, duration, false).OnComplete(()=> { CangePositionDown(); });
        }
    }

    private void CangePositionDown()
    {
        if (check_2) return;

        var child = transform.GetChild(transform.childCount -1);
        Debug.Log("Item mame: " + child.name);
       // child.SetParent(papa);

        var temp = items[items.Count - 1];
        items.Remove(temp);

        //temp.gameObject.SetActive(false);
        //temp.gameObject.SetActive(true);
        temp.position = new Vector2(temp.position.x, items[0].position.y + distance);
        items.Insert(0,temp);

        foreach (var item in items)
        {
            item.SetParent(papa);
            item.SetParent(transform);
        }

        //foreach (var item in items)
        //{
        //    item.SetParent(transform);
        //}

        check_2 = true;
    }
}
