using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

namespace CompleteProject
{
    public class PlayerShooting : MonoBehaviour
    {
        public int damagePerShot = 20;                  // The damage inflicted by each bullet.
        public float timeBetweenBullets = 0.15f;        // The time between each shot.
        public float range = 100f;                      // The distance the gun can fire.
        public Material gunMatOne;
        public Material gunMatTwo;


        float timer;                                    // A timer to determine when to fire.
        Ray shootRay = new Ray();                       // A ray from the gun end forwards.
        RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
        RaycastHit[] shootHitsTwo;
        
        int shootableMask;                              // A layer mask so the raycast only hits things on the shootable layer.
        ParticleSystem gunParticles;                    // Reference to the particle system.
        LineRenderer gunLine;                           // Reference to the line renderer.
        AudioSource gunAudio;                           // Reference to the audio source.
        Light gunLight;                                 // Reference to the light component.
		public Light faceLight;								// Duh
        float effectsDisplayTime = 0.2f;                // The proportion of the timeBetweenBullets that the effects will display for.
        int weaponEquipped = 1;


        void Awake ()
        {
            // Create a layer mask for the Shootable layer.
            shootableMask = LayerMask.GetMask ("Shootable");

            // Set up the references.
            gunParticles = GetComponent<ParticleSystem> ();
            gunLine = GetComponent <LineRenderer> ();
            gunAudio = GetComponent<AudioSource> ();
            gunLight = GetComponent<Light> ();
			//faceLight = GetComponentInChildren<Light> ();
        }


        void Update ()
        {
            // Add the time since Update was last called to the timer.
            timer += Time.deltaTime;

            if (Input.GetButton("Weapon1"))
            {
                WeaponEquippedOne();
            }

            if (Input.GetButton("Weapon2"))
            {
                WeaponEquippedTwo();
            }

#if !MOBILE_INPUT
            // If the Fire1 button is being press and it's time to fire...
            if (Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
            {
                // ... shoot the gun.
                Shoot ();
            }
#else
            // If there is input on the shoot direction stick and it's time to fire...
            if ((CrossPlatformInputManager.GetAxisRaw("Mouse X") != 0 || CrossPlatformInputManager.GetAxisRaw("Mouse Y") != 0) && timer >= timeBetweenBullets)
            {
                // ... shoot the gun
                Shoot();
            }
#endif
            // If the timer has exceeded the proportion of timeBetweenBullets that the effects should be displayed for...
            if(timer >= timeBetweenBullets * effectsDisplayTime)
            {
                // ... disable the effects.
                DisableEffects ();
            }
        }


        public void DisableEffects ()
        {
            // Disable the line renderer and the light.
            gunLine.enabled = false;
			faceLight.enabled = false;
            gunLight.enabled = false;
        }


        void Shoot ()
        {



            if (weaponEquipped == 1)
            {

                // Reset the timer.
                timer = 0f;

                // Play the gun shot audioclip.
                gunAudio.Play();

                // Enable the lights.
                gunLight.enabled = true;
                faceLight.enabled = true;

                // Stop the particles from playing if they were, then start the particles.
                gunParticles.Stop();
                gunParticles.Play();

                // Enable the line renderer and set it's first position to be the end of the gun.
                gunLine.enabled = true;
                gunLine.SetPosition(0, transform.position);

                // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
                shootRay.origin = transform.position;
                shootRay.direction = transform.forward;

                // Perform the raycast against gameobjects on the shootable layer and if it hits something...

                if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
                {
                    // Try and find an EnemyHealth script on the gameobject hit.
                    EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();

                    // If the EnemyHealth component exist...
                    if (enemyHealth != null)
                    {
                        // ... the enemy should take damage.
                        enemyHealth.TakeDamage(damagePerShot, shootHit.point);
                    }

                    // Set the second position of the line renderer to the point the raycast hit.
                    gunLine.SetPosition(1, shootHit.point);
                }
                // If the raycast didn't hit anything on the shootable layer...
                else
                {
                    // ... set the second position of the line renderer to the fullest extent of the gun's range.
                    gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
                }
            }


            if (weaponEquipped == 2)
            {

                // Reset the timer.
                timer = 0f;

                // Play the gun shot audioclip.
                gunAudio.Play();

                // Enable the lights.
                gunLight.enabled = true;
                faceLight.enabled = true;

                // Stop the particles from playing if they were, then start the particles.
                gunParticles.Stop();
                gunParticles.Play();

                // Enable the line renderer and set it's first position to be the end of the gun.
                gunLine.enabled = true;
                gunLine.SetPosition(0, transform.position);

                // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
                shootRay.origin = transform.position;
                shootRay.direction = transform.forward;

                shootHitsTwo = Physics.SphereCastAll(shootRay.origin, 1f, shootRay.direction, range, shootableMask);
                int enemiesHit = 0;
                for (int i = 0; i < shootHitsTwo.Length; i++)
                {
                    RaycastHit shootHitTwo = shootHitsTwo[i];

                    // Try and find an EnemyHealth script on the gameobject hit.
                    EnemyHealth enemyHealth = shootHitTwo.collider.GetComponent<EnemyHealth>();

                    // If the EnemyHealth component exist...
                    if (enemyHealth != null)
                    {
                        enemiesHit++;
                        Debug.Log(enemiesHit);
                        // ... the enemy should take damage.
                        enemyHealth.TakeDamage(damagePerShot*(Mathf.Clamp(enemiesHit, 0,5)), shootHitTwo.point);
                    }

                    // Set the second position of the line renderer to the point the raycast hit.
                    gunLine.SetPosition(1, shootHitTwo.point);

                }
            // If the raycast didn't hit anything on the shootable layer...
                if(shootHitsTwo == null)
                {
                    // ... set the second position of the line renderer to the fullest extent of the gun's range.
                    gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
                }
                
            }
        }

        void WeaponEquippedOne()
        {
            weaponEquipped = 1;
            damagePerShot = 20;                  // The damage inflicted by each bullet.
            timeBetweenBullets = 0.15f;        // The time between each shot.
            range = 100f;
            gunLight.color = Color.yellow;
            gunLine.material = gunMatOne;

        }

        void WeaponEquippedTwo()
        {
            weaponEquipped = 2;
            damagePerShot = 20;                  // The damage inflicted by each bullet.
            timeBetweenBullets = 1.5f;        // The time between each shot.
            range = 1000f;                      // The distance the gun can fire.
            gunLight.color = Color.magenta;
            gunLine.material = gunMatTwo;
        }

    }
}