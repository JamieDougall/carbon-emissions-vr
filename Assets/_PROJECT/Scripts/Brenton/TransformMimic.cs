using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TransformMimic : MonoBehaviour {

    #region Members
    public Transform target, mimic;
    [Range(0.0f, 10.0f)]
    public float delay = 0.0f;
    public bool copyPosition = true, copyRotation = true;
    public bool late = false;
    public bool reverse = false;
    #endregion

    
    void Start () {
		if (mimic == null)
        {
            mimic = this.transform;
        }
	}
	
	private void Mimic()
    {
        bool delayed = (delay > 0.0f);
        float speed = 10.0f / delay;
        if (copyPosition)
        {
            if (reverse)
            {
                target.position = delayed ? Vector3.Lerp(target.position, mimic.position, Time.deltaTime * speed) : mimic.position;
            }
            else
            {
                mimic.position = delayed ? Vector3.Lerp(mimic.position, target.position, Time.deltaTime * speed) : target.position;
            }
        }
        if (copyRotation)
        {
            if (reverse)
            {
                target.rotation = delayed ? Quaternion.Lerp(target.rotation, mimic.rotation, Time.deltaTime * speed) : mimic.rotation;
            }
            else
            {
                mimic.rotation = delayed ? Quaternion.Lerp(mimic.rotation, target.rotation, Time.deltaTime * speed) : target.rotation;
            }
        }
    }

	void Update () {
        if (!late)
        {
            Mimic();
        }
    }

    private void LateUpdate()
    {
        if (late)
        {
            Mimic();
        }
    }
}
