using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Controll : MonoBehaviour {


    public sluggerstates state;


    Vector3 normal;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

       

        if (Input.GetKey(KeyCode.Space))
        {
            state = sluggerstates.roll;
            Debug.Log("roll");
        }
        else
        {
            state = sluggerstates.crawl;
            //CheckGroundHit();
            Debug.DrawRay(transform.position, -normal, Color.red, 100);
            Debug.Log("crawl");

            Debug.Log(normal);

        }



    }

    float groundDistance = 2;



    /*
    Vector3 CheckGroundHit()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.down));
        Vector3 hitNormal = Vector3.zero;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(ray ,out hit, groundDistance))
        {
            hitNormal = hit.normal;
            Debug.DrawRay(ray.origin, ray.direction * groundDistance, Color.red);
        }
        return hitNormal;
    }
    */

    void OnCollisionStay(Collision collision)
    {

        if (state == sluggerstates.crawl)
        {
            normal = collision.contacts[0].normal;
            //Debug.DrawRay(transform.position, -normal, Color.red, 10);
            transform.rotation =  Quaternion.LookRotation(transform.right, normal);

            transform.forward = GetComponent<Rigidbody>().velocity;

            /*
            float sValue = 0.9f;
            if (normal.y <= 0.6f || normal.z <= 0.6f)
            {
                /// crawlProblem
               // transform.position = new Vector3(transform.position.x, transform.position.y + normal.y, transform.position.z + normal.z);
                print("to short");
            }
            */



            /*
            Vector3 v  = collision.ClosestPoint(transform.position);
           // Vector3 v = collision.ClosestPointOnBounds(transform.position);
            Vector3 normal = transform.position - v;
            transform.rotation = Quaternion.LookRotation(transform.forward, normal);
            Debug.DrawRay(transform.position, -normal, Color.red, 10);
            */
            Debug.Log("crawl + Col");
        }

    }

}

public enum sluggerstates
{
    crawl, roll
}
