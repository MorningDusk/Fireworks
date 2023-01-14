using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    Rigidbody rigid;
    public Animator Anim;
    public enum Player_Type
    {
        SHOOTER,
        CATCHER,
        END
    }
    public enum STATE
    {
        IDLE,
        MOVE,
        ACTION,
        STATE_END
    }

    [SerializeField]
    Player_Type _Type;
    [SerializeField]
    STATE _State;

    [SerializeField]
    int Health, MaxHealth, Bullet, MaxBullet, Piece, PiecePerBullet;

    [SerializeField]
    float Speed, JumpPower;
    float move_Shooter, move_Catcher;
    float jump_Y = 0f;


    [SerializeField]
    bool isJumping = false, isGround = false;
    bool isMovable_S = true, isMovable_C = true;

    // Getter
    public int GetHealth() { return Health; }
    public int GetMaxHealth() { return MaxHealth; }
    public int GetPiece() { return Piece; }
    public int GetPiecePerBullet() { return PiecePerBullet; }
    public int GetBullet() { return Bullet; }
    public int GetMaxBullet() { return MaxBullet; }
    public Player_Type GetPlayerType() { return _Type; }

    // Setter
    public void DecreaseHealth(int Value) { Health -= Value; }

    private void Awake()
    {
        _State = STATE.IDLE;
        rigid = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();
    }

    void Start()
    {

        MaxHealth = 10;
        Health = MaxHealth;

        MaxBullet = 10;
        Bullet = MaxBullet;

        Piece = 0;
        PiecePerBullet = 5;

        Speed = 0.1f;
        JumpPower = 20.0f;
    }
    
    // 플레이어 초기화
    public void playerInit()
    {
        _State = STATE.IDLE;
        rigid = gameObject.GetComponent<Rigidbody>();

        MaxHealth = 10;
        Health = MaxHealth;

        MaxBullet = 10;
        Bullet = MaxBullet;

        Piece = 0;
        PiecePerBullet = 5;

        Speed = 0.1f;
        JumpPower = 20.0f;
    }

    private void Update()
    {
        jump();
        move();
        attack();
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
        Debug.Log(this.gameObject + " " + collision.gameObject);
        if (collision.gameObject.CompareTag("Floor"))
        {
            isJumping = false;
            isGround = true;
            jump_Y = this.transform.position.y;
            Anim.SetBool("isJump", false);
        }

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
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGround = false;
        }
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
        {
            Vector3 moveVec = new Vector3(move_Shooter, 0f, 0f);
            if (isJumping)
            {
                //transform.Translate(moveVec * Speed * 0.7f);
                transform.position += moveVec * Speed * 0.7f;
                transform.LookAt(transform.position + moveVec);
            }
            else
            {
                //transform.Translate(moveVec * Speed);
                transform.position += moveVec * Speed;
                transform.LookAt(transform.position + moveVec);
            }
            Anim.SetBool("isWalk", moveVec != Vector3.zero);
        }

        if (isMovable_C && _Type == Player_Type.CATCHER)
        {
            Vector3 moveVec = new Vector3(move_Catcher, 0f, 0f);
            if (isJumping)
            {
                //transform.Translate(moveVec * Speed * 0.7f);
                transform.position += moveVec * Speed * 0.7f;
                transform.LookAt(transform.position + moveVec);
            }
            else
            {
                //transform.Translate(moveVec * Speed);
                transform.position += moveVec * Speed;
                transform.LookAt(transform.position + moveVec);
            }
            Anim.SetBool("isWalk", moveVec != Vector3.zero);
        }

        //
    }
    void jump()
    {
        if (isJumping)
        {
            if (jump_Y <= this.transform.position.y)
            {
                jump_Y = this.transform.position.y;
            }
            else
            {
                //Debug.Log("Trigger Off");
                this.GetComponent<MeshCollider>().isTrigger = false;
            }
        }
        else if (isGround)
        {
            switch (_Type)
            {
                case Player_Type.SHOOTER:
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        if (!isJumping)
                        {
                            isJumping = true;
                            this.GetComponent<MeshCollider>().isTrigger = true;
                            this.GetComponent<Rigidbody>().AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
                            Anim.SetBool("isJump", true);
                        }
                    }
                    break;

                case Player_Type.CATCHER:
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        if (!isJumping)
                        {
                            isJumping = true;
                            this.GetComponent<MeshCollider>().isTrigger = true;
                            this.GetComponent<Rigidbody>().AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
                            Anim.SetBool("isJump", true);
                        }
                    }
                    break;
            }
        }
        //
    }
    void attack()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _Type == Player_Type.SHOOTER)
        {
            if (Bullet > 0)
            {
                Debug.Log("Shot!");
                Anim.SetTrigger("doAttack");
                Bullet--;
            }
            else
            {
                Debug.Log("No Bullet");
            }
        }
    }

    // Catcher 가 유성조각 획득 시 실행하는 함수, Fragment 스크립트에서 호출됨
    public void catchFragment()
    {
        Piece++; // 조각 획득
        if (Piece % PiecePerBullet==0) // 만약 최대 유성조각 수까지 유성조각을 모으면
        {
            Piece = 0; // 조각 초기화
            if (Bullet != MaxBullet) // 탄약수가 최대가 아니라면
            {
                Bullet++; // 총알 개수 증가
                UIManager.Instance.UI_changeBullet(); // 총알 개수 증가하는 ux
            }; 
        }
        UIManager.Instance.UI_changeFragment(); // UI 변경
    }
    
}
