using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    private SpriteRenderer m_spriteRenderer;
    private Rigidbody2D m_rigidbody2D;
    private BoxCollider2D m_boxCollider2D;
    private Animator m_animator;

    public LayerMask groundedLayerMask;
    public LayerMask wallLayerMask;
    private float m_checkPauseMaxTime = 0.2f;
    private float m_checkPauseTimer;
    [SerializeField]
    private float m_raycastDistance = 0.05f;
    private Vector2 m_groundedRaycastStartOffset;
    private Vector2 m_wallRaycastStartOffsetL1;
    private Vector2 m_wallRaycastStartOffsetL2;
    private Vector2 m_wallRaycastStartOffsetR1;
    private Vector2 m_wallRaycastStartOffsetR2;

    private bool m_isRolling = false;
    private float m_rollMaxTime = 8.0f / 14.0f;
    private float m_rollHalfTime = 4.0f / 14.0f;
    private float m_rollTimer;
    [SerializeField]
    private float m_rollStartSpeed = 10f;
    private float m_rollMinSpeed;
    private float m_rollDeceleration;

    [SerializeField]
    private float m_moveMaxSpeed = 6f;
    [SerializeField]
    private float m_acceleration = 100f;
    [SerializeField]
    private int m_towardsRight = 1;

    [SerializeField]
    private float m_slideMaxSpeed = 4f;

    [SerializeField]
    private float m_jumpForce = 6f;
    [SerializeField]
    private float m_jumpMaxTime = 0.3f;
    private float m_jumpTimer;
    [SerializeField]
    private float m_riseMulitiplier = 2f;
    [SerializeField]
    private float m_fallMulitiplier = 5f;

    //private bool m_isBlocking = false;
    //private bool m_blockWithEffect = false;
    //private bool m_deathWithBlood = false;

    public enum PlayerState
    {
        OnGround,
        InAir,
        OnWall
    };
    public PlayerState playerState = PlayerState.OnGround;

    void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_boxCollider2D = GetComponent<BoxCollider2D>();
        m_animator = GetComponent<Animator>();
        m_groundedRaycastStartOffset = m_boxCollider2D.offset + Vector2.down * 
            (m_boxCollider2D.size.y * 0.5f - m_raycastDistance);
        m_wallRaycastStartOffsetL1 = m_boxCollider2D.offset + 
            Vector2.up * m_boxCollider2D.size.y * 0.4f + 
            Vector2.left * (m_boxCollider2D.size.x * 0.5f - m_raycastDistance);
        m_wallRaycastStartOffsetL2 = m_boxCollider2D.offset + 
            Vector2.down * m_boxCollider2D.size.y * 0.4f + 
            Vector2.left * (m_boxCollider2D.size.x * 0.5f - m_raycastDistance);
        m_wallRaycastStartOffsetR1 = m_boxCollider2D.offset + 
            Vector2.up * m_boxCollider2D.size.y * 0.4f + 
            Vector2.right * (m_boxCollider2D.size.x * 0.5f - m_raycastDistance);
        m_wallRaycastStartOffsetR2 = m_boxCollider2D.offset + 
            Vector2.down * m_boxCollider2D.size.y * 0.4f + 
            Vector2.right * (m_boxCollider2D.size.x * 0.5f - m_raycastDistance);
    }

    private string lastName = string.Empty;
    void FixedUpdate()
    {
        Move();
        Jump();
        CheckGroundedAndOnWall();
        SetAnimPara();
        //if (m_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != lastName)
        //{
        //    Debug.Log(m_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        //    lastName = m_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        //}
    }

    /// <summary>
    /// 移动刚体
    /// </summary>
    public void Move()
    {
        if (m_isRolling)
        {
            if (m_rollTimer > m_rollHalfTime)
            {
                m_rigidbody2D.velocity = new Vector2(m_rollStartSpeed * m_towardsRight + m_rollDeceleration * (m_rollTimer - m_rollHalfTime), 0f);
            }
            else
            {
                m_rigidbody2D.velocity = new Vector2(m_rollStartSpeed * m_towardsRight, 0f);
            }
            m_rollTimer += Time.deltaTime;
            if (m_rollTimer > m_rollMaxTime) m_isRolling = false;
        }
        else if (PlayerInput.Instance.Roll.Down)
        {
            if ((m_towardsRight == 1 && m_rigidbody2D.velocity.x > float.Epsilon) ||
                (m_towardsRight == -1 && m_rigidbody2D.velocity.x < -float.Epsilon))
            {
                m_rollMinSpeed = m_rigidbody2D.velocity.x;
            }
            else
            {
                m_rollMinSpeed = 0f;
                m_spriteRenderer.flipX = m_towardsRight == -1;
            }
            m_rollDeceleration = (m_rollMinSpeed - m_rollStartSpeed * m_towardsRight) / m_rollHalfTime;
            m_rigidbody2D.velocity = new Vector2(m_rollStartSpeed * m_towardsRight, 
                m_rigidbody2D.velocity.y);
            m_isRolling = true;
            m_rollTimer = 0f;
            m_animator.SetTrigger("Roll");
            if (playerState == PlayerState.OnWall) playerState = PlayerState.InAir;
            m_checkPauseTimer = m_rollMaxTime;
        }
        else
        {
            if (PlayerInput.Instance.Horizontal.Value > float.Epsilon) m_towardsRight = 1;
            else if (PlayerInput.Instance.Horizontal.Value < -float.Epsilon) m_towardsRight = -1;
            float currentSpeedX = Mathf.MoveTowards(m_rigidbody2D.velocity.x, 
                m_moveMaxSpeed * PlayerInput.Instance.Horizontal.Value, m_acceleration * Time.deltaTime);
            m_rigidbody2D.velocity = new Vector2(currentSpeedX, m_rigidbody2D.velocity.y);
            if (playerState == PlayerState.OnWall)
            {
                if (Mathf.Abs(currentSpeedX) > float.Epsilon)
                {
                    if (m_spriteRenderer.flipX != currentSpeedX < -float.Epsilon)
                    {
                        playerState = PlayerState.InAir;
                        m_checkPauseTimer = m_checkPauseMaxTime;
                    }
                }
                else
                {
                    float currentSpeedY = Mathf.MoveTowards(m_rigidbody2D.velocity.y,
                            m_slideMaxSpeed * PlayerInput.Instance.Vertical.Value, 
                            m_acceleration * Time.deltaTime);
                    m_rigidbody2D.velocity = new Vector2(0f, currentSpeedY);
                }
            }
            if (currentSpeedX > float.Epsilon) m_spriteRenderer.flipX = false;
            else if (currentSpeedX < -float.Epsilon) m_spriteRenderer.flipX = true;
        }
    }

    public void Jump()
    {
        if (m_isRolling)
        {
            m_jumpTimer = 0f;
            return;
        }
        if (playerState == PlayerState.InAir)
        {
            if (PlayerInput.Instance.Jump.Up)
            {
                m_jumpTimer = 0f;
            }
            if (m_jumpTimer > 0 && PlayerInput.Instance.Jump.Held)
            {
                m_rigidbody2D.velocity = new Vector2(m_rigidbody2D.velocity.x, m_jumpForce);
                m_jumpTimer -= Time.deltaTime;
            }
            else if (m_rigidbody2D.velocity.y > 0)
            {
                m_rigidbody2D.velocity += Vector2.up * Physics2D.gravity *
                    m_riseMulitiplier * Time.deltaTime;
            }
            else
            {
                m_rigidbody2D.velocity += Vector2.up * Physics2D.gravity *
                    m_fallMulitiplier * Time.deltaTime;
            }
        }
        else
        {
            if (PlayerInput.Instance.Jump.Down)
            {
                playerState = PlayerState.InAir;
                m_rigidbody2D.velocity = new Vector2(m_rigidbody2D.velocity.x, m_jumpForce);
                m_jumpTimer = m_jumpMaxTime;
                m_checkPauseTimer = m_checkPauseMaxTime;
                m_animator.SetTrigger("Jump");
                Debug.Log("Jump!!!");
            }
        }
    }

    /// <summary>
    /// 直接传送
    /// </summary>
    /// <param name="position">全局空间中的位置</param>
    public void Teleport(Vector2 position)
    {
        m_rigidbody2D.MovePosition(position);
    }

    public void CheckGroundedAndOnWall()
    {
        if (m_checkPauseTimer > 0)
        {
            m_checkPauseTimer -= Time.deltaTime;
            return;
        }
        RaycastHit2D hit = Physics2D.Raycast(m_rigidbody2D.position + m_groundedRaycastStartOffset,
            Vector2.down, m_raycastDistance * 2, groundedLayerMask);
        if (hit.collider != null)
        {
            playerState = PlayerState.OnGround;
        }
        else
        {
            RaycastHit2D hitL1 = Physics2D.Raycast(m_rigidbody2D.position + m_wallRaycastStartOffsetL1,
            Vector2.left, m_raycastDistance * 2, wallLayerMask);
            RaycastHit2D hitL2 = Physics2D.Raycast(m_rigidbody2D.position + m_wallRaycastStartOffsetL2,
                Vector2.left, m_raycastDistance * 2, wallLayerMask);
            RaycastHit2D hitR1 = Physics2D.Raycast(m_rigidbody2D.position + m_wallRaycastStartOffsetR1,
                Vector2.right, m_raycastDistance * 2, wallLayerMask);
            RaycastHit2D hitR2 = Physics2D.Raycast(m_rigidbody2D.position + m_wallRaycastStartOffsetR2,
                Vector2.right, m_raycastDistance * 2, wallLayerMask);
            //Debug.DrawLine(m_rigidbody2D.position + m_wallRaycastStartOffsetL1, m_rigidbody2D.position + m_wallRaycastStartOffsetL1 + Vector2.left * m_raycastDistance * 2, Color.red);
            //Debug.DrawLine(m_rigidbody2D.position + m_wallRaycastStartOffsetR1, m_rigidbody2D.position + m_wallRaycastStartOffsetR1 + Vector2.right * m_raycastDistance * 2, Color.blue);
            //Debug.Log("L1: " + (hitL1.collider != null) + "R1: " + (hitR1.collider != null));
            if ((hitL1.collider != null && hitL2.collider != null) || (hitR1.collider != null && hitR2.collider != null))
            {
                playerState = PlayerState.OnWall;
            }
            else
            {
                playerState = PlayerState.InAir;
            }
            //m_rigidbody2D.gravityScale = playerState == PlayerState.OnWall ? 0 : 1;
        }
    }

    public void SetAnimPara()
    {
        m_animator.SetInteger("PlayerState", (int)playerState);
        m_animator.SetBool("IsFalling", m_rigidbody2D.velocity.y < -float.Epsilon);
        m_animator.SetBool("IsRunning", !Mathf.Approximately(m_rigidbody2D.velocity.x, 0));
        //m_animator.SetBool("IsBlocking", m_isBlocking);
        //m_animator.SetBool("BlockWithEffect", m_blockWithEffect);
        //m_animator.SetBool("DeathWithBlood", m_deathWithBlood);
    }
}