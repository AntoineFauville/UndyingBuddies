using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burnable : MonoBehaviour
{
    public bool DidIAlreadyBurn;

    [SerializeField] private GameObject BurningEffect;

    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private AudioClip AudioClip;

    // Start is called before the first frame update
    void Start()
    {
        BurningEffect.SetActive(false);
    }

    public void Burn()
    {
        if (!DidIAlreadyBurn)
        {
            DidIAlreadyBurn = true;

            if (BurningEffect != null)
            {
                BurningEffect.SetActive(true);
            }
            StartCoroutine(waitRandSecToCheckNeighbourgs());

            StartCoroutine(waitToTurnOffBurn());

            if (this.GetComponent<Resource>() != null)
            {
                this.GetComponent<Resource>().amountOfResourceAvailable -= Random.Range(1,3);
            }

            if (AudioSource != null && AudioClip != null)
            {
                AudioSource.PlayOneShot(AudioClip);
            }
        }
    }

    void CheckOnOtherBurnableNeighbourghs()
    {
        Collider[] HitColliderWithFlammes = Physics.OverlapSphere(this.transform.position, 1.5f);

        if (HitColliderWithFlammes.Length > 0) {
            for (int i = 0; i < HitColliderWithFlammes.Length; i++)
            {
                if (HitColliderWithFlammes[i].GetComponent<Burnable>() != null && !HitColliderWithFlammes[i].GetComponent<Burnable>().DidIAlreadyBurn)
                {
                    HitColliderWithFlammes[i].GetComponent<Burnable>().Burn();
                }

                if (HitColliderWithFlammes[i].GetComponent<AIStatController>() != null)
                {
                    HitColliderWithFlammes[i].GetComponent<AIStatController>().TakeDamage(AiStatus.Physical, Random.Range(4, 15));
                }
            }
        }

        //check if we are colliding with poison
        Collider[] HitCollider = Physics.OverlapSphere(this.transform.position, 2 +
            ((float)GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.poisonExplosionSpell.Range / 2)//we take the sphere of the fire spell and we add half of the explosion spell to see if the two circle collides
            );

        for (int i = 0; i < HitCollider.Length; i++)
        {
            if (HitCollider[i].GetComponent<PoisonExplosion>() != null && !HitCollider[i].GetComponent<PoisonExplosion>().alreadyCollidedWithFireExplosion)
            {
                HitCollider[i].GetComponent<PoisonExplosion>().CollideWithFireSpell();

                Debug.Log("I've hit " + HitCollider[i].name);
            }
        }
    }

    IEnumerator waitRandSecToCheckNeighbourgs()
    {
        yield return new WaitForSeconds(Random.Range(5, 10));

        CheckOnOtherBurnableNeighbourghs();

        if (this.GetComponent<Resource>() != null)
        {
            this.GetComponent<Resource>().amountOfResourceAvailable -= Random.Range(0, 1);
        }

        StartCoroutine(waitRandSecToCheckNeighbourgs());
    }

    IEnumerator waitToTurnOffBurn()
    {
        yield return new WaitForSeconds(Random.Range(10, 15));

        if (BurningEffect != null)
        {
            BurningEffect.SetActive(false);
        }
    }
}
