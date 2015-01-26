using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    public static ShipMovement shipMovement;
    private Transform ship;
    public float bankSpeed;
    private Quaternion worldRotation;

    /// <summary>
    /// Decelerates the ship and raises on y-axis. 
    /// </summary>
    void Ascend()
    {
        bool pastTopScreenEdge = Camera.main.WorldToScreenPoint(transform.position).y > Screen.height;
        if (!pastTopScreenEdge)
        {
            transform.Translate(0, .5f, 0);
            ship.rotation = Quaternion.Slerp(ship.rotation, Quaternion.Euler(worldRotation.eulerAngles.x - 45, worldRotation.eulerAngles.y, worldRotation.eulerAngles.z), (Time.fixedDeltaTime * bankSpeed));
        }
    }
    
    /// <summary>
    /// Steers ship to the left.
    /// </summary>
    void BankLeft()
    {
        bool pastLeftScreenEdge = Camera.main.WorldToScreenPoint(transform.position).x <= 0;
        if (!pastLeftScreenEdge)
        {
            transform.Translate(-.5f, 0, 0);
            ship.rotation = Quaternion.Slerp(ship.rotation, Quaternion.Euler(worldRotation.eulerAngles.x + 45, worldRotation.eulerAngles.y - 90, worldRotation.eulerAngles.z + 90), (Time.fixedDeltaTime * bankSpeed));
        }
    }

    /// <summary>
    /// Steers ship to the right.
    /// </summary>
    void BankRight()
    {
        bool pastRightScreenEdge = Camera.main.WorldToScreenPoint(transform.position).x >= Screen.width;
        if (!pastRightScreenEdge)
        {
            transform.Translate(.5f, 0, 0);
            ship.rotation = Quaternion.Slerp(ship.rotation, Quaternion.Euler(worldRotation.eulerAngles.x + 45, worldRotation.eulerAngles.y + 90, worldRotation.eulerAngles.z - 90), (Time.fixedDeltaTime * bankSpeed));
        }
    }

    /// <summary>
    /// Accelerates ship and lowers on y-axis.
    /// </summary>
    void Dive()
    {
        bool pastBottomScreenEdge = Camera.main.WorldToScreenPoint(transform.position).y <= 0;

        if (!pastBottomScreenEdge)
        {
            transform.Translate(0, -.5f, 0);
            ship.rotation = Quaternion.Slerp(ship.rotation, Quaternion.Euler(worldRotation.eulerAngles.x + 10, worldRotation.eulerAngles.y, worldRotation.eulerAngles.z), (Time.fixedDeltaTime * bankSpeed));
        }
    }

    void FixedUpdate()
    {
        // find the proper worldRotation for this update
        Quaternion localRotation = ship.localRotation;
        ship.localRotation = Quaternion.Euler(new Vector3(270,0,0));
        worldRotation = ship.rotation;
        ship.localRotation = localRotation;

        
        // normalize ship rotation
        ship.rotation = Quaternion.Slerp(ship.rotation, worldRotation, (Time.fixedDeltaTime * bankSpeed));
        //ship.localPosition = Vector3.zero;
        rigidbody.velocity = Vector3.zero;
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) ||
            (Input.acceleration.y > -.25 && Input.acceleration.y != 0))
        {
            Dive();
        } else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) ||
            Input.acceleration.y < -.85)
        {
            Ascend();
        }
        
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) ||
            Input.acceleration.x < -.25)
        {
            BankLeft();
        } else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) ||
            Input.acceleration.x > .25)
        {
            BankRight();
        }


    }

    void RotateShip(float speed, Vector3 targetDirection)
    {
        Vector3 newDirection = Vector3.RotateTowards(ship.forward, targetDirection, Time.fixedDeltaTime * speed, 0);
        ship.rotation = Quaternion.LookRotation(newDirection);
    }

    void Start()
    {
        shipMovement = this;
        ship = transform.Find("ship");
    }
}