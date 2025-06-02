using UnityEngine;
using UnityEditor;

public class PhysicsLayerSetup : EditorWindow
{
    [MenuItem("Tools/Setup Physics Layers")]
    public static void SetupLayers()
    {
        // Создаем слои, если их нет
        CreateLayer("Player");
        CreateLayer("Enemy");
        CreateLayer("Projectile");
        CreateLayer("Environment");
        
        // Настраиваем матрицу коллизий
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Projectile"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Projectile"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Projectile"), LayerMask.NameToLayer("Projectile"), true);
        
        Debug.Log("Physics layers have been set up!");
    }
    
    private static void CreateLayer(string layerName)
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty layers = tagManager.FindProperty("layers");
        
        bool layerExists = false;
        for (int i = 8; i < layers.arraySize; i++)
        {
            SerializedProperty layerSP = layers.GetArrayElementAtIndex(i);
            if (layerSP.stringValue == layerName)
            {
                layerExists = true;
                break;
            }
        }
        
        if (!layerExists)
        {
            for (int i = 8; i < layers.arraySize; i++)
            {
                SerializedProperty layerSP = layers.GetArrayElementAtIndex(i);
                if (layerSP.stringValue == "")
                {
                    layerSP.stringValue = layerName;
                    tagManager.ApplyModifiedProperties();
                    break;
                }
            }
        }
    }
} 