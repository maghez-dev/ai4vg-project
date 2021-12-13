using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AIMovement : MonoBehaviour {

    private float circleRadius = 0;

    private int currentDirection = 1;
    private Vector3 targetPosition;
    //private float targetAngle = 0f;

    private Rigidbody rb;

    [SerializeField] private Transform circlePivot;
    [SerializeField] private float angularSpeed = 2f;
    [SerializeField] [Range(1f, 15f)] private float minRadius = 1f;
    [SerializeField] [Range(1f, 15f)] private float maxRadius = 1f;
    [SerializeField] [Range(0, 360)] private int minAngle = 90;
    [SerializeField] [Range(0, 360)] private int maxAngle = 180;


    private void Start() {
        rb = GetComponent<Rigidbody>();

        GenerateNextPivot(Random.Range(minRadius, maxRadius));
    }


    private void Update() {
        if (minRadius > maxRadius)
            minRadius = maxRadius;
        if (minAngle > maxAngle)
            minAngle = maxAngle;

        if (Input.GetKeyUp("up"))
            GenerateNextPivot(Random.Range(minRadius, maxRadius));

        Vector3 toDestination = transform.position - targetPosition;
        if (toDestination.magnitude < 0.5f)
            GenerateNextPivot(Random.Range(minRadius, maxRadius));
    }


    private void FixedUpdate() {
        Quaternion q = Quaternion.AngleAxis((angularSpeed/10) * currentDirection, Vector3.up);
        Vector3 destination = q * (rb.transform.position - circlePivot.position) + circlePivot.position;
        rb.MovePosition(destination);
        rb.MoveRotation(rb.transform.rotation * q);
        transform.LookAt(destination);
    }


    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(circlePivot.position, Vector3.up, circleRadius);
        UnityEditor.Handles.DrawWireDisc(circlePivot.position, Vector3.up, 0.2f);
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(targetPosition, Vector3.up, 0.3f);
    }


    private void GenerateNextPivot(float radius) {
        circleRadius = radius;

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
        circlePivot.position = new Vector3(x, y, z);

        int targetAngle = Random.Range(minAngle, maxAngle);

        circlePivot.LookAt(transform);
        circlePivot.eulerAngles = new Vector3(0, targetAngle, 0);

        x = circlePivot.position.x + ((transform.forward.x * circleRadius));
        y = circlePivot.position.y;
        z = circlePivot.position.z + ((transform.forward.z * circleRadius));
        targetPosition = new Vector3(x, y, z);

        if (!CheckPathValidity())
            GenerateNextPivot(radius / 2);
    }


    private bool CheckPathValidity() {

        return true;
    }
}
