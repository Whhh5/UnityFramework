using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class UISkeletonCanvasLayerSync : MonoBehaviour
{
    [SerializeField]
    private int _OrderLayer = 0;

    private void OnEnable()
    {
        var parent = transform.parent;
        while (parent != null)
        {
            if (parent.TryGetComponent<Canvas>(out var canvas))
            {
                if (canvas.overrideSorting)
                {
                    var skeleCom = gameObject.GetComponent<MeshRenderer>();
                    skeleCom.sortingOrder = canvas.sortingOrder + _OrderLayer;
                    break;
                }
            }
            parent = parent.parent;
        }
    }
}
