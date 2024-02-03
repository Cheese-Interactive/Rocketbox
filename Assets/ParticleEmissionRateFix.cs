using UnityEngine;

public class ParticleEmissionRateFix : MonoBehaviour {
    private ParticleSystem.EmissionModule emission;
    ParticleSystem.MinMaxCurve emissionRate;
    [SerializeField] private float minRate, maxRate;
    // Start is called before the first frame update
    void Start() {
        GetComponent<ParticleSystem>().Pause();
        emission = GetComponent<ParticleSystem>().emission;
        emissionRate = emission.rateOverTime;
        emissionRate.mode = ParticleSystemCurveMode.TwoConstants;
        emission.rate = new ParticleSystem.MinMaxCurve(minRate, maxRate);
        GetComponent<ParticleSystem>().Play();
    }
}
