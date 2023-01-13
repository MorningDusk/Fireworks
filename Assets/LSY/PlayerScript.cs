using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private enum Player_Type
    {
        SHOOTER,
        CATCHER,
        END
    }

    [SerializeField]
    Player_Type _Type;
    [SerializeField]
    float move_Shooter, move_Catcher, MoveSpeed, JumpPower;
    [SerializeField]
    bool isJumping = false, isMovable_S = true, isMovable_C = true;

    void Start()
    {
        MoveSpeed = 0.1f;
        JumpPower = 500.0f;
    }

    private void Update()
    {
        jump();
        move();
    }

    void FixedUpdate()
    {
        move_Shooter = 0f;
        move_Catcher = 0f;

        if (Input.GetKey(KeyCode.A))
            move_Shooter -= 1f;
        if (Input.GetKey(KeyCode.D))
            move_Shooter += 1f;

        if (Input.GetKey(KeyCode.LeftArrow))
            move_Catcher -= 1f;
        if (Input.GetKey(KeyCode.RightArrow))
            move_Catcher += 1f;

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Floor"))
            isJumping = false;

        if (collision.gameObject.CompareTag("Wall"))
        {
            if (_Type == Player_Type.SHOOTER)
                //isMovable_S = false;
                move_Shooter *= -2;
            if (_Type == Player_Type.CATCHER)
                //isMovable_C = false;
                move_Catcher *= -2;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (_Type == Player_Type.SHOOTER)
                isMovable_S = true;
            if (_Type == Player_Type.CATCHER)
                isMovable_C = true;
        }

    }

    void move()
    {
        if (isMovable_S && _Type == Player_Type.SHOOTER)
            transform.Translate(new Vector3(move_Shooter, 0f, 0f) * MoveSpeed);
        else if (isMovable_C && _Type == Player_Type.CATCHER)
            transform.Translate(new Vector3(move_Catcher, 0f, 0f) * MoveSpeed);
    }
    void jump()
    {
        switch (_Type)
        {
            case Player_Type.SHOOTER:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (!isJumping)
                    {
                        isJumping = true;
                        this.GetComponent<Rigidbody>().AddForce(Vector3.up * JumpPower, ForceMode.Force);
                    }
                }
                break;

            case Player_Type.CATCHER:
                if (Input.GetKeyDown(KeyCode.RightShift))
                {
                    if (!isJumping)
                    {
                        isJumping = true;
                        this.GetComponent<Rigidbody>().AddForce(Vector3.up * JumpPower, ForceMode.Force);
                    }
                }
                break;
        }
    }


    
}
