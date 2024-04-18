using UnityEngine;

public class GunRotation : MonoBehaviour
{
    public Sprite pistolSprite;
    public Sprite machineGunSprite;
    public Sprite shotgunSprite;

    private SpriteRenderer gunSpriteRenderer;
    private Gun gunScript;

    void Start()
    {
        gunSpriteRenderer = GetComponent<SpriteRenderer>();
        gunScript = GetComponentInParent<Gun>();
    }

    void Update()
    {
        //get the mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        //calculate the direction from the gun's position to the mouse position
        Vector3 direction = mousePosition - transform.position;

        //calculate the angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //rotate the gun to face the mouse cursor
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        //flip the gun sprite if it's angled beyond 90 degrees to the left or right
        if (angle > 90f || angle < -90f)
        {
            //flip the gun sprite horizontally
            transform.localScale = new Vector3(1f, -1f, 1f);
        }
        else
        {
            //restore the gun sprite's original scale
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        //update the gun sprite based on the current gun type
        UpdateGunSprite();
    }

    //update the gun sprite based on the current gun type
    private void UpdateGunSprite()
    {
        if (gunScript != null)
        {
            switch (gunScript.gunType)
            {
                case GunType.Pistol:
                    gunSpriteRenderer.sprite = pistolSprite;
                    break;
                case GunType.MachineGun:
                    gunSpriteRenderer.sprite = machineGunSprite;
                    break;
                case GunType.Shotgun:
                    gunSpriteRenderer.sprite = shotgunSprite;
                    break;
                default:
                    break;
            }
        }
    }
}
