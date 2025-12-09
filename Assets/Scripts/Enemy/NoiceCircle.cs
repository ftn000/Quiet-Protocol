using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class NoiseCircleMesh : MonoBehaviour
{
    public int segments = 64;
    public float radius = 1f;

    private Mesh mesh;

    void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void SetRadius(float r)
    {
        radius = r;
    }

    void LateUpdate()
    {
        Draw();
    }

    void Draw()
    {
        if (radius <= 0)
        {
            mesh.Clear();
            return;
        }

        Vector3[] verts = new Vector3[segments + 1];
        int[] tris = new int[segments * 3];

        verts[0] = Vector3.zero;

        float step = 360f / segments;

        for (int i = 0; i < segments; i++)
        {
            float angle = step * i;

            verts[i + 1] = DirXY(angle) * radius;

            if (i < segments - 1)
            {
                tris[i * 3 + 0] = 0;
                tris[i * 3 + 1] = i + 1;
                tris[i * 3 + 2] = i + 2;
            }
        }

        // замыкаем последний треугольник
        int last = (segments - 1) * 3;
        tris[last + 0] = 0;
        tris[last + 1] = segments;
        tris[last + 2] = 1;

        mesh.Clear();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
    }


    Vector3 DirXY(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;

        return new Vector3(
            Mathf.Cos(rad),
            Mathf.Sin(rad),
            0
        );
    }
}
