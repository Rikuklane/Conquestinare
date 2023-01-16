using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingAnimation : MonoBehaviour
{
    public AnimationCurve Curve;
    public float Duration;
    public Vector3 StartScale;
    public Vector3 EndScale;
    public bool CloseOnEnd;

    private float timeAggregate;

    private void OnEnable()
    {
        timeAggregate = 0;
        transform.localScale = StartScale;
    }

    // Update is called once per frame
    void Update()
    {
        timeAggregate += Time.deltaTime;
        float value = Curve.Evaluate(timeAggregate / Duration);
        transform.localScale = Vector3.LerpUnclamped(StartScale, EndScale, value);
        if(timeAggregate >= Duration)
        {
            if(CloseOnEnd)
            {
                enabled = false;
                gameObject.SetActive(false);
            }
        }
    }
}
