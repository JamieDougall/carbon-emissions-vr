using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonGun : MonoBehaviour
{
    [SerializeField] GunMode mode = GunMode.Laser;
    [SerializeField] bool shooting = false;
    [SerializeField] Transform barrelAxis;
    [SerializeField] float laserMaxDistance = 10.0f;
    [SerializeField] LayerMask laserLayerMask;
    [SerializeField] BalloonController spawner;
    [SerializeField] LineRenderer laserLine;
    [SerializeField] Transform hitMarker;
    [SerializeField] ParticleSystem hitParticles;
    [SerializeField] AudioSource laserAudio;
    [SerializeField] AudioSource burnAudio;
    void Update()
    {
        if (shooting)
        {
            RaycastHit hit;
            if (Physics.Raycast(barrelAxis.position, barrelAxis.forward, out hit, laserMaxDistance, laserLayerMask))
            {
                hitMarker.position = hit.point;
                laserLine.SetPosition(1, laserLine.transform.InverseTransformPoint(hit.point));
                hitParticles.Play();
                if (!burnAudio.isPlaying)
                {
                    burnAudio.Play();
                }
                Balloon balloon = hit.transform.GetComponent<Balloon>();
                if (balloon != null)
                {
                    balloon.DestroyBalloon();
                }
            }
            else
            {
                hitParticles.Stop();
                if (burnAudio.isPlaying)
                {
                    burnAudio.Stop();
                }
                laserLine.SetPosition(1, Vector3.forward * laserMaxDistance);
            }
        }
    }

    public void PullTrigger()
    {
        switch (mode)
        {
            case GunMode.Laser:
                shooting = true;
                laserLine.enabled = true;
                laserAudio.Play();
                burnAudio.Play();
                break;
            case GunMode.Spawner:
                spawner.SpawnBalloons(30);
                break;
            default:
                break;
        }
    }

    public void ReleaseTrigger()
    {
        switch (mode)
        {
            case GunMode.Laser:
                shooting = false;
                laserLine.enabled = false;
                hitParticles.Stop();
                laserAudio.Stop();
                burnAudio.Stop();
                break;
            case GunMode.Spawner:
                spawner.SpawnActive = false;
                break;
            default:
                break;
        }
    }

    [System.Serializable]
    public enum GunMode
    {
        Laser,
        Spawner
    }
}
