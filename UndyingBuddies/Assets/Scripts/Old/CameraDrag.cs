using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    public float dragSpeed = 2;
    private Vector3 dragOrigin;

    public float moveSpeed = 0.1f;

    void Update()
    {
        Vector3 currentCamPos = Camera.main.transform.position;
        Vector3 keymove = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, 0, Input.GetAxis("Vertical") * moveSpeed);

        transform.Translate(keymove, Space.World);

        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(1)) return;
        
        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(pos.x * dragSpeed,0, pos.y * dragSpeed);

        transform.Translate(-move, Space.World);

       
    }


}
