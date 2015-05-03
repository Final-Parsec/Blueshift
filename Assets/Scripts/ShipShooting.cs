using UnityEngine;
using System;
using System.Collections.Generic;

public class ShipShooting : MonoBehaviour
{
	private static ShipShooting instance;
	public static ShipShooting Instance
	{
		get
		{
			if (instance == null)
			{
				throw new InvalidOperationException("Bruh?");
			}
			return instance;
		}
	}

    public TagsAndEnums.ProjectileType projectileType;
	public Transform[] muzzelPoints;

	[System.ComponentModel.DefaultValue(0)]
	private int weaponUpgradeLevel;
	public int WeaponUpgradeLevel 
	{
		get{return weaponUpgradeLevel;}
		set
		{
			weaponUpgradeLevel = value;
			if(weaponUpgradeLevel == 2)
				shootSpeed = .15f;
		}
	}

	private float shootSpeed = .2f;
	private float lastShootTime = 0;
    private AudioSource audioSource;
	private ShipHealth shipHealth;
	public GameObject pauseMenu;

    void Start()
    {
        audioSource = GetComponentInParent<AudioSource>();
		shipHealth = GetComponentInChildren<ShipHealth>();
		instance = this;
		this.pauseMenu = GameObject.Find("PauseMenu");
		this.pauseMenu.SetActive(false);
    }
    
    public void PauseToggle()
    {
    	this.pauseMenu.SetActive(!this.pauseMenu.activeSelf);
		if (this.pauseMenu.activeSelf)
		{
			Time.timeScale = 0f;
		}
		else
		{
			Time.timeScale = 1f;
		}
    }
    
    // Update is called once per frame
    void Update()
    {
		if (shipHealth.IsDying)
			return;
			
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			this.PauseToggle();
		}

		if (Input.GetMouseButton(0) && Time.time > lastShootTime + shootSpeed && Time.timeScale != 0f)
        {
			lastShootTime = Time.time;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity);
            RaycastHit mainHit = new RaycastHit();
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.tag == TagsAndEnums.enemy)
                {
                    mainHit = hit;
                    break;
                }
				if (hit.collider.tag == TagsAndEnums.terrain || hit.collider.tag == TagsAndEnums.shootingBox)
                    mainHit = hit;
            }

			List<Projectile> projList = new List<Projectile>();

			if (WeaponUpgradeLevel <= 0)
			{
				projList.Add(PrefabAccessor.GetProjectile(projectileType, transform.root.tag, muzzelPoints[0].position));
			}
			else if (WeaponUpgradeLevel == 1 || WeaponUpgradeLevel == 2)
			{
				projList.Add(PrefabAccessor.GetProjectile(projectileType, transform.root.tag, muzzelPoints[1].position));
				projList.Add(PrefabAccessor.GetProjectile(projectileType, transform.root.tag, muzzelPoints[2].position));
			}
			else if (WeaponUpgradeLevel >= 3)
			{
				//Todo: add a different projectile type to index 0
				projList.Add(PrefabAccessor.GetProjectile(projectileType, transform.root.tag, muzzelPoints[0].position));
				projList.Add(PrefabAccessor.GetProjectile(projectileType, transform.root.tag, muzzelPoints[1].position));
				projList.Add(PrefabAccessor.GetProjectile(projectileType, transform.root.tag, muzzelPoints[2].position));
			}

			foreach(Projectile proj in projList)
			{
				proj.transform.LookAt(mainHit.point);
				Vector3 aimVector = Vector3.Normalize(transform.position - mainHit.point);
				proj.Intercept(aimVector, CameraMovement.cameraMovement.speed);
			}

            PlayShootSound();
        }
    }

    void PlayShootSound()
    {
        audioSource.clip = PrefabAccessor.prefabAccessor.GetRandomeSound(PrefabAccessor.prefabAccessor.shootSounds);
        audioSource.Play();
    }
}