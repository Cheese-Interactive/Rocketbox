
using System.Collections;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {
    private SpriteRenderer sprite;
    private Color visible = Color.yellow;
    private Color invisible = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, 0);
    [SerializeField] private float fadeInTime = 0;
    [SerializeField] private float fadeOutTime = 0;
    private bool hasDoneWhateverTheHeckThisThingIsSupposedToDoAlreadyLongestBoolNameEver;

    // Start is called before the first frame update
    void Start() {
        sprite = GetComponent<SpriteRenderer>();
        sprite.color = invisible;


    }

    // Update is called once per frame
    void Update() {
        if (!hasDoneWhateverTheHeckThisThingIsSupposedToDoAlreadyLongestBoolNameEver) {
            StartCoroutine(lerpColor(invisible, visible, fadeInTime));
            hasDoneWhateverTheHeckThisThingIsSupposedToDoAlreadyLongestBoolNameEver = true;
        }

    }

    private IEnumerator lerpColor(Color start, Color end, float duration) {
        float t = 0;
        sprite.color = start;
        while (t < duration) {
            sprite.color = Color.Lerp(start, end, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        sprite.color = end;
        StartCoroutine(lerpColor(visible, invisible, fadeOutTime));
        StartCoroutine(queueForDeletion(fadeOutTime));
    }

    private IEnumerator queueForDeletion(float wait) {
        yield return new WaitForSeconds(wait);
        Destroy(gameObject);
    }
}
