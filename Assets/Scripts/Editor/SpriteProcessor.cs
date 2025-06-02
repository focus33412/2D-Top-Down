using UnityEngine;
using UnityEditor;
using System.IO;

public class SpriteProcessor : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        TextureImporter textureImporter = assetImporter as TextureImporter;
        if (textureImporter != null)
        {
            // Включаем чтение/запись пикселей
            textureImporter.isReadable = true;
            
            // Настраиваем импорт для спрайтов
            textureImporter.textureType = TextureImporterType.Sprite;
            
            // Устанавливаем фильтрацию
            textureImporter.filterMode = FilterMode.Point;
            
            // Отключаем сжатие для пиксельной графики
            textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
            
            // Настраиваем настройки спрайта
            TextureImporterSettings settings = new TextureImporterSettings();
            textureImporter.ReadTextureSettings(settings);
            settings.spriteMeshType = SpriteMeshType.FullRect;
            settings.spriteGenerateFallbackPhysicsShape = true;
            textureImporter.SetTextureSettings(settings);
        }
    }
} 