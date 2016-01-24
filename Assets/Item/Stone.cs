using UnityEngine;
using System.Collections;

public class Stone : Item {

	// Use this for initialization
    [SerializeField] GameObject StoneBallPrefab;
    int UseCount;
    //bool inUse;
    float timeCount;

    void Start()
    {
        UseCount = 1;
        //inUse = false;
        timeCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!eaten) return;

        if (owner.GetComponent<BoxController>().acting)
        {
            timeCount += Time.deltaTime;
            if (timeCount >= 1.5f)
            {
                owner.GetComponent<BoxController>().acting = false;
                timeCount = 0;
            }
        }
        else if (UseCount <= 0)
        {
            Destroy(this);
        }

    }

    public override void Use()
    {
        owner.GetComponent<BoxController>().acting = true;
        Vector3 pos = new Vector3(owner.transform.position.x, owner.transform.position.y, owner.transform.position.z);
        GameObject ball = Instantiate(StoneBallPrefab, pos + owner.transform.forward*1.0f, owner.transform.rotation) as GameObject;
        ball.GetComponent<StoneBall>().SetSpeed(1.0f);
        ball.GetComponent<StoneBall>().SetDamage(2.0f);
        ball.GetComponent<StoneBall>().SetOwner(owner);
        
        Destroy(ball, 15.0f);
        UseCount--;
    }
}
