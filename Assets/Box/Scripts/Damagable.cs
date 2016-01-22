using UnityEngine;
using System.Collections;

public class Damagable : MonoBehaviour {

	// Use this for initialization
    public float HP;
    public float ATK;
    public GameObject checkSphere;

    float rotateSpeed = 5.0f;
    bool onDamage;
    Vector3 damageDir,endDir,startDir;
    Quaternion damageQ;
    float count;
    [SerializeField] private ParticleSystem deathEffect;
    void Start () {
        onDamage = false;
        count = 0;
    }
	
	// Update is called once per frame
	void Update () {
       

	}

    void FixedUpdate()
    {
      /*   if (onDamage)
        {   
            Quaternion q;
            if(count<=0.4)
              q = Quaternion.LookRotation(endDir);
            else
              q = Quaternion.LookRotation(startDir);

            Debug.Log("qqq");
            count+=Time.fixedDeltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, q ,Time.fixedDeltaTime*rotateSpeed);
            
            if(count>=0.8f){
                count =0;
                onDamage = false;
                damageDir = Vector3.zero;
                transform.rotation = Quaternion.Euler(startDir);
            }
        
        }
        */
    }
    
    public void HitEvent()
    {   
        //GameObject checkSphere = GameObject.Find("Body/rightHand");
        Collider[] hitObj = Physics.OverlapSphere(checkSphere.transform.position,0.3f);
        
        foreach (Collider c in hitObj)
        {
            if((c.tag=="Player"&&c.gameObject!=gameObject)||c.tag=="Monster"){
               c.gameObject.GetComponent<Damagable>().Damage( gameObject.transform.forward,ATK);
            }
        }

    }

    void Damage(Vector3 v,float atk)
    {
        gameObject.GetComponent<Rigidbody>().velocity += v * 4.0f + new Vector3(0,3.0f,0);
        HP -= atk;
        if (HP <= 0)
            gameObject.GetComponent<BoxController>().Death();
        Destroy(Instantiate(deathEffect.gameObject, transform.position, transform.rotation) as GameObject, 0.8f);
       // onDamage = true;
       // damageDir = Vector3.Cross(v,new Vector3(0,1.0f,0));
      // endDir = new Vector3(0,-1,0) ;//damageDir*40 ;
      //  startDir = transform.forward;
       // gameObject.GetComponent<Animator>().Play("Damaged");
    }
}
