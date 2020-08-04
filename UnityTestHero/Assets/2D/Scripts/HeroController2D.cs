using UnityEngine;

public class HeroController2D : MonoBehaviour
{
    public Animator animator;
    public new Rigidbody2D rigidbody;

    public float moveSpeed = 5; //移动速度
    public float runSpeed = 8;  //奔跑速度
    public float jumpForce = 3; //跳跃力

    private Vector3 _curSpeed;
    private bool _jumping;
    private bool _running;

    void Start()
    {
        animator = animator == null ? GetComponentInChildren<Animator>() : animator;
        rigidbody = rigidbody == null ? GetComponentInChildren<Rigidbody2D>() : rigidbody;
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
        UpdateAnimation();

    }

    void UpdateInput()
    {
        int moveDir = 0;
        if (Input.GetAxis("Horizontal") < 0f)
        {
            moveDir = -1;
        }
        else if (Input.GetAxis("Horizontal") > 0f)
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
            _jumping = true;
        }
        else
        {
            //如果再次回到平台上,表示跳跃结束
        }

    }

    void UpdateTransform()
    {
        var newSpeed = _curSpeed * Time.deltaTime;
        transform.Translate(newSpeed);

        if (newSpeed.x < 0f)
        {
            var oldLocalScale = transform.localScale;
            oldLocalScale.x = -Mathf.Abs(oldLocalScale.x);
            transform.localScale = oldLocalScale;
        }
        else if (newSpeed.x > 0f)
        {
            var oldLocalScale = transform.localScale;
            oldLocalScale.x = Mathf.Abs(oldLocalScale.x);
            transform.localScale = oldLocalScale;
        }

    }

    void UpdateRigidbody()
    {
        if (rigidbody == null)
            return;


    }


    void UpdateAnimation()
    {
        if (animator == null)
            return;

        animator.SetBool("Running", _running);
        animator.SetInteger("Speed", (int)_curSpeed.x); 
    }


    void ClearState()
    {

    }
}



