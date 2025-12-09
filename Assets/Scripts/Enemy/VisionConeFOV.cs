using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VisionConeFOV : MonoBehaviour
{
    public float viewRadius = 5f;
    public float viewAngle = 60f;
    public int rayCount = 60;

    private Mesh mesh;
    private Vector3 origin;

    public LayerMask obstacleMask;

    void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void SetOrigin(Vector3 o)
    {
        origin = o;
    }

    void LateUpdate()
    {
        Draw();
    }

    void Draw()
    {
        float half = viewAngle * 0.5f;
        float step = viewAngle / rayCount;

        Vector3[] verts = new Vector3[rayCount + 2];
        int[] tris = new int[rayCount * 3];

        verts[0] = Vector3.zero;

        float angle = -half;

        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 dir = DirXY(angle);  

            Vector3 point;

            if (Physics.Raycast(origin, dir, out var hit, viewRadius, obstacleMask))
                point = hit.point;
            else
                point = origin + dir * viewRadius;

            verts[i + 1] = transform.InverseTransformPoint(point);

            angle += step;
        }

        mesh.Clear();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
    }


    // Важно: вычисляем направление в плоскости XY
    Vector3 DirXY(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;

        return new Vector3(
            Mathf.Cos(rad),   // X
            Mathf.Sin(rad),   // Y
            0f              // Z всегда ноль
        );
    }
}
