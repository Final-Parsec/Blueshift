using UnityEngine;

public class ShipMovement : MonoBehaviour 
{
    /// <summary>
    /// Decelerates the ship and raises on y-axis. 
    /// </summary>
    void Ascend()
    {
        transform.Translate(0, .5f, .25f);
    }
    
    /// <summary>
    /// Steers ship to the left.
    /// </summary>
    void BankLeft()
    {
        transform.Translate(-.5f, 0, 0);
    }

    /// <summary>
    /// Steers ship to the right.
    /// </summary>
    void BankRight()
    {
        transform.Translate(.5f, 0, 0);
    }

    /// <summary>
    /// Accelerates ship and lowers on y-axis.
    /// </summary>
    void Dive()
    {
        var groundLevel = transform.position.y < 3;

        // Do not let ship go below 3.
        transform.Translate(0, groundLevel ? 0 : -.5f, groundLevel ? 1 : 1.5f);
    }

    /// <summary>
    /// Makes the ship go forward.
    /// </summary>
    void GoForward()
    {
        transform.Translate(0, 0, 1);
    }

    /// <summary>
    /// Use this for initialization
    /// </summary>
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.DownArrow) == true ||
            Input.acceleration.y > -.25)
        {
            Dive();
        }
        else if (Input.GetKey(KeyCode.UpArrow) == true ||
            Input.acceleration.y < -.85)
        {
            Ascend();
        }
        else
        {
            GoForward();
        }

        if (Input.GetKey(KeyCode.LeftArrow) == true ||
            Input.acceleration.x < -.25)
        {
            BankLeft();
        }
        else if (Input.GetKey(KeyCode.RightArrow) == true ||
            Input.acceleration.x > .25)
        {
            BankRight();
        }
	}
}