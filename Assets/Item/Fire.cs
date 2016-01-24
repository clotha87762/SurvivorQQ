using UnityEngine;
using System.Collections;

public class Fire :Item {

	// Use this for initialization
    [SerializeField] GameObject FireBallPrefab;
    int UseCount;
    //bool inUse;
    float timeCount;
    
	void Start () {
        UseCount = 5;
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
            if (timeCount >= 0.8f)
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
        Vector3 pos = new Vector3(owner.transform.position.x, owner.transform.position.y , owner.transform.position.z);
        GameObject ball = Instantiate(FireBallPrefab, pos + owner.transform.forward, owner.transform.rotation) as GameObject;
        ball.GetComponent<FireBall>().SetVec(owner.transform.forward);
        ball.GetComponent<FireBall>().SetDmage(2);
        Destroy(ball, 5.0f);
        UseCount--;
    }
}
