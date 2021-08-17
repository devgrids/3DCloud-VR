using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairPosition : MonoBehaviour
{
    public static ChairPosition sharedInstance;
    [SerializeField] List<Transform> positionList;

    private void Awake()
    {
        sharedInstance = this;
    }

    public Vector3 GetPositionPlayer()
    {
        return positionList[0].position;
    }
    
}
