using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class CharacterController2D : MonoBehaviour
{
    public LayerMask groundedLayerMask;
    public float groundedRaycastDistance = 0.1f;

    Rigidbody2D m_Rigidbody2D;
    CapsuleCollider2D m_Capsule;
    Vector2 m_PreviousPosition;
    Vector2 m_CurrentPosition;
    Vector2 m_NextMovement;
    Vector2[] m_RaycastPositions = new Vector2[3];

    public bool IsGrounded { get; protected set; }
    public bool IsCeilinged { get; protected set; }
    public Vector2 Velocity { get; protected set; }

    void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Capsule = GetComponent<CapsuleCollider2D>();

        m_CurrentPosition = m_Rigidbody2D.position;
        m_PreviousPosition = m_Rigidbody2D.position;
    }

    void FixedUpdate()
    {
        m_PreviousPosition = m_Rigidbody2D.position;
        m_CurrentPosition = m_PreviousPosition + m_NextMovement;
        Velocity = (m_CurrentPosition - m_PreviousPosition) / Time.deltaTime;
        m_Rigidbody2D.MovePosition(m_CurrentPosition);
        m_NextMovement = Vector2.zero;

        CheckCapsuleEndCollisions(true);
        CheckCapsuleEndCollisions(false);
    }

    /// <summary>
    /// 移动刚体，需从FixeUpdate调用
    /// </summary>
    /// <param name="movement">移动的量</param>
    public void Move(Vector2 movement)
    {
        m_NextMovement += movement;
    }

    /// <summary>
    /// 直接传送
    /// </summary>
    /// <param name="position">全局空间中的位置</param>
    public void Teleport(Vector2 position)
    {
        Vector2 delta = position - m_CurrentPosition;
        m_PreviousPosition += delta;
        m_CurrentPosition = position;
        m_Rigidbody2D.MovePosition(position);
    }

    /// <summary>
    /// 更新IsGrounded/IsCeilinged的状态
    /// </summary>
    public void CheckCapsuleEndCollisions(bool bottom)
    {
        float raycastDistance;
        Vector2 raycastStartCentre;
        Vector2 raycastDirection = bottom ? Vector2.down : Vector2.up;

        // 这里需要调整参数
        if (m_Capsule == null)
        {
            raycastDistance = 1f + groundedRaycastDistance;
            raycastStartCentre = m_Rigidbody2D.position + Vector2.up;

            m_RaycastPositions[0] = raycastStartCentre + Vector2.left * 0.4f;
            m_RaycastPositions[1] = raycastStartCentre;
            m_RaycastPositions[2] = raycastStartCentre + Vector2.right * 0.4f;
        }
        else
        {
            raycastDistance = m_Capsule.size.x * 0.5f + groundedRaycastDistance;
            raycastStartCentre = m_Rigidbody2D.position + m_Capsule.offset + 
                raycastDirection * (m_Capsule.size.y * 0.5f - m_Capsule.size.x * 0.5f);
            m_RaycastPositions[0] = raycastStartCentre + Vector2.left * m_Capsule.size.x * 0.5f;
            m_RaycastPositions[1] = raycastStartCentre;
            m_RaycastPositions[2] = raycastStartCentre + Vector2.right * m_Capsule.size.x * 0.5f;
        }

        if (bottom) IsGrounded = false;
        else IsCeilinged = false;

        for (int i = 0; i < m_RaycastPositions.Length; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(m_RaycastPositions[i], raycastDirection, raycastDistance, groundedLayerMask);

            if (hit.collider != null)
            {
                if (bottom) IsGrounded = true;
                else IsCeilinged = true;
            }
        }
    }
}