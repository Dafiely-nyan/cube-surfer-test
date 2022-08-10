using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshCollider), 
    typeof(MeshRenderer))]
public class LevelGenerator : MonoBehaviour
{
    private List<Vector3> _points;

    [SerializeField]
    private float _segmentLength = 5f;

    private MeshFilter _meshFilter;
    private MeshCollider _meshCollider;

    public Vector3 LastPoint { get; private set; }
    public Vector3 StartPoint { get; } = new Vector3(0.3f, .6f, 0);

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshCollider = GetComponent<MeshCollider>();
        
        GeneratePath();
        GenerateMesh();
    }

    void GeneratePath()
    {
        float[] angles = new[] {30f, -30f, 45f, -45f};
        
        _points = new List<Vector3>();
        
        _points.Add(Vector3.zero);
        _points.Add(Vector3.right * _segmentLength);
        
        _points.Add(_points[1] + Quaternion.AngleAxis(angles[0], Vector3.up) * Vector3.right * _segmentLength);
        _points.Add(_points[2] + Quaternion.AngleAxis(angles[1], Vector3.up) * Vector3.right * _segmentLength);
        _points.Add(_points[3] + Quaternion.AngleAxis(angles[2], Vector3.up) * Vector3.right * _segmentLength);
        _points.Add(_points[4] + Quaternion.AngleAxis(angles[3], Vector3.up) * Vector3.right * _segmentLength);

        LastPoint = _points[_points.Count - 1] + Vector3.up * 0.75f;
    }

    void GenerateMesh()
    {
        Mesh mesh = new Mesh();
        
        Vector3[] verts = new Vector3[_points.Count * 4];
        int[] tris = new int[(4 + 8 * _points.Count) * 3];
        
        float height = .75f;
        float halfSide = height * Mathf.Sqrt(2) / 2;

        Vector3 nextVector = _points[1] - _points[0];
        Vector3 ortoToNext = Vector3.Cross(nextVector, Vector3.up).normalized * height;

        verts[0] = Quaternion.AngleAxis(45, nextVector) * ortoToNext;
        verts[1] = Quaternion.AngleAxis(45 + 90 * 1, nextVector) * ortoToNext;
        verts[2] = Quaternion.AngleAxis(45 + 90 * 2, nextVector) * ortoToNext;
        verts[3] = Quaternion.AngleAxis(45 + 90 * 3, nextVector) * ortoToNext;

        tris[0] = 0;
        tris[1] = 3;
        tris[2] = 1;

        tris[3] = 3;
        tris[4] = 2;
        tris[5] = 1;

        for (int i = 1; i < _points.Count; i++)
        {
            Vector3 crtDir = -(_points[i] - _points[i - 1]).normalized;

            if (i == _points.Count - 1)
            {
                Vector3 nextVector2 = (_points[i] - _points[i - 1]).normalized;
                Vector3 ortoToNext2 = Vector3.Cross(nextVector2, Vector3.up).normalized * height;

                verts[4 * i] = _points[i] + Quaternion.AngleAxis(45, nextVector2) * ortoToNext2;
                verts[4 * i + 1] = _points[i] + Quaternion.AngleAxis(45 + 90 * 1, nextVector2) * ortoToNext2;
                verts[4 * i + 2] = _points[i] + Quaternion.AngleAxis(45 + 90 * 2, nextVector2) * ortoToNext2;
                verts[4 * i + 3] = _points[i] + Quaternion.AngleAxis(45 + 90 * 3, nextVector2) * ortoToNext2;
            }
            else
            {
                Vector3 nextDir = (_points[i + 1] - _points[i]).normalized;
                
                float dirAngle = Vector3.SignedAngle(nextDir, crtDir, Vector3.up) / 2;
                float length = halfSide / Mathf.Sin(dirAngle * Mathf.Deg2Rad);
                
                Vector3 resDir = Quaternion.Euler(0, dirAngle, 0) * nextDir;

                verts[4 * i] = _points[i] - resDir * length - Vector3.up * halfSide;
                verts[4 * i + 3] = _points[i] - resDir * length + Vector3.up * halfSide;
                
                verts[4 * i + 1] = _points[i] + resDir * length - Vector3.up * halfSide;
                verts[4 * i + 2] = _points[i] + resDir * length + Vector3.up * halfSide;
            }

            int trisStart = 6 + 24 * (i - 1);
            // верхняя плоскость
            tris[trisStart] = 4 * i - 4;
            tris[trisStart + 1] = 4 * i - 3;
            tris[trisStart + 2] = 4 * i + 1;
            
            tris[trisStart + 3] = 4 * i + 1;
            tris[trisStart + 4] = 4 * i;
            tris[trisStart + 5] = 4 * i - 4;
            
            // нижняя плоскость
            tris[trisStart + 6] = 4 * i - 2;
            tris[trisStart + 7] = 4 * i - 1;
            tris[trisStart + 8] = 4 * i + 2;
            
            tris[trisStart + 9] = 4 * i + 2;
            tris[trisStart + 10] = 4 * i - 1;
            tris[trisStart + 11] = 4 * i + 3;
            
            // правая боковая плоскость
            tris[trisStart + 12] = 4 * i - 1;
            tris[trisStart + 13] = 4 * i - 4;
            tris[trisStart + 14] = 4 * i;
            
            tris[trisStart + 15] = 4 * i;
            tris[trisStart + 16] = 4 * i + 3;
            tris[trisStart + 17] = 4 * i - 1;
            
            // левая боковая плоскость
            tris[trisStart + 18] = 4 * i - 3;
            tris[trisStart + 19] = 4 * i - 2;
            tris[trisStart + 20] = 4 * i + 1;
            
            tris[trisStart + 21] = 4 * i + 1;
            tris[trisStart + 22] = 4 * i - 2;
            tris[trisStart + 23] = 4 * i + 2;
        }

        int trisEnd = (2 + 8 * _points.Count) * 3;
        int vertsEnd = 4 * (_points.Count - 1);

        tris[trisEnd] = vertsEnd;
        tris[trisEnd + 1] = vertsEnd + 1;
        tris[trisEnd + 2] = vertsEnd + 2;
        
        tris[trisEnd + 3] = vertsEnd + 2;
        tris[trisEnd + 4] = vertsEnd + 3;
        tris[trisEnd + 5] = vertsEnd;

        mesh.vertices = verts;
        mesh.triangles = tris;
        
        mesh.Optimize();

        _meshFilter.mesh = mesh;
        _meshCollider.sharedMesh = mesh;
    }
}
