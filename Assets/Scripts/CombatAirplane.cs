using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAirplane : MonoBehaviour
{
    public string GroupName { get; set; }

    [SerializeField]
    private float kmph = 100f;

    private Rigidbody body = null;


    private void Awake()
    {
        this.body = this.GetComponent<Rigidbody>();

        var pos = this.transform.position;
        this.transform.LookAt(new Vector3(0f, pos.y, pos.z), Vector3.up);
    }

    private void FixedUpdate()
    {
        var rb = this.body;

        var forward = this.transform.TransformVector(Vector3.forward);

        var closest = CombatAirplanesManager.Instance.GetClosestCombatAirplane(this);
        var dir = closest.transform.position - this.transform.position;
        this.body.AddTorque(Vector3.Cross(forward, dir) * 50f);

        rb.AddTorque(Vector3.Cross(forward, -this.transform.position) * 5f);

        var left = this.transform.TransformVector(Vector3.left);
        var horizonForward = new Vector3(forward.x, 0f, forward.z).normalized;
        var horizonLeft = Vector3.Cross(Vector3.up, horizonForward);
        rb.AddTorque(Vector3.Cross(forward, horizonForward) * 2f);
        rb.AddTorque(Vector3.Cross(horizonLeft, left) * 2f);

        var force = (this.kmph / 3600f * 1000f) * (rb.mass * rb.drag / (1f - rb.drag * Time.fixedDeltaTime));
        this.body.AddForce(this.transform.TransformVector(new Vector3(0f, 0f, force)));
    }
}
