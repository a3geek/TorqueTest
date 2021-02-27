using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAirplane : MonoBehaviour
{
    [SerializeField]
    private float kmph = 100f;


    private void FixedUpdate()
    {
        var rb = this.GetComponent<Rigidbody>();

        var horizon = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        rb.AddRelativeTorque(new Vector3(0f, horizon, -horizon));
        rb.AddRelativeTorque(new Vector3(vertical, 0f, 0f));

        var left = this.transform.TransformVector(Vector3.left);
        var horizonLeft = new Vector3(left.x, 0f, left.z).normalized;
        rb.AddTorque(Vector3.Cross(left, horizonLeft) * 4f);

        var forward = this.transform.TransformVector(Vector3.forward);
        var horizonForward = new Vector3(forward.x, 0f, forward.z).normalized;
        rb.AddTorque(Vector3.Cross(forward, horizonForward) * 4f);

        var force = (rb.mass * rb.drag * this.kmph / 3.6f) / (1f - rb.drag * Time.fixedDeltaTime);
        rb.AddRelativeForce(new Vector3(0f, 0f, force));
    }
}
