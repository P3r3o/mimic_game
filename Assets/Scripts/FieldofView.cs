using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldofView : MonoBehaviour
{
    private Mesh mesh;
    private float fov;
    private Vector3 origin;
    private float startingAngle;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }
    private void Update(){
        fov = 90f;
        origin = Vector3.zero;
        int rayCount = 50;
        float angle = startingAngle;
        float angleIncrease = fov/rayCount;
        float viewDistance = 50f;

        Vector3[] vertices = new Vector3[rayCount + 2];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount*3];

        vertices[0] = origin;
        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i=0; i <= rayCount; i++) {
            Vector3 vertex;
            RaycastHit2D RaycastHit2D = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance);
            if (RaycastHit2D.collider == null) {
                // no hit
                vertex = origin + GetVectorFromAngle(angle) * viewDistance;
            } else
            {
                vertex = RaycastHit2D.point;
            }
            vertices[vertexIndex] = vertex;

            if (i > 0) {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3; 
            }
            vertexIndex++;
            angle -= angleIncrease;
        }
        
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    private static Vector3 GetVectorFromAngle(float angle) {
        float angleRad = angle * (Mathf.PI/180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
    
    private static float getAngleFromVectorFloat(Vector3 dir) {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n+= 360;

        return n;
    }

    public void SetOrgin(Vector3 origin) {
        this.origin = origin;
    }
    public void SetAimDirection(Vector3 aimDirection) {
        startingAngle = getAngleFromVectorFloat(aimDirection);
    }
}
