using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    public static ShipMovement shipMovement;
    public Transform bankingShip;
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
			bankingShip.localRotation = Quaternion.Slerp(bankingShip.localRotation, Quaternion.Euler(-20, 0, 0), (Time.fixedDeltaTime * bankSpeed));
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
			bankingShip.localRotation = Quaternion.Slerp(bankingShip.localRotation, Quaternion.Euler(0, -45, 0), (Time.fixedDeltaTime * bankSpeed));
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
			bankingShip.localRotation = Quaternion.Slerp(bankingShip.localRotation, Quaternion.Euler(0, 45, 0), (Time.fixedDeltaTime * bankSpeed));
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
			bankingShip.localRotation = Quaternion.Slerp(bankingShip.localRotation, Quaternion.Euler(10, 0, 0), (Time.fixedDeltaTime * bankSpeed));
        }
    }

    void FixedUpdate()
    {
        // normalize ship rotation
		bankingShip.localRotation = Quaternion.Slerp(bankingShip.localRotation, Quaternion.Euler(0, 0, 0), (Time.fixedDeltaTime * bankSpeed));
        //ship.localPosition = Vector3.zero;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
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
        Vector3 newDirection = Vector3.RotateTowards(bankingShip.forward, targetDirection, Time.fixedDeltaTime * speed, 0);
        bankingShip.rotation = Quaternion.LookRotation(newDirection);
    }

    void Start()
    {
        shipMovement = this;
    }
}