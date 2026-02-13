using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float ExtraGravity=30f;
    [SerializeField] float JumpForce=8f;
    [SerializeField]float speed=10f;
    
    Rigidbody rb;
    Vector3 MoveDirection;
    [SerializeField] float turnSpeed = 5f;
    float targetY;
    

    bool isGrounded=true;
    int StepIndex=0;


    void Start()
    {
        targetY = transform.eulerAngles.y;

        rb= GetComponent<Rigidbody>();
        rb.freezeRotation=true;
        MoveDirection=transform.forward;

        int level=PlayerPrefs.GetInt("Level",0);
        if (level==0)
        {
            speed=10f;
        }
        else if (level == 1)
        {
            speed=12;
        }
        else if (level == 2)
        {
            speed=14;
        }
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)&& isGrounded)
        {
            rb.AddForce(Vector3.up *JumpForce,ForceMode.Impulse);
            isGrounded=false;
        }
        if (StepIndex==0 && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            targetY -= 90f;
            StepIndex=1;
        }

        if (StepIndex==1 && Input.GetKeyDown(KeyCode.RightArrow))
        {
            targetY += 90f;
            StepIndex=0;
        }

        
        
    }
    void FixedUpdate()
    {
        float newY = Mathf.LerpAngle(transform.eulerAngles.y,targetY,turnSpeed * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Euler(0, newY, 0);
        MoveDirection = transform.forward;


        rb.AddForce(Vector3.down * ExtraGravity, ForceMode.Acceleration);
        Vector3 finalVelocity = MoveDirection.normalized * speed;
        finalVelocity.y = rb.velocity.y;
        rb.velocity = finalVelocity;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded=true;
        }
    }
    

}
