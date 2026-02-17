using UnityEngine;
using System.Collections;

public class Bandit : MonoBehaviour
{
    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_Bandit m_groundSensor;
    private bool m_grounded = false;
    private bool m_combatIdle = false;
    private bool m_isDead = false;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
    }

    void Update()
    {
        // Ground check
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", true);
        }

        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", false);
        }

        // WASD movement
        float inputX = 0f;

        if (Input.GetKey(KeyCode.D))
            inputX = 1f;
        else if (Input.GetKey(KeyCode.A))
            inputX = -1f;

        // Flip sprite
        if (inputX > 0)
            transform.localScale = new Vector3(-2, 2, 2);
        else if (inputX < 0)
            transform.localScale = new Vector3(2, 2, 2);

        // Move
        m_body2d.linearVelocity = new Vector2(inputX * m_speed, m_body2d.linearVelocity.y);

        m_animator.SetFloat("AirSpeed", m_body2d.linearVelocity.y);

        // Death
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!m_isDead)
                m_animator.SetTrigger("Death");
            else
                m_animator.SetTrigger("Recover");

            m_isDead = !m_isDead;
        }

        // Hurt
        else if (Input.GetKeyDown(KeyCode.Q))
            m_animator.SetTrigger("Hurt");

        // Attack
        else if (Input.GetMouseButtonDown(0))
            m_animator.SetTrigger("Attack");

        // Combat idle toggle
        else if (Input.GetKeyDown(KeyCode.F))
            m_combatIdle = !m_combatIdle;

        // Jump (W)
        else if (Input.GetKeyDown(KeyCode.W) && m_grounded)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", false);
            m_body2d.linearVelocity = new Vector2(m_body2d.linearVelocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }

        // Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
            m_animator.SetInteger("AnimState", 2);

        // Combat Idle
        else if (m_combatIdle)
            m_animator.SetInteger("AnimState", 1);

        // Idle
        else
            m_animator.SetInteger("AnimState", 0);
    }
}
