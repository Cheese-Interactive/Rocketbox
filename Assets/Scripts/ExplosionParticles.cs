using System.Collections;
using UnityEngine;

public class ExplosionParticles : MonoBehaviour {

    private ParticleSystem particles;
    [SerializeField] private float radius;
    [SerializeField] private float simSpeed;
    [SerializeField] private float maxEmission;
    [SerializeField] private float minEmission;
    [SerializeField] private int maxParticles;
    ParticleSystem.MainModule main;
    ParticleSystem.ShapeModule shape;
    ParticleSystem.EmissionModule emission;
    ParticleSystem.Burst burst;
    ParticleSystem.MinMaxCurve curve;

    void Start() {
    }

    private IEnumerator playAnim(float duration) {
        particles.Play();
        yield return new WaitForSeconds(duration * 100);
        particles.Stop();
        Destroy(gameObject);
    }

    public void initialize(float newRadius) {
        particles = GetComponent<ParticleSystem>();
        main = particles.main;
        shape = particles.shape;
        emission = particles.emission;
        burst = emission.GetBurst(0);
        curve = burst.count;

        curve.mode = ParticleSystemCurveMode.TwoConstants;
        curve.constantMax = maxEmission;
        curve.constantMin = maxEmission;
        main.maxParticles = maxParticles;
        main.simulationSpeed = simSpeed;
        shape.scale = new Vector3(newRadius, newRadius, 1);

        StartCoroutine(playAnim(main.duration));
    }
}
