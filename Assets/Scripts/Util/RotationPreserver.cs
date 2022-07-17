using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPreserver : MonoBehaviour
{
    [SerializeField] private Transform root;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        root.transform.rotation = Quaternion.identity;
    }
}
