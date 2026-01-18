using System.Collections;
using UnityEngine;

public class MultiLaserSpawner : MonoBehaviour
{
    public enum LaserDirection
    {
        Right,
        Left,
        Up,
        Down,
        UpRight,
        UpLeft,
        DownRight,
        DownLeft
    }

    [SerializeField] private bool shootImmediately;
    
    [SerializeField] private bool shootRight;
    [SerializeField] private bool shootLeft;
    [SerializeField] private bool shootUp;
    [SerializeField] private bool shootDown;
    [SerializeField] private bool shootUpRight;
    [SerializeField] private bool shootUpLeft;
    [SerializeField] private bool shootDownRight;
    [SerializeField] private bool shootDownLeft;
    
    [SerializeField] private GameObject laserRight;
    [SerializeField] private GameObject laserLeft;
    [SerializeField] private GameObject laserUp;
    [SerializeField] private GameObject laserDown;
    [SerializeField] private GameObject laserUpRight;
    [SerializeField] private GameObject laserUpLeft;
    [SerializeField] private GameObject laserDownRight;
    [SerializeField] private GameObject laserDownLeft;

    private void Awake()
    {
        if (shootImmediately)
        {
            StartShooting();
        }
    }
    

    public void ActivateLaser(LaserDirection dir)
    {
        GameObject laser = GetLaser(dir);
        if (laser != null)
            laser.SetActive(true);
    }

    public void DeactivateLaser(LaserDirection dir)
    {
        GameObject laser = GetLaser(dir);
        if (laser != null)
            laser.SetActive(false);
    }
    

    public void StartShooting()
    {
        if (shootRight) ActivateLaser(LaserDirection.Right);
        if (shootLeft) ActivateLaser(LaserDirection.Left);
        if (shootUp) ActivateLaser(LaserDirection.Up);
        if (shootDown) ActivateLaser(LaserDirection.Down);
        if (shootUpRight) ActivateLaser(LaserDirection.UpRight);
        if (shootUpLeft) ActivateLaser(LaserDirection.UpLeft);
        if (shootDownRight) ActivateLaser(LaserDirection.DownRight);
        if (shootDownLeft) ActivateLaser(LaserDirection.DownLeft);
    }

    public void StopShooting()
    {
        if (shootRight) DeactivateLaser(LaserDirection.Right);
        if (shootLeft) DeactivateLaser(LaserDirection.Left);
        if (shootUp) DeactivateLaser(LaserDirection.Up);
        if (shootDown) DeactivateLaser(LaserDirection.Down);
        if (shootUpRight) DeactivateLaser(LaserDirection.UpRight);
        if (shootUpLeft) DeactivateLaser(LaserDirection.UpLeft);
        if (shootDownRight) DeactivateLaser(LaserDirection.DownRight);
        if (shootDownLeft) DeactivateLaser(LaserDirection.DownLeft);
    }

    public void ShootForTime(float duration)
    {
        StartCoroutine(ShootForTimeCoroutine(duration));
    }

    private IEnumerator ShootForTimeCoroutine(float duration)
    {
        StartShooting();
        yield return new WaitForSeconds(duration);
        StopShooting();
    }

    public void ShootForTimeInDir(float duration, LaserDirection dir)
    {
        StartCoroutine(ShootForTimeInDirCoroutine(duration, dir));
    }

    private IEnumerator ShootForTimeInDirCoroutine(float duration, LaserDirection dir)
    {
        ActivateLaser(dir);
        yield return new WaitForSeconds(duration);
        DeactivateLaser(dir);
    }

    private GameObject GetLaser(LaserDirection dir)
    {
        return dir switch
        {
            LaserDirection.Right => laserRight,
            LaserDirection.Left => laserLeft,
            LaserDirection.Up => laserUp,
            LaserDirection.Down => laserDown,
            LaserDirection.UpRight => laserUpRight,
            LaserDirection.UpLeft => laserUpLeft,
            LaserDirection.DownRight => laserDownRight,
            LaserDirection.DownLeft => laserDownLeft,
            _ => null
        };
    }
}
