namespace MikuMikuMethods.Converter;

/// <summary>
/// PMMからカメラモーションを抽出する際のオプション
/// </summary>
/// <param name="StartFrame">開始フレーム</param>
/// <param name="EndFrame">終了フレーム nullなら全フレーム</param>
/// <param name="Camera">カメラ</param>
/// <param name="Light">照明</param>
/// <param name="Shadow">セルフ影</param>
public sealed record CameraMotionExtractionOptions(uint StartFrame = 0, uint? EndFrame = null, bool Camera = true, bool Light = false, bool Shadow = false);

/// <summary>
/// PMMからモデルモーションを抽出する際のオプション
/// </summary>
/// <param name="StartFrame">開始フレーム</param>
/// <param name="EndFrame">終了フレーム nullなら全フレーム</param>
/// <param name="Motion">モーション</param>
/// <param name="Morph">モーフ</param>
/// <param name="Property">表示・IK</param>
public sealed record ModelMotionExtractionOptions(uint StartFrame = 0, uint? EndFrame = null, bool Motion = true, bool Morph = true, bool Property = true);

/// <summary>
/// VMDのカメラモーションを適用する際のオプション
/// </summary>
/// <param name="Offset">適用開始フレーム</param>
/// <param name="Camera">カメラ</param>
/// <param name="Light">照明</param>
/// <param name="Shadow">セルフ影</param>
public sealed record CameraMotionApplyingOptions(uint Offset = 0, bool Camera = true, bool Light = true, bool Shadow = true);

/// <summary>
/// VMDのモデルモーションを適用する際のオプション
/// </summary>
/// <param name="Offset">適用開始フレーム</param>
/// <param name="Motion">モーション</param>
/// <param name="Morph">モーフ</param>
/// <param name="Property">表示・IK</param>
public sealed record ModelMotionApplyingOptions(uint Offset = 0, bool Motion = true, bool Morph = true, bool Property = true);