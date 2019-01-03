using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonCamera : MonoBehaviour
{
    public float m_DampTime = 0.2f;
    public float m_ScreenEdgeBuffer = 4f;
    private float m_MinSize = 11f;
    private float m_MaxSize = 16f;

    private float m_xPosRange = 7f;
    private float m_xPosMin = 0f;
    private float m_xPosMax = 0f;

    public Transform[] m_Targets;

    private Camera m_Camera;
    private float m_ZoomSpeed;
    private Vector3 m_DesiredPosition;
    private Vector3 m_MoveVelocity;

    private void Awake()
    {
        m_Camera = GetComponentInChildren<Camera>();
        m_xPosMin = transform.position.x - m_xPosRange;
        m_xPosMax = transform.position.x + m_xPosRange;
    }
	
	void FixedUpdate ()
    {
        Move();
        Zoom();
	}

    void Move()
    {
        FindAveragePosition();
        transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
    }

    void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();
        int numTargets = 0;

        for (int i = 0; i < m_Targets.Length; i++)
        {
            if (!m_Targets[i].gameObject.activeSelf)
                continue;
            averagePos += m_Targets[i].position;
            numTargets++;
        }

        if (numTargets > 0)
            averagePos /= numTargets;

        averagePos.y = transform.position.y;
        // averagePos.y += 5.0f;
        averagePos = new Vector3(Mathf.Clamp(averagePos.x, m_xPosMin, m_xPosMax), averagePos.y, averagePos.z);

        m_DesiredPosition = averagePos;
    }

    void Zoom()
    {
        float requiredSize = FindRequiredSize();
        m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);
    }

    private float FindRequiredSize()
    {
        Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);

        float size = 0f;

        for (int i = 0; i < m_Targets.Length; i++)
        {
            if (!m_Targets[i].gameObject.activeSelf)
                continue;

            Vector3 targetLocalPos = transform.InverseTransformPoint(m_Targets[i].position);

            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / m_Camera.aspect);
        }

        size += m_ScreenEdgeBuffer;

        size = Mathf.Clamp(size, m_MinSize, m_MaxSize);

        return size;
    }

    public void SetStartPositionAndSize()
    {
        FindAveragePosition();

        transform.position = m_DesiredPosition;

        m_Camera.orthographicSize = FindRequiredSize();
    }

}
