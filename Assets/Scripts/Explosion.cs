using UnityEngine;
/// <summary>
/// class responsible for escalate the explosions
/// </summary>
public class Explosion : MonoBehaviour
{
    private float ExpansionScale = 10f;
    [SerializeField]
    private float _explosionLifeTime = 3f;

    private float _actualTime = 0f;
    [SerializeField]
    private Fire _fire;


    void Start()
    {
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_actualTime >= _explosionLifeTime)
        {
            //instantiate the fire after the explosion occurs
            Instantiate(_fire.gameObject, transform.position, new Quaternion());
            //start the fire spread
            _fire.StartFire(transform.position, transform.localScale);
            //destroy the explosion game object
            Destroy(this.gameObject);

        }
        else
        {
            //expand the explosion while the explosion time wasn't reached
            transform.localScale += (ExpansionScale * Vector3.one * Time.fixedDeltaTime);

            _actualTime += Time.fixedDeltaTime;
        }

    }
}



