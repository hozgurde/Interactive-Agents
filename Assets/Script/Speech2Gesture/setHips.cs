using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setHips : MonoBehaviour
{

    [SerializeField] Transform referenceHips;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = referenceHips.position;
    }
}
