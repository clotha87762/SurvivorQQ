using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class BoxController : MonoBehaviour {

    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float rotateSpeed = 10.0f;
    [SerializeField] private float jumpPower = 10.0f;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [Range(0.0f, 1.0f)][SerializeField] private float runExtraSpeedScale = 0.5f;      // speed * (1 + runExtraSpeedScale)
    [Range(0.0f, 1.0f)][SerializeField] private float notGroundedScale = 0.5f;
    [Range(0.0f, 1.0f)][SerializeField] private float attackStateScale = 0.3f;
    [SerializeField] private ParticleSystem deathEffect;
    [SerializeField] private ParticleSystem spawnEffect;

    private int DeathTimes = 0;

    private Rigidbody mRigidbody;
    private Collider mCollider;
    private Animator mAnimator;
    private Transform mGroundCheck;
    private Collider mAttackRange;
    private TriggerChecker mGroundTrigger;
    private List<Renderer> mBodyRenderers = new List<Renderer>();
  
    private float deathTime = 2.0f;

    private Vector3 jumpVelocity;
    private bool mGrounded = true;

    private bool mAttack = false;
    private int mAttackCount = 1;
    private bool attackHit = false;

    private bool controlByPlayer;

    private Damagable damagable;

    private float rollSpeed = 0.0f;
    public const float rollInitialSpeed = 15.0f;
    bool rollable = true;

    public bool acting = false;
    
    public bool isPlayer ;

    //private float STR = 3.0f;
    //private float VIT = 10.0f;
    //private float AGI = 5.0f;

    void Awake()
    {
        if (transform.gameObject.GetComponent<UserInputController>()) controlByPlayer = true;
        else controlByPlayer = false;
    }

	// Use this for initialization
	void Start () {
        mRigidbody = transform.GetComponent<Rigidbody>();
        mCollider = transform.GetComponent<Collider>();
        mAnimator = transform.GetComponent<Animator>();
		damagable = transform.GetComponent<Damagable>();
        
        GameObject mainBody = transform.Find("Body").gameObject;
        mBodyRenderers = new List<Renderer>(mainBody.transform.GetComponentsInChildren<Renderer>());

        mGroundCheck = transform.Find("groundCheck").transform;
        mAttackRange = transform.Find("attackRange").GetComponent<Collider>();
        mGroundTrigger = transform.Find("groundCheckCollider").GetComponent<TriggerChecker>();
	}

    void Update()
    {
        GroundStateCheck();
    }

    public void Roll()
    {
        if (rollable&&!acting)
        {
            mAnimator.Play("Roll");
            rollSpeed = rollInitialSpeed;
            startRoll();
        }
    }

    public void Move(Vector3 mMove, bool mRunning)
    {
        //not moving, return
        UpdateAnimator(mMove, mRunning);
        //if (mMove.magnitude <= 0.0f) return;
        if (mMove.magnitude > 1.0f) mMove.Normalize();
        if (rollSpeed > 0.01f || rollSpeed < -0.01f) rollSpeed -= Time.deltaTime * rollInitialSpeed/1.0f;
        else rollSpeed = 0.0f;
        //transform.LookAt(transform.position + mMove);
        //rotate character to move direction if not in attack mode
        if (mMove.magnitude > 0.01f && !mAttack)
        {
			Vector3 lookRotationVector = transform.position + mMove - transform.position;
			if(lookRotationVector.magnitude > 0.1f)
			{
				Quaternion targetRotation = Quaternion.LookRotation(lookRotationVector);
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotateSpeed);
			}
        }

        Vector3 originalVelocity = mRigidbody.velocity;
        //OnAir move slower
        if (!mGrounded) mMove = mMove * notGroundedScale;
        //if (!mGrounded) mMove = mMove * notGroundedScale + mRigidbody.velocity * (1.0f - notGroundedScale);
        //if (!mGrounded) return;
        //OnAttack move slower
        if (mAttack) mMove = mMove * attackStateScale;
        float extraSpeed = (mRunning) ? 1.0f + runExtraSpeedScale : 1.0f; 
        mRigidbody.velocity = mMove * speed * extraSpeed + new Vector3(0, originalVelocity.y, 0);
        if (!mGrounded) mRigidbody.velocity += jumpVelocity * (1.0f-notGroundedScale);

        mRigidbody.velocity += transform.forward * rollSpeed;

    }

    public void Jump()
    {
        //jump if grounded
        if (mGrounded)
        {
            mRigidbody.velocity = new Vector3(mRigidbody.velocity.x, jumpPower, mRigidbody.velocity.z);
            jumpVelocity = new Vector3( mRigidbody.velocity.x, 0, mRigidbody.velocity.z);
        }
    }

    public void UpdateAnimator(Vector3 move, bool mRunning)
    {
        //in attack
        if (mAttack) mAnimator.Play("boxAttack00");
        //in moving
        if (move.magnitude > 0.0f && mGrounded)
        {
            mAnimator.SetBool("Walking", true);
            if (mRunning) mAnimator.speed = 1.0f + runExtraSpeedScale;
            else mAnimator.speed = 1.0f;
        }
        else
        {
            mAnimator.SetBool("Walking", false);
            mAnimator.speed = 1.0f;
        }
    }
    //check if character grounded, update mGrounded
    void GroundStateCheck()
    {
        /*
        RaycastHit hit;
        if (Physics.Raycast(mGroundCheck.position + Vector3.up * 0.1f, Vector3.down, out hit, groundCheckDistance)) 
            mGrounded = true;
        else 
            mGrounded = false;
        */
        mGrounded = mGroundTrigger.isGrounded();
    }

    public void Attack()
    {
        if (acting) return;

        mAttack = true;
        acting = true;
    }

    public void AttackCompleteEvent()
    {
        mAttack = false;
        acting = false;
    }

    public void AttackHitEvent()
    {
        attackHit = true;
        GetComponent<Damagable>().HitEvent();
    }

    public void AttackLeaveEvent()
    {
        attackHit = false;
    }

    public void Death()
    {
        Debug.Log(" die." + DeathTimes);
        DeathTimes++;
        jumpVelocity = Vector3.zero;

        //mBody.SetActive(false);
        mRigidbody.constraints = RigidbodyConstraints.FreezePosition;
        mRigidbody.velocity = Vector3.zero;
        mRigidbody.angularVelocity = Vector3.zero;
        SetRenderer(false);
        Destroy(Instantiate(deathEffect.gameObject, transform.position, transform.rotation) as GameObject, deathEffect.startLifetime);
        if (!controlByPlayer) Destroy(this);
        else
        {
            transform.gameObject.GetComponent<UserInputController>().SetEnable(false);

        }
    }

    public void SetRenderer(bool set)
    {
        foreach (Renderer element in mBodyRenderers)
        {
            element.enabled = set;
        }
    }

    public void lerpShow()
    {
        foreach (Renderer element in mBodyRenderers)
        {
            element.material.DOFade(0.0f, 1.0f).From();
        }
    }

    public void startRoll()
    {
        Debug.Log("STARTROLL");
        rollable = false;
        acting = true;
    }

    public void endRoll()
    {
       Debug.Log("ENDROLL");
        rollable = true;
        acting = false;
    }

    public void UseItem()
    {
       // acting = true;
        if (acting) return;

        damagable.UseItem();
    }
}
