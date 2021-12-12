using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AIMovement : MonoBehaviour
{
    private float timeCounter = 0;

    private float circleRadius = 0;
    private Vector3 circlePivot = new Vector3(0f, 0f, 0f);

    private Rigidbody rb;

    [SerializeField] [Range(1f, 20f)] private float minRadius = 1f;
    [SerializeField] [Range(1f, 20f)] private float maxRadius = 1f;

    [SerializeField] public float speed = 2f;
    public float stopAt = 0.01f;

    private void Start() {
        rb = GetComponent<Rigidbody>();

        GetNextPivot();
    }

    private void Update() {
        if (minRadius > maxRadius)
            minRadius = maxRadius;
    }

    private void FixedUpdate() {
        Quaternion q = Quaternion.AngleAxis(speed, Vector3.up);
        Vector3 destination = q * (rb.transform.position - circlePivot) + circlePivot;
        rb.MovePosition(destination);
        rb.MoveRotation(rb.transform.rotation * q);
        transform.LookAt(destination);
    }


    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(circlePivot, Vector3.up, circleRadius);
    }


    private void GetNextPivot() {
        circleRadius = Random.Range(minRadius, maxRadius);
        float zPos = 0f;
        int dir = Random.Range(1, 3);
        if (dir == 1)
            zPos = circleRadius;
        else
            zPos = -circleRadius;

        circlePivot = new Vector3(transform.position.x, transform.position.y, transform.position.z + zPos);
    }
}
