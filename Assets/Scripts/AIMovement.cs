using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIMovement : MonoBehaviour {

    private float destinationPoint = 0.4f;

    private float circleRadius = 0;
    private Vector3 circlePivot;

    private int currentDirection = 1;
    private Vector3 targetPosition;

    [SerializeField] [Range(0f, 15f)] private float speed = 5f;
    [SerializeField] [Range(5f, 50f)] private float minRadius = 5f;
    [SerializeField] [Range(5f, 50f)] private float maxRadius = 50f;
    [SerializeField] [Range(1, 359)] private int minAngle = 1;
    private int maxAngle = 359;


    private void Start() {
        GenerateNextPivot(Random.Range(minRadius, maxRadius));
    }

    private void Update() {
        if (minRadius > maxRadius)
            minRadius = maxRadius;

        if (Input.GetKeyUp("up"))
            GenerateNextPivot(Random.Range(minRadius, maxRadius));

        Vector3 toDestination = transform.position - targetPosition;
        if (toDestination.magnitude < destinationPoint)
            GenerateNextPivot(Random.Range(minRadius, maxRadius));
    }


    private void FixedUpdate() {
        Quaternion rotation = Quaternion.AngleAxis((speed / circleRadius) * currentDirection, Vector3.up);
        Vector3 destination = rotation * (transform.position - circlePivot) + circlePivot;
        transform.LookAt(destination);
        transform.position += transform.forward * speed * Time.deltaTime;
    }


    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(circlePivot, Vector3.up, circleRadius);
        UnityEditor.Handles.DrawWireDisc(circlePivot, Vector3.up, 0.2f);
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(targetPosition, Vector3.up, destinationPoint);
    }


    private void GenerateNextPivot(float radius) {
        circleRadius = radius;

        int dir = Random.Range(1, 3);

        float zPos;
        if (dir == 1) {
            zPos = circleRadius;
            currentDirection = 1;
        }
        else {
            zPos = -circleRadius;
            currentDirection = -1;
        }

        int targetAngle = Random.Range(minAngle, maxAngle);

        if (circleRadius < 5f) {
            currentDirection = 1;
            targetAngle = 135;
        }
        if (circleRadius < destinationPoint) {
            circleRadius = destinationPoint + 1f;
        }

        float x = transform.position.x + ((transform.right.x * circleRadius) * currentDirection);
        float y = transform.position.y;
        float z = transform.position.z + ((transform.right.z * circleRadius) * currentDirection);
        circlePivot = new Vector3(x, y, z);

        targetPosition = RotateOnPivot(targetAngle * currentDirection, circleRadius);

        if (!CheckPathValidity(targetAngle))
            GenerateNextPivot(radius / 2);
    }


    private bool CheckPathValidity(float targetAngle) {
        for (int i = 0; i < targetAngle + 5; i++) {
            Vector3 checkPosition = RotateOnPivot(i * currentDirection, circleRadius + 2f);

            if (!Physics.Raycast(checkPosition, Vector3.down, 10))
                return false;

            checkPosition = RotateOnPivot(i * currentDirection, circleRadius - 2f);

            if (!Physics.Raycast(checkPosition, Vector3.down, 10))
                return false;
        }
        
        return true;
    }

    private Vector3 RotateOnPivot(float angle, float offset) {
        Vector3 vector = transform.position - circlePivot;
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        vector = (rotation * vector).normalized;

        float x = circlePivot.x + (vector.x * offset);
        float y = circlePivot.y;
        float z = circlePivot.z + (vector.z * offset);
        Vector3 checkPosition = new Vector3(x, y, z);

        return checkPosition;
    }
}
