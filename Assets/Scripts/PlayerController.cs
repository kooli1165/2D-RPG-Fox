using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D coll;
    private Collider2D smallcoll;
    //private Collider2D tmp;
    private Animator anim;
    //public Rigidbody2D rd;
    public float speed = 10f;
    public float jumpforce;
    public Transform groundCheck;
    public Transform headCheck;
    public LayerMask ground;

    //��Ծ�������ж�
    public bool isGround, isJump, isDashing, isHead;
    public int jumpCount;
    bool jumpPressed;

    //
    public int Cherry=0,Gem=0;
    public Text CherryNumber,GemNumber;

    private float horizontalmove = 0;
    private float verticalmove = 0;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        smallcoll = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump")&&jumpCount>0)
        {
            jumpPressed = true;
            anim.SetBool("jumping", true);
        }

    }
    void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
        isHead = Physics2D.OverlapCircle(headCheck.position, 0.49f, ground);
        //tmp= Physics2D.OverlapCircle(headCheck.position, 0.5f, ground);
        Movement();
        Crouch();
        Jump();
        SwitchAnimation();
    }

    void Movement()
    {
        horizontalmove = Input.GetAxisRaw("Horizontal");
        anim.SetFloat("running", Mathf.Abs(horizontalmove));
        rb.velocity = new Vector2(horizontalmove * speed, rb.velocity.y);

        if(horizontalmove!=0)
        {
            transform.localScale = new Vector3(horizontalmove, 1, 1);
        }
    }

    void Crouch()
    {
        verticalmove = Input.GetAxisRaw("Vertical");
        if(verticalmove<0)
        {
            anim.SetBool("crouching", true);
            coll.enabled = false;
            smallcoll.enabled = true;
        }
        else if(!isHead)
        {
            anim.SetBool("crouching", false);
            coll.enabled = true;
            smallcoll.enabled = false;
        }
        //Debug.Log(tmp);
        //Debug.Log(isHead);
    }

    void Jump()
    {
        if(isGround)
        {
            jumpCount = 1;
        }
        if(jumpPressed&&isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            jumpPressed = false;
        }
        else if(!isGround&&jumpPressed&&jumpCount>0)
        {
            jumpCount--;
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            jumpPressed = false;
        }
    }

    void SwitchAnimation()
    {
        if(anim.GetBool("jumping"))
        {
            if(rb.velocity.y<0)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }
        else if(anim.GetBool("falling"))
        {
            if(isGround)
            {
                anim.SetBool("falling", false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Collection")
        {
            Cherry += 1;
            CherryNumber.text = Cherry.ToString();
            Destroy(collision.gameObject);
        }else if(collision.tag=="Gem")
        {
            Gem += 1;
            GemNumber.text = Gem.ToString();
            Destroy(collision.gameObject);
        }
    }
}
