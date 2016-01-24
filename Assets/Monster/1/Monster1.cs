using UnityEngine;
using System.Collections;

public class Monster1 : MonoBehaviour {

	// Use this for initialization
    BoxController monster;
    Animator mAnimator;
    Vector3 roamingVec;
    public GameObject mother;
    //public GameObject checkFall;

    float timeCount = 0;
    float timeCountChase = 0;
    float randomWait;
    float randomMove;
    bool isRoaming;
    bool isGrounded;
    GameObject chasing;
    bool isChase;
    public float atkRange = 50.0f;
    public float chaseWait;
    public float attackCD;
    float attackCDCount;

    void Start () {
        mAnimator = GetComponent<Animator>();
        randomWait = ((float)Random.Range(1000,4000))/1000.0f;
        isRoaming = false;
        monster = transform.GetComponent<BoxController>();
        isChase = false;
        chaseWait = 1.0f;
        attackCD = 1.5f;
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    void FixedUpdate()
    {
        if(!isChase)
        timeCount += Time.fixedDeltaTime;

        timeCountChase += Time.fixedDeltaTime;
        //Debug.Log(timeCountChase);
        Quaternion temp = transform.rotation;


        isGrounded = false;

        float minDis = 1000000;

        Collider[] toFind = Physics.OverlapSphere(transform.position, atkRange);
        GameObject tempChase;

        if (timeCountChase >= chaseWait||isChase)
        {

            foreach (Collider c in toFind)
            {

                if (c.gameObject.tag == "Player"&& c.gameObject != mother)
                {

                    float dis = Vector3.Distance(c.gameObject.transform.position, transform.position);
                    if (dis < minDis && dis <= 5.0f)
                    {
                        //Debug.Log(c.gameObject.tag);
                        minDis = dis;
                        if (!isChase)
                        {
                            isChase = true;
                            chasing = c.gameObject;
                        }
                    }

                }
            }
            if (minDis >= 1000000)
            {
                isChase = false;
                chasing = null;
            }
            attackCDCount += Time.fixedDeltaTime;
            GoAttack();
            timeCountChase = 0;

        }

        if (!isChase)
        {
            if (!isRoaming && timeCount >= randomWait)
            {
                bool flag = false;
                int tempCount = 0;
                while (!flag && tempCount < 30)
                {

                    roamingVec = new Vector3(((float)Random.Range(-100, 100)) / 100.0f, 0, ((float)Random.Range(-100, 100)) / 100.0f);
                    roamingVec.Normalize();
                    transform.LookAt(transform.position + roamingVec);
                    flag = CheckGrounded();
                  //  Debug.Log("GGGGG");
                    tempCount++;
                }

                if (tempCount >= 30)
                {
                    return;
                }



                randomMove = ((float)Random.Range(300, 800)) / 100.0f;
                isRoaming = true;
                timeCount = 0;
            }

            if (isRoaming && timeCount >= randomMove)
            {
                isRoaming = false;
                roamingVec = Vector3.zero;
                randomWait = ((float)Random.Range(1000, 4000)) / 1000.0f;
                timeCount = 0;
            }

            if (isRoaming)
            {
                bool flag = CheckGrounded();
                if (!flag)
                {
                    isRoaming = false;
                    roamingVec = Vector3.zero;
                    timeCount = 0;
                    randomWait = ((float)Random.Range(1000, 2000)) / 1000.0f;
                }

            }
            if (isGrounded)
                transform.rotation = temp;
        }
        else if (isChase)
        {
            isRoaming = false;
            timeCount = 0;
            randomWait = ((float)Random.Range(1000, 2000)) / 1000.0f;
            roamingVec = chasing.transform.position - transform.position;
            roamingVec.Normalize();

        }
           // Debug.Log("roaming");
            roamingVec = new Vector3(roamingVec.x, 0, roamingVec.z);
            if(isChase)
            monster.Move(roamingVec, true);
            else
            monster.Move(roamingVec, false);

    }

    void GoAttack()
    {
        if (attackCDCount < attackCD) return;

        if (chasing != null && isChase)
        {
            float d = Vector3.Distance(chasing.transform.position,transform.position);
            if (d <= 0.8f)
            {
               // Debug.Log("Attack");
                monster.Attack();
                attackCDCount = 0;
            }
        }

    }


    bool CheckGrounded()
    {
            Transform checkFall = transform.FindChild("checkFall");
            
            Collider[] toCheck = Physics.OverlapSphere(checkFall.position, 0.5f);
            isGrounded = false;
            
            foreach (Collider c in toCheck)
            {
               // Debug.Log(c.transform.tag);
                if (c.transform.tag!="Monster"&&c.transform.tag != "Player" && c.transform.tag != "PlayerGroundCheck" && !c.isTrigger)
                {
                   
                    isGrounded = true;
                    
                }
            }

            return isGrounded;
     }

}
