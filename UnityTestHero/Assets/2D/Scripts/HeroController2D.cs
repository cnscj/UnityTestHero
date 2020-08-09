using UnityEngine;

public class HeroController2D : MonoBehaviour
{
    public Animator[] animators;
    public new Rigidbody2D rigidbody;

    public float moveSpeed = 1; //移动速度
    public float runSpeed = 3;  //奔跑速度
    public float jumpForce = 3; //跳跃力
    public int jumpCount = 1;   //跳跃次数

    private Vector3 _curSpeed;
    private int _curJumpCount;
    private bool _jumping;
    private bool _running;

    void Start()
    {
        animators = (animators == null || animators.Length <= 0) ? GetComponentsInChildren<Animator>() : animators;
        rigidbody = rigidbody == null ? GetComponentInChildren<Rigidbody2D>() : rigidbody;

        _curJumpCount = jumpCount;
    }

    void Update()
    {
        UpdateInput();
        UpdateTransform();


        ClearState();
    }

    void FixedUpdate()
    {
        UpdateRigidbody();
        UpdateAnimator();

    }

    void UpdateInput()
    {
        var oldSpeed = _curSpeed;
        _curSpeed = Vector3.zero;
        int moveDir = 0;
        if (Input.GetKey(KeyCode.A))
        {
            moveDir = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveDir = 1;
        }


        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            _curSpeed.x = runSpeed * moveDir;
            _running = true;
        }
        else
        {
            _curSpeed.x = moveSpeed * moveDir;
            _running = false;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (!_jumping || _jumping && _curJumpCount > 0)
            {
                _curSpeed.y = jumpForce;
                _curJumpCount--;
                _jumping = true;
            }
        }
    }

    void UpdateTransform()
    {
        if (!Mathf.Approximately(_curSpeed.x, 0f))
        {
            transform.localScale = new Vector3(
                Mathf.Sign(_curSpeed.x),
                transform.localScale.y,
                transform.localScale.z);
        }
    }

    void UpdateRigidbody()
    {
        if (rigidbody == null)
            return;

        var horizontalSpeed = _curSpeed.x;
        var verticalSpeed = rigidbody.velocity.y;

        if (!Mathf.Approximately(_curSpeed.y,0f))
        {
            verticalSpeed = _curSpeed.y;
        }

        if (Mathf.Approximately(rigidbody.velocity.y, 0f))
        {
            _curJumpCount = jumpCount;
            _jumping = false;
        }

        rigidbody.velocity = new Vector2(horizontalSpeed, verticalSpeed);
    }

    void UpdateAnimator()
    {
        if (animators == null)
            return;

        if (animators.Length <= 0)
            return;

        if (Mathf.Approximately(rigidbody.velocity.x, 0f) && Mathf.Approximately(rigidbody.velocity.y, 0f))
        {
            foreach(var animator in animators) animator.Play("Idle");
        }
        else
        {
            if (!Mathf.Approximately(rigidbody.velocity.y, 0f))
            {
                if (_jumping)
                {
                    if (rigidbody.velocity.y > 0f)
                    {
                        foreach (var animator in animators) animator.Play("JumpUp");
                    }
                    else if (rigidbody.velocity.y < 0f)
                    {
                        foreach (var animator in animators) animator.Play("JumpFull");
                    }
                }
            }
            else
            {
                if (_running)
                {
                    foreach (var animator in animators) animator.Play("Run");
                }
                else
                {
                    foreach (var animator in animators) animator.Play("Walk");
                }
            }
            
        }
    }


    void ClearState()
    {

    }
}



