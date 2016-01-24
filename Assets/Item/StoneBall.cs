using UnityEngine;
using System.Collections;

public class StoneBall : MonoBehaviour {

	// Use this for initialization
    GameObject owner;
    float speed;
    float damage;
    Vector3 nowVec;
    Vector3 ownerLast;
    [SerializeField]
    private ParticleSystem explode; 

	void Start () {
        nowVec = Vector3.zero;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //Vector3 delta = transform.position - owner.transform.position;
        //delta.Normalize();
        //nowVec = Vector3.Cross(delta, owner.transform.up*3);
        //transform.position += 1.0f * nowVec * Time.deltaTime;
        transform.RotateAround(owner.transform.position, owner.transform.up, 400 * Time.fixedDeltaTime);
        transform.position += owner.transform.position - ownerLast;
        ownerLast = owner.transform.position;
    }

    void OnTriggerEnter(Collider c)
    {
       // if(owner==null)return;
        
        if (!c.isTrigger && (c.gameObject.tag == "Player" || c.gameObject.tag == "Monster")&&c.gameObject!=owner)
        {
            Debug.Log("DDDD");
            Vector3 temp = -(c.gameObject.transform.position - owner.transform.position);
            temp.y=0;
            c.gameObject.GetComponent<Damagable>().Damage(temp*20.0f, damage);
            Destroy(Instantiate(explode.gameObject, transform.position, Quaternion.identity) as GameObject, explode.startLifetime);
            Destroy(gameObject);
        }
    }

    public void SetSpeed(float s)
    {
        speed = s;
    }

    public void SetDamage(float d)
    {
        damage = d;
    }

    public void SetOwner(GameObject g)
    {
        Debug.Log("OwO");
        owner = g;
        ownerLast = owner.transform.position;
    }
}
