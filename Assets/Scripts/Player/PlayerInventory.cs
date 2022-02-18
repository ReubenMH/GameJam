using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] Transform gunHoldPoint;
    List<GunPickup> availableGuns;
    Gun heldGun;

    private void Start()
    {
        availableGuns = new List<GunPickup>();
    }

    public void AttemptPickup()
    {
        if(availableGuns.Count > 0)
        {
            Pickup(availableGuns[0]);
        }
    }

    public void AttemptShoot()
    {
        Debug.Log("Inventory attempt shoot");

        if(heldGun == null)
        {
            return;
        }

        heldGun.AttemptShoot();
    }

    public void SetGunAngle(Vector3 angle)
    {
        if (heldGun == null)
            return;

        heldGun.transform.localEulerAngles = angle;
    }

    void DiscardHeldGun()
    {
        Destroy(heldGun);
        heldGun = null;

        UIControl.Instance.HideGunInfoCanvas();
    }

    void Pickup(GunPickup pickup)
    {
        availableGuns.Remove(pickup);

        if (heldGun != null)
            DiscardHeldGun();

        Destroy(pickup.parentGO.gameObject);

        heldGun = Instantiate(pickup.ContainedGun, gunHoldPoint);
        heldGun.Pickup();

        UIControl.Instance.SetPickupGun(false);
        UIControl.Instance.ShowGunInfoCanvas(heldGun);
    }

    private void OnTriggerEnter(Collider other)
    {
        GunPickup pickup = other.GetComponent<GunPickup>();
        if (pickup)
        {
            NewGunAvailableToPickup(pickup);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GunPickup pickup = other.GetComponent<GunPickup>();
        if (pickup)
        {
            GunCanNoLongerPickup(pickup);
        }
    }

    void NewGunAvailableToPickup(GunPickup pickup)
    {
        availableGuns.Add(pickup);
        RefreshPickupUI();
    }

    void GunCanNoLongerPickup(GunPickup pickup)
    {
        availableGuns.Remove(pickup);
        RefreshPickupUI();
    }

    void RefreshPickupUI()
    {
        UIControl.Instance.SetPickupGun(availableGuns.Count > 0);
    }
}
