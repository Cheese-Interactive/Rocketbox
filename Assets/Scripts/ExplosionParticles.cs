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
    ParticleSystem.MinMaxCurve emissionRate;

    void Start() {
    }

    private IEnumerator playAnim(float duration) {
        particles.Play();
        yield return new WaitForSeconds(duration * 10);
        particles.Stop();
        print("i have exploded");
        Destroy(gameObject);
    }

    public void initialize(float newRadius) {
        particles = GetComponent<ParticleSystem>();
        main = particles.main;
        shape = particles.shape;
        emission = particles.emission;
        //emissionRate = emission.rateOverTime;

        emissionRate.mode = ParticleSystemCurveMode.TwoConstants;
        emission.rate = new ParticleSystem.MinMaxCurve(minEmission, maxEmission);
        main.maxParticles = maxParticles;
        main.simulationSpeed = simSpeed;
        shape.scale = new Vector3(newRadius, newRadius, 1);

        StartCoroutine(playAnim(main.duration));
    }
}
