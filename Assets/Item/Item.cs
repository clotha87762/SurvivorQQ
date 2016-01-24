using UnityEngine;
using System.Collections;

public abstract class Item : MonoBehaviour {

	// Use this for initialization
    protected bool eaten;
    protected GameObject owner;

	void Start () {
        eaten = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider c)
    {
        if (eaten)
            return;
        if (c.gameObject.tag == "Player")
        {
            c.gameObject.GetComponent<Damagable>().SetItem(this);
            eaten = true;
            gameObject.GetComponent<Renderer>().enabled=false;
            owner = c.gameObject;
        }
    }

    public abstract void Use();

    
}
