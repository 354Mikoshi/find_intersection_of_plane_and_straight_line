using UnityEngine;

public class IntersectionController : MonoBehaviour
{
    private FindCoordinateOfIntersection f;
    public GameObject camera;
    
    private void Start() {
        f = camera.GetComponent<FindCoordinateOfIntersection>();
    }

    private void Update() {
        gameObject.transform.position = f.Intersection;
    }
}
