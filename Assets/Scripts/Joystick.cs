using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour
{
    private Vector2 buttonCenter;
    private static Joystick instance;
    private int pointerId = -1;

    void Start()
    {
        Joystick.instance = this;
        
        Debug.Log(Input.multiTouchEnabled);

        if (!Application.isMobilePlatform)
        {
            this.gameObject.SetActive(false);
        }
    }

    public static Joystick Instance
    {
        get
        {
            return Joystick.instance;
        }
    }

    public bool IsBeingAccessed
    {
        get
        {
            return this.pointerId != -1;
        }
    }

    public Touch Touch
    {
        get
        {
            return Input.touches.FirstOrDefault(t => t.fingerId == this.pointerId);
        }
    }
    
    internal Vector2 ButtonCenter
    {
        get
        {
            if (this.buttonCenter == Vector2.zero)
            {
                System.Diagnostics.Debug.Assert(this.transform != null, "transform != null");
                this.buttonCenter = this.transform.position;
            }

            return this.buttonCenter;
        }
    }

    void Update () {
        ////if (this.Touch.phase == TouchPhase.Ended || this.Touch.phase == TouchPhase.Canceled)
        ////{
        ////    return;
        ////}
        
        var touchPosition = this.Touch.position;
        if (this.pointerId == -1 || touchPosition.x < 0 || touchPosition.y < 0)
        {
            return;
        }

        //now check which direction to move
        if (touchPosition.x - this.ButtonCenter.x > 75)
        {
            ShipMovement.shipMovement.BankRight();
        }
        else if (touchPosition.x - this.ButtonCenter.x < -75)
        {
            ShipMovement.shipMovement.BankLeft();
        }
        
        if (touchPosition.y - this.ButtonCenter.y > 75)
        {
            ShipMovement.shipMovement.Ascend();
        }
        else if (touchPosition.y - this.ButtonCenter.y < -75)
        {
            ShipMovement.shipMovement.Dive();
        }
    }

	/// <summary>
	///     Called when the pointer is dragged.
	/// </summary>
	public void OnBeginDrag(BaseEventData eventData)
    {
        var pointerEventData = eventData as PointerEventData;

		this.pointerId = pointerEventData.pointerId;
    }

	/// <summary>
	///     Called when the pointer stopped being dragged.
	/// </summary>  
	public void OnEndDrag()
    {
	    this.pointerId = -1;
    }
}
