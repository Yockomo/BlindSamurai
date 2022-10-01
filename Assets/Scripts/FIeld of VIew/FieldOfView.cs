using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private float fieldOfView = 360;
    [SerializeField] private int rayCount = 4;
    
    private Mesh fieldOfViewMesh;

    private Vector3[] viewVertices = new Vector3[3];
    private Vector2[] viewUv = new Vector2[3];
    private int[] viewTriangles = new int[3];
    
    private void Start()
    {
        fieldOfViewMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = fieldOfViewMesh;
        
        Whatever();
        //
        // ConfigureMesh();
        
        fieldOfViewMesh.vertices = viewVertices;
        fieldOfViewMesh.uv = viewUv;
        fieldOfViewMesh.triangles = viewTriangles;
    }

    private void Whatever()
    {
        float angle = 0f;
        float angleIncrease = fieldOfView / rayCount;

        var verticesAndUvsCount = rayCount + 2;

        viewVertices = new Vector3[verticesAndUvsCount];
        viewUv = new Vector2[verticesAndUvsCount];
        viewTriangles = new int[rayCount * 3];

        var origin = Vector3.zero;
        viewVertices[0] = origin;
        
        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i < rayCount; i++)
        {
            var vertex = origin + GetVectorFromAngle(angle) * radius;
            viewVertices[i] = vertex;
            
            if (i > 0)
            {
                viewTriangles[triangleIndex] = 0;
                viewTriangles[triangleIndex + 1] = vertexIndex - 1;
                viewTriangles[triangleIndex + 2] = vertexIndex;
                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }

    }

    private Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    private void ConfigureMesh()
    {
        viewVertices[0] = new Vector3(0, 0);
        viewVertices[1] = new Vector3(0, radius);
        viewVertices[2] = new Vector3(radius, radius);

        viewTriangles[0] = 0;
        viewTriangles[1] = 1;
        viewTriangles[2] = 2;
    }
}
