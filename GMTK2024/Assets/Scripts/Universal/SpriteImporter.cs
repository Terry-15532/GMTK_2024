// using UnityEditor;
//
// #if  UNITY_EDITOR
// public class SpriteImporter : AssetPostprocessor {
//     private void OnPreprocessTexture() {
//         var textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
//         if (!textureImporter) return;
//         textureImporter.textureType = TextureImporterType.Sprite;
//         textureImporter.mipmapEnabled = false;
//         textureImporter.spriteImportMode = SpriteImportMode.Single;
//
//         //var settings = new TextureImporterPlatformSettings() {
//         //    overridden = true,
//         //    name = "iPhone",
//         //    maxTextureSize = textureImporter.name.Contains("Icon") ? 256 : 1024,
//         //    format = TextureImporterFormat.ASTC_6x6
//         //};
//         //textureImporter.SetPlatformTextureSettings(settings);
//         //settings.name = "Android";
//         //settings.format = TextureImporterFormat.ASTC_4x4;
//         //textureImporter.SetPlatformTextureSettings(settings);
//     }
// }
// #endif
