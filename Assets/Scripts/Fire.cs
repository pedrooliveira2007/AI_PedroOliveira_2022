using UnityEngine;
/// <summary>
/// Class responsible for the fire behaviour
/// </summary>
public class Fire : MonoBehaviour
{/// <summary>
/// Fire Expansion rate.
/// </summary>
    [SerializeField]
    private float ExpansionScale = 1f;


    // Update is called once per frame
    void FixedUpdate()
    {
        ExpandFire();

    }

    /// <summary>
    /// start the fire at the given position with the given scale
    /// </summary>
    /// <param name="_position">center position</param>
    /// <param name="_scale">initial fire size</param>
    internal void StartFire(Vector3 _position, Vector3 _scale)
    {
        gameObject.transform.position = _position;
        gameObject.transform.localScale = new Vector3(_scale.x, 0.33f, _scale.y);
    }
    /// <summary>
    /// spansion methos used in fire propagation
    /// </summary>
    internal void ExpandFire()
    {
        gameObject.transform.localScale += (ExpansionScale * new Vector3(1f, 0, 1f) * Time.fixedDeltaTime);
    }
}