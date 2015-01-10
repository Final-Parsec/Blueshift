using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    /// <summary>
    /// Decelerates the ship and raises on y-axis. 
    /// </summary>
    void Ascend()
    {
        bool pastTopScreenEdge = Camera.main.WorldToScreenPoint(transform.position).y > Screen.height;

        transform.Translate(0, pastTopScreenEdge ? 0 : .5f, 0);
    }
    
    /// <summary>
    /// Steers ship to the left.
    /// </summary>
    void BankLeft()
    {
        bool pastLeftScreenEdge = Camera.main.WorldToScreenPoint(transform.position).x <= 0;

        transform.Translate(pastLeftScreenEdge ? 0 : -.5f, 0, 0);
    }

    /// <summary>
    /// Steers ship to the right.
    /// </summary>
    void BankRight()
    {
        bool pastRightScreenEdge = Camera.main.WorldToScreenPoint(transform.position).x >= Screen.width;

        transform.Translate(pastRightScreenEdge ? 0 : .5f, 0, 0);
    }

    /// <summary>
    /// Accelerates ship and lowers on y-axis.
    /// </summary>
    void Dive()
    {
        var groundLevel = transform.position.y < 1;

        // Do not let ship go below the ground.
        transform.Translate(0, groundLevel ? 0 : -.5f, 0);
    }

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
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
}