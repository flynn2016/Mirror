using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public float character_speed; //character movement speed
    public float jump_acceleration; //character jump accelration
    public float gravity_coefficient; //how much character affected by the gravity if not grounded
    public float ray_spacing; // how far away are the rays from origin
    public float ray_offset_x; // how much you space you offset on the x direction rays
    public float ray_offset_y; // how much you space you offset on the y direction rays
    public bool isbear; 

    private float disToGround;
    private float disToSide;
    private Vector3 movement_x;
    private Vector3 movement_y;

    public void Start()
    {
        disToGround = GetComponent<Collider>().bounds.extents.y;
        disToSide = GetComponent<Collider>().bounds.extents.x;
    }


    public void OnUpdate()
    {
        Debug.DrawRay((transform.position), Vector3.down*(disToGround+ ray_offset_y), Color.blue);
        Debug.DrawRay((transform.position + Vector3.up * ray_spacing), Vector3.left * (disToSide + ray_offset_x), Color.blue);
        Debug.DrawRay((transform.position - Vector3.up * ray_spacing), Vector3.left * (disToSide + ray_offset_x), Color.blue);
        Debug.DrawRay((transform.position + Vector3.up * ray_spacing), Vector3.right * (disToSide + ray_offset_x), Color.blue);
        Debug.DrawRay((transform.position - Vector3.up * ray_spacing), Vector3.right * (disToSide + ray_offset_x), Color.blue);

        Move();
        Jump();
        
    }

    void Move()
    {
        Physics.Raycast(transform.position + Vector3.up * ray_spacing, Vector3.left, out RaycastHit left_ray1, disToSide + ray_offset_x);
        Physics.Raycast(transform.position - Vector3.up * ray_spacing, Vector3.left, out RaycastHit left_ray2, disToSide + ray_offset_x);
        Physics.Raycast(transform.position + Vector3.up * ray_spacing, Vector3.right, out RaycastHit right_ray1, disToSide + ray_offset_x);
        Physics.Raycast(transform.position - Vector3.up * ray_spacing, Vector3.right, out RaycastHit right_ray2, disToSide + ray_offset_x);

        if (Input.GetAxis("Horizontal") < 0)
        {
            if (left_ray1.transform != null) //detect collision left_ray1
            {
                float hit_xbound = transform.GetComponent<Collider>().bounds.extents.x + left_ray1.transform.GetComponent<Collider>().bounds.extents.x;
                transform.position = new Vector3(left_ray1.transform.position.x + hit_xbound, transform.position.y, transform.position.z);
            }

            else if (left_ray2.transform != null) //detect collision left_ray2
            {
                float hit_xbound = transform.GetComponent<Collider>().bounds.extents.x + left_ray2.transform.GetComponent<Collider>().bounds.extents.x;
                transform.position = new Vector3(left_ray2.transform.position.x + hit_xbound, transform.position.y, transform.position.z);
            }

            else
            {
                movement_x = Vector3.right* character_speed * Input.GetAxis("Horizontal");
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, (!isbear)?0:180, transform.eulerAngles.z);
            }
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            if (right_ray1.transform != null) //detect collision right_ray1
            {
                float hit_xbound = transform.GetComponent<Collider>().bounds.extents.x + right_ray2.transform.GetComponent<Collider>().bounds.extents.x;
                transform.position = new Vector3(right_ray1.transform.position.x - hit_xbound, transform.position.y, transform.position.z);
            }
            else if (right_ray2.transform != null) //detect collision right_ray2
            {
                float hit_xbound = transform.GetComponent<Collider>().bounds.extents.x + right_ray2.transform.GetComponent<Collider>().bounds.extents.x; ;
                transform.position = new Vector3(right_ray2.transform.position.x - hit_xbound, transform.position.y, transform.position.z);
            }
            else
            {
                movement_x =Vector3.left* character_speed * Input.GetAxis("Horizontal");
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, (!isbear) ? 180 : 0, transform.eulerAngles.z);
            }
        }
        else
        {
            movement_x = Vector3.zero;
        }
            transform.Translate(movement_x * Time.deltaTime);

    }

    void Jump()
    {
        if (GroundCheck())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                movement_y = Vector3.up * jump_acceleration;
            }
            else
            {
                movement_y = Vector3.zero;
            }
        }
        else
        {
            movement_y += Vector3.down * gravity_coefficient;
        }
        transform.Translate(movement_y * Time.deltaTime);
    }

    bool GroundCheck()
    {
        Physics.Raycast(transform.position, (!isbear)?Vector3.down:Vector3.up, out RaycastHit groundcheckhit, disToGround + ray_offset_y);
        if (groundcheckhit.transform!=null)
        {
            if (groundcheckhit.transform.tag == "Terrain")
            {
                float hit_ybound = transform.GetComponent<Collider>().bounds.extents.y+ groundcheckhit.transform.GetComponent<Collider>().bounds.extents.y;
                transform.position = new Vector3(transform.position.x,(!isbear)?groundcheckhit.transform.position.y+ hit_ybound :
                    groundcheckhit.transform.position.y - hit_ybound, transform.position.z);
                return true;
            }
        }
        return false;
    }
}
