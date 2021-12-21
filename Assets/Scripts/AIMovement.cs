using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIMovement : MonoBehaviour {

    private float circleRadius = 0;
    private Vector3 circlePivot;

    private int currentDirection = 1;
    private Vector3 targetPosition;

    [SerializeField] private float speed = 2f;
    [SerializeField] [Range(1f, 100f)] private float minRadius = 1f;
    [SerializeField] [Range(1f, 100f)] private float maxRadius = 1f;
    [SerializeField] [Range(0, 359)] private int minAngle = 90;
    [SerializeField] [Range(0, 359)] private int maxAngle = 180;

    private List<GameObject> cache = new List<GameObject>();

    private void Start() {
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
        Quaternion rotation = Quaternion.AngleAxis((speed / circleRadius) * currentDirection, Vector3.up);
        Vector3 destination = rotation * (transform.position - circlePivot) + circlePivot;
        if ((transform.position - circlePivot).magnitude > circleRadius + 0.1f)
        {
            Debug.Log("hi");
        } else if ((transform.position - circlePivot).magnitude < circleRadius - 0.1f)
        {
            Debug.Log("hi");
        }

        //MovePosition(destination);
        //MoveRotation(transform.rotation * rotation);
        transform.LookAt(destination);
        transform.position += transform.forward * speed * Time.deltaTime;
    }


    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(circlePivot, Vector3.up, circleRadius);
        UnityEditor.Handles.DrawWireDisc(circlePivot, Vector3.up, 0.2f);
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
        circlePivot = new Vector3(x, y, z);

        int targetAngle = Random.Range(minAngle, maxAngle);

        targetPosition = RotateOnPivot(targetAngle * currentDirection, circleRadius);

        if (!CheckPathValidity(targetAngle))
            GenerateNextPivot(radius / 2);
    }


    private bool CheckPathValidity(float targetAngle) {
        for (int i = 0; i < targetAngle; i++) {
            Vector3 checkPosition = RotateOnPivot(i * currentDirection, circleRadius + 1f);

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
