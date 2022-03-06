namespace MikuMikuMethods.Converter;

/// <summary>
/// PMMデータからカメラ情報を抽出する際のオプション
/// </summary>
/// <param name="StartFrame">開始フレーム</param>
/// <param name="EndFrame">終了フレーム nullなら全フレーム</param>
/// <param name="Camera">カメラ</param>
/// <param name="Light">照明</param>
/// <param name="Shadow">セルフ影</param>
public sealed record CameraMotionExtractionOptions(uint StartFrame = 0, uint? EndFrame = null, bool Camera = true, bool Light = false, bool Shadow = false);

/// <summary>
/// PMMデータからモデルモーション情報を抽出する際のオプション
/// </summary>
/// <param name="StartFrame">開始フレーム</param>
/// <param name="EndFrame">終了フレーム nullなら全フレーム</param>
/// <param name="Motion">モーション</param>
/// <param name="Morph">モーフ</param>
/// <param name="Property">表示・IK</param>
public sealed record ModelMotionExtractionOptions(uint StartFrame = 0, uint? EndFrame = null, bool Motion = true, bool Morph = true, bool Property = true);
