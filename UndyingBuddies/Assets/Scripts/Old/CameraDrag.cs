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

    private float negX;
    private float posX;
    private float negZ;
    private float posZ;


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
        Vector3 keymove = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, 0, Input.GetAxis("Vertical") * moveSpeed);

        if (this.transform.position.x < negX)
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
            transform.Translate(keymove, Space.World);
        }
        
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
