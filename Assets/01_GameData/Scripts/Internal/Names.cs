﻿/// <summary>
/// シーン名をEnumで管理するクラス
/// </summary>
internal enum SceneName
{
	SampleScene,
}

/// <summary>
/// タグ名を文字列で管理するクラス
/// </summary>
internal static class TagName
{
	public const string Untagged = "Untagged";
	public const string Respawn = "Respawn";
	public const string Finish = "Finish";
	public const string EditorOnly = "EditorOnly";
	public const string MainCamera = "MainCamera";
	public const string Player = "Player";
	public const string GameController = "GameController";
}

/// <summary>
/// レイヤー名を定数で管理するクラス
/// </summary>
internal static class LayerName
{
	public const int Default = 0;
	public const int TransparentFX = 1;
	public const int IgnoreRaycast = 2;
	public const int Water = 4;
	public const int UI = 5;
	public const int DefaultMask = 1;
	public const int TransparentFXMask = 2;
	public const int IgnoreRaycastMask = 4;
	public const int WaterMask = 16;
	public const int UIMask = 32;
}

