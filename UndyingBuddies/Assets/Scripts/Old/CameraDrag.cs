using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    public float dragSpeed = 2;
    private Vector3 dragOrigin;

    public float moveSpeed = 0.1f;

    public float MaxNegativeValueX;
    public float MaxPositiveValueX;

    public float MaxNegativeValueZ;
    public float MaxPositiveValueZ;

    [SerializeField] private float negX;
    [SerializeField] private float posX;
    [SerializeField] private float negZ;
    [SerializeField] private float posZ;


    void Start()
    {
        negX = this.transform.position.x - MaxNegativeValueX;
        posX = this.transform.position.x + MaxPositiveValueX;
        negZ = this.transform.position.z - MaxNegativeValueZ;
        posZ = this.transform.position.z + MaxPositiveValueZ;
    }

    void Update()
    {
        Vector3 currentCamPos = Camera.main.transform.position;
        //Vector3 keymove = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, 0, Input.GetAxis("Vertical") * moveSpeed);

        //if (this.transform.position.x < negX)
        //{
        //    this.transform.position = new Vector3(negX, 17, this.transform.position.z);
        //}
        //else if (this.transform.position.x > posX)
        //{
        //    //this.transform.position =
        //    this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(posX-2, 17, this.transform.position.z), 0.4f);
        //}
        //else if (this.transform.position.z < negZ)
        //{
        //    this.transform.position = new Vector3(this.transform.position.x, 17, negZ);
        //}
        //else if (this.transform.position.z > posZ)
        //{
        //    this.transform.position = new Vector3(this.transform.position.x, 17, posZ);
        //}
        //else
        //{
        //    transform.Translate(keymove, Space.World);
        //}

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (this.transform.position.x <= negX && horizontal < 0)
        {
            horizontal = 0;
        }
        if (this.transform.position.x > posX && horizontal > 0)
        {
            horizontal = 0;
        }
        if (this.transform.position.z < negZ && vertical < 0)
        {
            vertical = 0;
        }
        if (this.transform.position.z > posZ && vertical > 0)
        {
            vertical = 0;
        }


        Vector3 keymove = new Vector3(horizontal * moveSpeed, 0, vertical * moveSpeed);
        transform.Translate(keymove, Space.World);

        /* drag */


        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(1)) return;
        
        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(pos.x * dragSpeed,0, pos.y * dragSpeed);

        if (this.transform.position.x < negX )
        {
            this.transform.position = new Vector3(negX, 20, this.transform.position.z);
        }
        else if (this.transform.position.x > posX)
        {
            this.transform.position = new Vector3(posX, 20, this.transform.position.z);
        }
        else if (this.transform.position.z < negZ)
        {
            this.transform.position = new Vector3(this.transform.position.x, 20, negZ);
        }
        else if (this.transform.position.z > posZ)
        {
            this.transform.position = new Vector3(this.transform.position.x, 20, posZ);
        }
        else
        {
            transform.Translate(-move, Space.World);
        }
    }
}
