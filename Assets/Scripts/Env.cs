using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

public enum DisplayType
{
    None = 0,
    Cylinder,
    Cone,
    DoubleCone,
}

public enum StatisticType
{
    None = 0,
    Origin,
    Density,
}

[System.Serializable]
public class Env : MonoBehaviour
{
    // Tracking data section.
    [HideInInspector] public DisplayType displayType;
    [HideInInspector] public StatisticType statisticType;
    [HideInInspector] public int densityPointCount;
    
    // Tracking data storage.
    DisplayType recentDisplayType;
    StatisticType recentStatisticType;
    int recentDensityPointCount;
    
    // Other things...
    public Image refImage;
    public Text fileNameText;
    public GameObject meshBed;
    public MeshFilter meshRendererSource;
    public float maxRadius;
    bool shouldUpdate;
    
    void Start()
    {
        UnityEngine.Application.targetFrameRate = 30;
    }
    
    void Update()
    {
        shouldUpdate |= recentDisplayType != displayType;
        recentDisplayType = displayType;
        shouldUpdate |= recentStatisticType != statisticType;
        recentStatisticType = statisticType;
        
        if(shouldUpdate) Setup();
    }
    
    public Vector3 ColorToWorldSpacePoint(Color x)
    {
        Color.RGBToHSV(x, out var h, out var s, out var v);
        var angle = (0.5f - h) * Mathf.PI * 2;
        
        switch(displayType)
        {
            case DisplayType.Cylinder:
            {
                return new Vector3(
                    Mathf.Cos(angle) * s,
                    v - 0.5f,
                    Mathf.Sin(angle) * s
                );
            }
                
            case DisplayType.Cone:
            {
                var r = s * v;
                return new Vector3(
                    Mathf.Cos(angle) * r,
                    v - 0.5f,
                    Mathf.Sin(angle) * r 
                );
            }
            
            case DisplayType.DoubleCone:
            {
                var r = s * v / ((1 - v) * s + v);
                var y =  s / ((1 - v) * (1 - s) - 1) * (2 - v) + 1;
                return new Vector3(
                    Mathf.Cos(angle) * r,
                    y,
                    Mathf.Sin(angle) * r
                );
            }
            default: throw new System.Exception();
            
        }
    }
    
    public List<Mesh> SetupMesh((Vector3 pos, Color color)[] data)
    {
        var meshes = new List<Mesh>();
        var verts = new List<Vector3>();
        var colors = new List<Color>();
        var indices = new List<int>();
        
        void SubmitMesh(MeshTopology topo = MeshTopology.Points)
        {
            Mesh mesh = new Mesh();
            mesh.SetVertices(verts);
            mesh.SetColors(colors);
            mesh.SetIndices(indices.ToArray(), topo, 0);
            mesh.RecalculateBounds();
            meshes.Add(mesh);
            verts.Clear();
            colors.Clear();
            indices.Clear();
        }
        
        int SubmitSphere(Vector3 pos, Color color, float radius)
        {
            int pointCount = 0;
            const int horiziontalDivision = 6;
            const int verticalDivision = 4;
            for(int i=0; i<horiziontalDivision; i++)
            for(int j=0; j<verticalDivision; j++)
            {
                var horizontalAngle = i * Mathf.PI * 2 / horiziontalDivision;
                var verticleAngle = j * Mathf.PI / verticalDivision - Mathf.PI * 0.5f;
                var nextHorizontalAngle = (i + 1) * Mathf.PI * 2 / horiziontalDivision;
                var nextVerticleAngle = (j + 1) * Mathf.PI / verticalDivision - Mathf.PI * 0.5f;
                var lb = Quaternion.Euler(0, 0, verticleAngle * Mathf.Rad2Deg) * Quaternion.Euler(0, horizontalAngle * Mathf.Rad2Deg, 0) * Vector3.right * radius;
                var lt = Quaternion.Euler(0, 0, nextVerticleAngle * Mathf.Rad2Deg) * Quaternion.Euler(0, horizontalAngle * Mathf.Rad2Deg, 0) * Vector3.right * radius;
                var rb = Quaternion.Euler(0, 0, verticleAngle * Mathf.Rad2Deg) * Quaternion.Euler(0, nextHorizontalAngle * Mathf.Rad2Deg, 0) * Vector3.right * radius;
                var rt = Quaternion.Euler(0, 0, nextVerticleAngle * Mathf.Rad2Deg) * Quaternion.Euler(0, nextHorizontalAngle * Mathf.Rad2Deg, 0) * Vector3.right * radius;
                verts.Add(pos + lb);
                verts.Add(pos + lt);
                verts.Add(pos + rb);
                verts.Add(pos + rt);
                colors.Add(color);
                colors.Add(color);
                colors.Add(color);
                colors.Add(color);
                var baseInd = verts.Count - 4;
                indices.Add(baseInd);
                indices.Add(baseInd + 1);
                indices.Add(baseInd + 3);
                indices.Add(baseInd);
                indices.Add(baseInd + 2);
                indices.Add(baseInd + 3);
                pointCount += 4;
            }
            return pointCount;
        }
        
        switch(statisticType)
        {
            case StatisticType.Origin:
            {
                var used = new HashSet<Vector3>();
                foreach(var (pos, color) in data)
                {
                    if(used.Contains(pos)) continue;
                    used.Add(pos);
                    verts.Add(pos);
                    colors.Add(color);
                    indices.Add(indices.Count);
                    if(verts.Count >= 60000) SubmitMesh();
                }
                if(verts.Count != 0) SubmitMesh();
            }
            break;
            
            case StatisticType.Density:
            {
                var cnt = new Dictionary<Vector3, (Color color, int c)>();
                foreach(var (pos, color) in data)
                {
                    if(cnt.ContainsKey(pos)) cnt[pos] = (cnt[pos].color, cnt[pos].c + 1);
                    else cnt[pos] = (color, 1);
                }
                
                foreach(var i in cnt)
                {
                    var pos = i.Key;
                    var color = i.Value.color;
                    var cc = i.Value.c;
                    var radius = (1 - Mathf.Exp(-cc * 0.1f)) * maxRadius;
                    SubmitSphere(pos, color, radius);
                    
                    if(verts.Count >= 60000) SubmitMesh(MeshTopology.Triangles);
                }
                if(verts.Count != 0) SubmitMesh();
            }
            break;
            
            default: throw new System.Exception();
        }
        return meshes;
    }
    
    public void Setup()
    {
        if(refImage.sprite == null) return;
        
        shouldUpdate = false;
        
        var color = refImage.sprite.texture.ToColorArray();
        var data = color.Cast<Color>().Select(x => (ColorToWorldSpacePoint(x), x)).ToArray();
        var meshes = SetupMesh(data);
        for(int i=0; i<meshBed.transform.childCount; i++)
        {
            var ch = meshBed.transform.GetChild(i).gameObject;
            Destroy(ch);
        }
        foreach(var i in meshes) 
        {
            var x = Instantiate(meshRendererSource.gameObject, meshBed.transform).GetComponent<MeshFilter>();
            x.mesh = i;
        }
    }
    
    public void LoadTexture()
    {
        var path = fileNameText.text;
        
        // Show the loaded file path.
        fileNameText.text = path;
        
        // Load the image.
        var tex = ImageProcessing.ReadImage(path);
        
        // Show the image as a sprite.
        refImage.sprite = Sprite.Create(
            tex,
            new Rect(0, 0, tex.width, tex.height),
            Vector2.zero,
            256,
            0,
            SpriteMeshType.FullRect,
            new Vector4(0, 0, tex.width, tex.height),
            false
        );
        
        // Resize the image to fit the ratio of the picture.
        var h = refImage.rectTransform.rect.height;
        var w = h / tex.height * tex.width;
        refImage.rectTransform.offsetMax = refImage.rectTransform.offsetMin + new Vector2(w, h);
        
        // Notify it should be updated.
        shouldUpdate = true;
    }
}
