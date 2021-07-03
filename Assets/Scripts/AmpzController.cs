using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmpzController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(3 * Time.deltaTime, 7 * Time.deltaTime, 50 * Time.deltaTime);
    }
}
