using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable_Object : MonoBehaviour
{
    public float torqueMultiplier = 1.0f;
    public bool active;
    public float possessionDistance = 5.0f;

    bool almostActive = false;

    Vector3 newTorque;
    Rigidbody rb;

    GameObject possessionTarget;
    GameObject gameCam;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameCam = GameObject.FindGameObjectsWithTag("MainCamera")[0];
    }

    // Update is called once per frame
    void Update()
    {
        // Stupid workaround I have to do to prevent objects from transferring ownership twice.
        // I can also assign the camera here, because why not
        if (almostActive)
        {
            active = true;
            almostActive = false;

            gameCam.GetComponent<StolenFollowCamera>().target = gameObject;
        }

        if (active)
        {
            //newTorque.x = Input.GetAxis("Vertical");
            //newTorque.y = Input.GetAxis("Tilt");
            //newTorque.z = Input.GetAxis("Horizontal");

            //newTorque = Quaternion.AngleAxis(gameCam.transform.rotation.y, new Vector3(0, 1, 0)) * newTorque;

            newTorque = gameCam.transform.right * Input.GetAxis("Vertical") + gameCam.transform.forward * -Input.GetAxis("Horizontal");
            newTorque.y = Input.GetAxis("Tilt");

            rb.AddTorque(newTorque * torqueMultiplier * Time.deltaTime * 300);

            
        }
    }



    // Do these things in late update so possession doesn't accidentally happen twice
    void LateUpdate()
    {
        // If the player has clicked down the mouse button this frame
        if (Input.GetMouseButtonDown(0) && active && possessionTarget != null)
        {
            Debug.Log(possessionDistance);
            if (Vector3.Distance(gameObject.transform.position, possessionTarget.transform.position) < possessionDistance + possessionTarget.GetComponent<Moveable_Object>().possessionDistance)
            {
                // Possession code
                active = false;
                possessionTarget.GetComponent<Moveable_Object>().almostActive = true;

                Debug.Log(possessionTarget);
            }
        }
    }


    // On Collision Enter, if the object is possessable make it the current possessed object
    void OnCollisionEnter(Collision collision)
    {
        Moveable_Object script = collision.gameObject.GetComponent<Moveable_Object>();
        if (script != null)
        {
            string objectName = collision.gameObject.name;
            possessionTarget = GameObject.Find(objectName);
            
        }
        
    }
}
