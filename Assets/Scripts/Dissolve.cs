using UnityEngine;

public class Dissolve : MonoBehaviour
{
    Material material;

    [SerializeField] bool isCloaking = false;
    [SerializeField] bool deCloaking = false;
    [SerializeField] bool cloaked = false;
    [SerializeField] float fade = 1f;
    void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && !cloaked)
        {
            //cloaked = !cloaked;
            isCloaking = true;
        }
        else if (Input.GetKeyDown(KeyCode.O) && cloaked)
        {
            deCloaking = true;
        }

        if (isCloaking)
        {
            fade -= Time.deltaTime;

            if (fade <= 0f)
            {
                fade = 0f;
                isCloaking = false;
                cloaked = true;
            }

            material.SetFloat("_Fade", fade);

            if (cloaked)
            {
                // disable collider
                this.gameObject.GetComponent<Collider2D>().enabled = false;
            }
        }

        if (deCloaking)
        {
            fade += Time.deltaTime;

            if (fade >= 1f)
            {
                fade = 1f;
                deCloaking = false;
                cloaked = false;
            }

            material.SetFloat("_Fade", fade);

            if (deCloaking)
            {
                this.gameObject.GetComponent<Collider2D>().enabled = true;
            }
        }
    }
}
