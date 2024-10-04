using System.Text;
using System;
using UnityEditor;
using UnityEditorInternal;
using System.IO;
using System.Linq;
using UnityEngine;

public class NamesCreator : MonoBehaviour
{
    // �����ȕ������Ǘ�����z��
    private static readonly string[] INVALUD_CHARS =
    {
        " ", "!", "\"", "#", "$",
        "%", "&", "\'", "(", ")",
        "-", "=", "^",  "~", "\\",
        "|", "[", "{",  "@", "`",
        "]", "}", ":",  "*", ";",
        "+", "/", "?",  ".", ">",
        ",", "<"
    };

    private const string ITEM_NAME = "ExTools/CreateNames";    // �R�}���h��
    private const string PATH = "Assets/01_GameData/Scripts/Internal/Names.cs";        // �t�@�C���p�X

    private static readonly string FILENAME = Path.GetFileName(PATH);                   // �t�@�C����(�g���q����)
    private static readonly string FILENAME_WITHOUT_EXTENSION = Path.GetFileNameWithoutExtension(PATH);   // �t�@�C����(�g���q�Ȃ�)

    /// <summary>
    /// ���C���[����萔�ŊǗ�����N���X���쐬���܂�
    /// </summary>
    [MenuItem(ITEM_NAME, priority = 0)]
    public static void Create()
    {
        if (!CanCreate())
        {
            return;
        }

        CreateScript();

        EditorUtility.DisplayDialog(FILENAME, "Create success.", "OK");
    }

    /// <summary>
    /// �X�N���v�g���쐬���܂�
    /// </summary>
    public static void CreateScript()
    {
        var builder = new StringBuilder();

        //  �V�[��Enum
        builder.AppendLine("/// <summary>");
        builder.AppendLine("/// �V�[������Enum�ŊǗ�����N���X");
        builder.AppendLine("/// </summary>");
        builder.AppendFormat("internal enum SceneName").AppendLine();
        builder.AppendLine("{");

        foreach (var n in EditorBuildSettings.scenes
            .Select(x => Path.GetFileNameWithoutExtension(x.path))
            .Distinct()
            .Select(c => $"{RemoveInvalidChars(c)},"))
        {
            builder.Append("\t").AppendFormat(@"{0},", RemoveInvalidChars(n)).AppendLine();
        }

        builder.AppendLine("}");
        builder.AppendLine();

        //  �^�O��
        builder.AppendLine("/// <summary>");
        builder.AppendLine("/// �^�O���𕶎���ŊǗ�����N���X");
        builder.AppendLine("/// </summary>");
        builder.AppendFormat("internal static class TagName").AppendLine();
        builder.AppendLine("{");

        foreach (var n in InternalEditorUtility.tags.
            Select(c => new { var = RemoveInvalidChars(c), val = c }))
        {
            builder.Append("\t").AppendFormat(@"public const string {0} = ""{1}"";", n.var, n.val).AppendLine();
        }

        builder.AppendLine("}");
        builder.AppendLine();

        //  ���C���[�萔
        builder.AppendLine("/// <summary>");
        builder.AppendLine("/// ���C���[����萔�ŊǗ�����N���X");
        builder.AppendLine("/// </summary>");
        builder.AppendFormat("internal static class LayerName").AppendLine();
        builder.AppendLine("{");

        foreach (var n in InternalEditorUtility.layers.
            Select(c => new { var = RemoveInvalidChars(c), val = LayerMask.NameToLayer(c) }))
        {
            builder.Append("\t").AppendFormat(@"public const int {0} = {1};", n.var, n.val).AppendLine();
        }
        foreach (var n in InternalEditorUtility.layers.
            Select(c => new { var = RemoveInvalidChars(c), val = 1 << LayerMask.NameToLayer(c) }))
        {
            builder.Append("\t").AppendFormat(@"public const int {0}Mask = {1};", n.var, n.val).AppendLine();
        }

        builder.AppendLine("}");
        builder.AppendLine();

        //  ����
        var directoryName = Path.GetDirectoryName(PATH);
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }

        File.WriteAllText(PATH, builder.ToString(), Encoding.UTF8);
        AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
    }

    /// <summary>
    /// �Ǘ�����N���X���쐬�ł��邩�ǂ������擾���܂�
    /// </summary>
    [MenuItem(ITEM_NAME, true)]
    public static bool CanCreate()
    {
        return !EditorApplication.isPlaying && !Application.isPlaying && !EditorApplication.isCompiling;
    }

    /// <summary>
    /// �����ȕ������폜���܂�
    /// </summary>
    public static string RemoveInvalidChars(string str)
    {
        Array.ForEach(INVALUD_CHARS, c => str = str.Replace(c, string.Empty));
        return str;
    }
}
