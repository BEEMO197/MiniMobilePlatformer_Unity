using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform transformToMoveTo;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(transformToMoveTo.position.x, transformToMoveTo.position.y, -10.0f), 0.05f);
    }
}
