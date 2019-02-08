using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class characterController : MonoBehaviour {

    public float speed = 10.0F;
    public float jump_speed = 100;
    public float max_jump_height = 75.0F;
    public Rigidbody projectile;
    public Text projectileText;

    int projectile_num;
	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        projectile_num = 0;
        projectileText.text = "Projectiles: " + projectile_num.ToString();
	}
	
	// Update is called once per frame
	void Update () {

        float vertical_movement = Input.GetAxis("Vertical") * speed;
        float horizontal_movement = Input.GetAxis("Horizontal") * speed;
        float jump_movement = 0;

        vertical_movement *= Time.deltaTime;
        horizontal_movement *= Time.deltaTime;


        if (Input.GetKeyDown(KeyCode.Space) && (transform.position.y<max_jump_height))
        {
            jump_movement = jump_speed * Time.deltaTime;
        }

        if (transform.position.y>1.5){
            vertical_movement = 0;
            horizontal_movement = 0;
        }
        else
        {
            if (Input.GetKeyDown("f") && projectile_num > 0)
            {
                Rigidbody new_proj = Instantiate(projectile, new Vector3(transform.position.x, 1, transform.position.z), Quaternion.identity) as Rigidbody;
                new_proj.AddForce(Vector3.forward * speed * 30);
                projectile_num--;
                projectileText.text = "Projectiles: " + projectile_num.ToString();
            }
        }

        transform.Translate(horizontal_movement, jump_movement, vertical_movement);

        if (Input.GetKeyDown("escape"))
            Cursor.lockState = CursorLockMode.None;
	}

    void OnTriggerEnter(Collider other)
    {  
        
        if (other.gameObject.CompareTag("Projectile"))
        {
            Destroy(other.gameObject);
            projectile_num++;
            projectileText.text = "Projectiles: " + projectile_num.ToString();
        }
    }

}
