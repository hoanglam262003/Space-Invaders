using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Projectile laserPrefab;
    public float speed = 5.0f;
    private bool laserActive;

    private void Update()
    {
        var keyboard = Keyboard.current;

        if (keyboard == null) return;

        if (keyboard.leftArrowKey.isPressed || keyboard.aKey.isPressed)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        else if (keyboard.rightArrowKey.isPressed || keyboard.dKey.isPressed)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }

        if (keyboard.spaceKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (!laserActive)
        {
            Projectile projectile = Instantiate(this.laserPrefab, this.transform.position, Quaternion.identity);
            projectile.destroyed += LaserDestroy;
            laserActive = true;
        }
    }

    private void LaserDestroy()
    {
        laserActive = false;
    }
}
