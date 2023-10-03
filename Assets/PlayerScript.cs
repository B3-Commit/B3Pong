using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public KeyCode up = KeyCode.UpArrow;
    public KeyCode down = KeyCode.DownArrow;

    [SerializeField] float speed = 8.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(up))
        {
            transform.position += Vector3.up * speed * Time.deltaTime;            
        }
        if (Input.GetKey(down))
        {
            transform.position += Vector3.down * speed * Time.deltaTime;
        }
        
    }
}
