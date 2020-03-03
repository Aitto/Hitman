using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    private float m_speed = 5.0f,m_jumpForce = 10.0f;
    private Vector3 m_forward,m_right,m_up;
    // Start is called before the first frame update
    [SerializeField]
    private CameraMovement m_cm;
    private Vector3 m_lastPos,m_newPos;
    private bool m_onTheFloor;

    private Rigidbody m_rigidBody;

    
    void Awake()
    {
        Application.targetFrameRate = 60;
    }
    void Start()
    {
        if(Application.targetFrameRate!=60)
            Application.targetFrameRate = 60;
        m_onTheFloor = false;

        m_rigidBody = this.gameObject.GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        m_forward = this.transform.forward; //z axis
        m_right = this.transform.right;
        m_up = this.transform.up;
        ProcessInput();
    }

    private void ProcessInput()
    {
        //If not on the floor, then can't move the body, but can rotate with the mouse view
        if(!m_onTheFloor)
        {
            this.transform.eulerAngles = m_cm.GetYRot();
            return;
        }

        if(Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) )
        {
            this.transform.position = this.transform.position + m_forward*m_speed*Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
        {
            this.transform.position = this.transform.position - m_forward*m_speed*Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            this.transform.position = this.transform.position - m_right*m_speed*Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            this.transform.position = this.transform.position + m_right*m_speed*Time.deltaTime;
        }

        float jumpAxis = Input.GetAxis("Jump");
        m_rigidBody.AddForce(m_up*jumpAxis*Time.deltaTime*m_jumpForce);

        //Rotate body along with view
        this.transform.eulerAngles = m_cm.GetYRot();
        
    }

    private void OnCollisionStay(Collision other) {
        
        m_lastPos = this.transform.position;

        //Check if the rigidbody is touching the floor
        if( other.gameObject.tag == "Floor")
        {
            m_onTheFloor = true;
        }
    }

    private void OnCollisionExit(Collision other) {

        //Check if the rigidbody stopped touching the floor
        if(other.gameObject.tag == "Floor")
        {
            m_onTheFloor = false;
        }
    }

    void OnCollisionEnter(Collision other) {
        m_newPos = this.transform.position;
        Debug.Log("Jump Distance: " + Vector3.Distance(m_newPos,m_lastPos) );
    }

}
