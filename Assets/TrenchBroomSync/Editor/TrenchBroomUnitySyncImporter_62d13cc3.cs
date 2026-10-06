using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class TrenchBroomUnitySyncImporter_62d13cc3
{
    private const string Prefab = "Assets/TrenchBroomSync/Prefabs/TrenchBroomMap.prefab";

    private const float PrefabScale = 0.1f;

    private static readonly string[] Models = new string[]
    {
        "Assets/TrenchBroomSync/Models/brush_0001.obj",
    };

    private static readonly string[] ObjectNames = new string[]
    {
        "brush_0001",
    };

    private static readonly string[] MaterialNames = new string[]
    {
        "__TB_empty",
    };

    private static readonly string[] TexturePaths = new string[]
    {
        "",
    };

    private static readonly string[] MaterialPaths = new string[]
    {
        "Assets/TrenchBroomSync/Materials/__TB_empty.mat",
    };

    static TrenchBroomUnitySyncImporter_62d13cc3()
    {
        EditorApplication.delayCall += Sync;
    }

    [MenuItem("Tools/TrenchBroom/Regenerate Synced Prefabs")]
    public static void Sync()
    {
        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        var materials = EnsureMaterials();
        var directory = Path.GetDirectoryName(Prefab);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var root = new GameObject(Path.GetFileNameWithoutExtension(Prefab));
        root.transform.localScale = Vector3.one * PrefabScale;
        try
        {
            for (var i = 0; i < Models.Length; ++i)
            {
                var model = AssetDatabase.LoadAssetAtPath<GameObject>(Models[i]);
                if (model == null)
                {
                    Debug.LogWarning("TrenchBroom sync skipped missing model: " + Models[i]);
                    continue;
                }

                var child = PrefabUtility.InstantiatePrefab(model) as GameObject;
                if (child == null)
                {
                    child = Object.Instantiate(model);
                }
                child.name = i < ObjectNames.Length && !string.IsNullOrEmpty(ObjectNames[i])
                    ? ObjectNames[i]
                    : Path.GetFileNameWithoutExtension(Models[i]);
                child.transform.SetParent(root.transform, false);
                AssignMaterials(child, materials);
            }

            PrefabUtility.SaveAsPrefabAsset(root, Prefab);
        }
        finally
        {
            Object.DestroyImmediate(root);
        }

        AssetDatabase.SaveAssets();
        Debug.Log("TrenchBroom sync generated prefab: " + Prefab);
    }

    private static Dictionary<string, Material> EnsureMaterials()
    {
        var result = new Dictionary<string, Material>();
        var shader = FindShader();
        for (var i = 0; i < MaterialNames.Length && i < MaterialPaths.Length; ++i)
        {
            var materialPath = MaterialPaths[i];
            var material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
            if (material == null)
            {
                var materialDirectory = Path.GetDirectoryName(materialPath);
                if (!string.IsNullOrEmpty(materialDirectory))
                {
                    Directory.CreateDirectory(materialDirectory);
                }
                if (shader == null)
                {
                    Debug.LogWarning("TrenchBroom sync cannot create material because no supported shader was found: " + MaterialNames[i]);
                    continue;
                }
                material = new Material(shader);
                material.name = MaterialNames[i];
                if (i < TexturePaths.Length && !string.IsNullOrEmpty(TexturePaths[i]))
                {
                    var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(TexturePaths[i]);
                    if (texture != null)
                    {
                        material.mainTexture = texture;
                        if (material.HasProperty("_BaseMap")) material.SetTexture("_BaseMap", texture);
                        if (material.HasProperty("_MainTex")) material.SetTexture("_MainTex", texture);
                    }
                }
                AssetDatabase.CreateAsset(material, materialPath);
                EditorUtility.SetDirty(material);
            }
            result[MaterialNames[i]] = material;
        }
        return result;
    }

    private static Shader FindShader()
    {
        var shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null) shader = Shader.Find("Standard");
        if (shader == null) shader = Shader.Find("Diffuse");
        return shader;
    }

    private static void AssignMaterials(GameObject root, Dictionary<string, Material> materials)
    {
        foreach (var renderer in root.GetComponentsInChildren<Renderer>(true))
        {
            var rendererMaterials = renderer.sharedMaterials;
            var changed = false;
            for (var i = 0; i < rendererMaterials.Length; ++i)
            {
                var source = rendererMaterials[i];
                if (source == null) continue;
                var name = NormalizeMaterialName(source.name);
                if (materials.TryGetValue(name, out var replacement))
                {
                    rendererMaterials[i] = replacement;
                    changed = true;
                }
            }
            if (changed)
            {
                renderer.sharedMaterials = rendererMaterials;
            }
        }
    }

    private static string NormalizeMaterialName(string value)
    {
        if (string.IsNullOrEmpty(value)) return string.Empty;
        const string instanceSuffix = " (Instance)";
        if (value.EndsWith(instanceSuffix))
        {
            value = value.Substring(0, value.Length - instanceSuffix.Length);
        }
        return value;
    }
}
