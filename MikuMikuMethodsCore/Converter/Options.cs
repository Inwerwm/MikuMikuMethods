using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.Converter;

/// <summary>
/// PMMデータからカメラ情報を抽出する際のオプション
/// </summary>
/// <param name="StartFrame">抽出開始フレーム</param>
/// <param name="EndFrame">抽出終了フレーム nullなら全フレーム</param>
/// <param name="ExtractCamera">カメラフレームを抽出する</param>
/// <param name="ExtractLight">照明フレームを抽出する</param>
/// <param name="ExtractShadow">影フレームを抽出する</param>
public sealed record ExtractCameraMotionOptions(uint StartFrame = 0, uint? EndFrame = null, bool ExtractCamera = true, bool ExtractLight = false, bool ExtractShadow = false);

/// <summary>
/// PMMデータからモデルモーション情報を抽出する際のオプション
/// </summary>
/// <param name="StartFrame">抽出開始フレーム</param>
/// <param name="EndFrame">抽出終了フレーム nullなら全フレーム</param>
public sealed record ExtractModelMotionOptions(uint StartFrame = 0, uint? EndFrame = null);
