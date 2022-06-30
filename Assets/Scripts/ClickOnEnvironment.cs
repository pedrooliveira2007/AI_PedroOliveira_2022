using UnityEngine;
/// <summary>
/// class responsible for read the mouse click and generate the explosions
/// </summary>
public class ClickOnEnvironment : MonoBehaviour
{
    private Camera _camera;
    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    private LayerMask layer;

    void Start()
    {
        _camera = Camera.main;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //returns the cursor position to start the explosion using a raycast
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, layer))
            {
                //generates the explosion at the given position
                GenerateExplosion(hit.point);
            }
        }
    }
    /// <summary>
    /// instantiate the explosion
    /// </summary>
    /// <param name="point">initial position</param>

    void GenerateExplosion(Vector3 point)
    {
        Instantiate(explosion, point, new Quaternion());
    }
}
