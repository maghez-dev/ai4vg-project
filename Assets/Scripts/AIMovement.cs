using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AIMovement : MonoBehaviour {

    private float circleRadius = 0;
    private Vector3 circlePivot = new Vector3(0f, 0f, 0f);

    private int currentDirection = 1;

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

        if (Input.GetKeyUp("up"))
            GetNextPivot();

    }


    private void FixedUpdate() {
        Quaternion q = Quaternion.AngleAxis(speed * currentDirection, Vector3.up);
        Vector3 destination = q * (rb.transform.position - circlePivot) + circlePivot;
        rb.MovePosition(destination);
        rb.MoveRotation(rb.transform.rotation * q);
        transform.LookAt(destination);
    }


    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(circlePivot, Vector3.up, circleRadius);
        UnityEditor.Handles.DrawWireDisc(circlePivot, Vector3.up, 0.1f);
    }


    private void GetNextPivot() {
        circleRadius = Random.Range(minRadius, maxRadius);

        int dir = Random.Range(1, 3);

        float zPos = 0f;
        if (dir == 1) {
            zPos = circleRadius;
            currentDirection = 1;
        }
        else {
            zPos = -circleRadius;
            currentDirection = -1;
        }

        float x = transform.position.x + ((transform.right.x * circleRadius) * currentDirection);
        float y = transform.position.y;
        float z = transform.position.z + ((transform.right.z * circleRadius) * currentDirection);
        circlePivot = new Vector3(x, y, z);
    }
}
