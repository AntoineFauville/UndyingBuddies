using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonExplosion : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettings;

    [SerializeField] private List<GameObject> allPriestTouched = new List<GameObject>();
    [SerializeField] private List<GameObject> allPriestThatArePoisoned = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Collider[] HitCollider = Physics.OverlapSphere(this.transform.position, _gameSettings.poisonExplosionSpell.Range);

        for (int i = 0; i < HitCollider.Length; i++)
        {
            if (HitCollider[i].GetComponent<AIPriest>() != null && !HitCollider[i].GetComponent<AIPriest>().AmIBuilding)
            {
                allPriestTouched.Add(HitCollider[i].gameObject);

                int rand = Random.Range(0, 100);
                if (rand > 50)
                {
                    allPriestThatArePoisoned.Add(HitCollider[i].gameObject);
                }
            }
        }

        for (int i = 0; i < allPriestThatArePoisoned.Count; i++)
        {
            if (allPriestThatArePoisoned[i].GetComponent<AIPriest>().AmUnderEffect == false)
            {
                allPriestThatArePoisoned[i].GetComponent<AIPriest>().AmUnderEffect = true;
                allPriestThatArePoisoned[i].GetComponent<AIPriest>().currentAiPriestEffects = AiPriestEffects.Poisoned;
            }
        }

        for (int i = 0; i < allPriestTouched.Count; i++)
        {
            allPriestTouched[i].gameObject.GetComponent<AIStatController>().TakeDamage(AiStatus.Physical, _gameSettings.poisonExplosionSpell);
        }
    }
}
