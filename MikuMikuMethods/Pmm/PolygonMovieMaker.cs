using MikuMikuMethods.Common;
using MikuMikuMethods.Extension;
using MikuMikuMethods.Pmm.Frame;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MikuMikuMethods.Pmm;

/// <summary>
/// MMDプロジェクトファイル
/// </summary>
public class PolygonMovieMaker : ICloneable
{
    internal ObservableCollection<PmmAccessory> AccessoriesSubstance { get; private init; } = new();
    internal List<PmmAccessory> AccessoryRenderOrder { get; private init; } = new();

    internal ObservableCollection<PmmModel> ModelsSubstance { get; private init; } = new();
    internal List<PmmModel> ModelRenderOrder { get; private init; } = new();
    internal List<PmmModel> ModelCalculateOrder { get; private init; } = new();

    /// <summary>
    /// PMMファイルバージョン
    /// </summary>
    public string Version { get; internal set; } = "Polygon Movie maker 0002";
    /// <summary>
    /// 出力解像度
    /// </summary>
    public Size OutputResolution { get; set; } = new(1920, 1080);

    /// <summary>
    /// カメラ
    /// </summary>
    public PmmCamera Camera { get; private init; } = new();
    /// <summary>
    /// 照明
    /// </summary>
    public PmmLight Light { get; private init; } = new();
    /// <summary>
    /// セルフ影
    /// </summary>
    public PmmSelfShadow SelfShadow { get; private init; } = new();
    /// <summary>
    /// 物理
    /// </summary>
    public PmmPhysics Physics { get; private init; } = new();
    /// <summary>
    /// アクセサリー
    /// </summary>
    public IList<PmmAccessory> Accessories => AccessoriesSubstance;
    /// <summary>
    /// モデル
    /// </summary>
    public IList<PmmModel> Models => ModelsSubstance;

    /// <summary>
    /// 背景と音声
    /// </summary>
    public PmmBackGroundMedia BackGround { get; private init; } = new();

    /// <summary>
    /// 描画設定
    /// </summary>
    public PmmRenderConfig RenderConfig { get; private init; } = new();
    /// <summary>
    /// 編集対象の状態
    /// </summary>
    public PmmEditorState EditorState { get; private init; } = new();
    /// <summary>
    /// 各種操作パネル画面の状態
    /// </summary>
    public PmmPanelPane PanelPane { get; private init; } = new();
    /// <summary>
    /// 再生関連設定
    /// </summary>
    public PmmPlayConfig PlayConfig { get; private init; } = new();

    public PolygonMovieMaker()
    {
        AccessoriesSubstance.CollectionChanged += CreateOrderListSynchronizer(new[] { AccessoryRenderOrder });
        ModelsSubstance.CollectionChanged += CreateOrderListSynchronizer(new[] { ModelRenderOrder, ModelCalculateOrder });
    }

    /// <summary>
    /// ファイルから読み込む
    /// </summary>
    /// <param name="filePath">読み込むファイル名</param>
    public PolygonMovieMaker(string filePath) : this()
    {
        IO.PmmFileReader.Read(filePath, this);
    }

    /// <summary>
    /// ファイルに書き込む
    /// </summary>
    /// <param name="filePath">書き込むファイル名</param>
    public void Write(string filePath)
    {
        IO.PmmFileWriter.Write(filePath, this);
    }

    /// <summary>
    /// 最終フレームの位置を求める
    /// <para>
    /// 全フレームのソートを含むため、フレーム数が多いと重いので注意
    /// </para>
    /// </summary>
    /// <returns></returns>
    public int CalculateLastFrame()
    {
        static int lastFrameOf(IEnumerable<IPmmFrame> frames) => frames.OrderByDescending(f => f.Frame).FirstOrDefault()?.Frame ?? -1;

        return new Func<int>[]{
            () => lastFrameOf(Camera.Frames.Cast<IPmmFrame>()),
            () => lastFrameOf(Light.Frames.Cast<IPmmFrame>()),
            () => lastFrameOf(SelfShadow.Frames.Cast<IPmmFrame>()),
            () => lastFrameOf(Physics.GravityFrames.Cast<IPmmFrame>()),
            () => Accessories.Any() ? Accessories.Max(acs => lastFrameOf(acs.Frames)) : -1,
            () => Models.Any() ? Models.Max(model =>
                Math.Max(Math.Max(
                    model.Bones.Any() ? model.Bones.Max(bone => lastFrameOf(bone.Frames)) : -1,
                    model.Morphs.Any() ? model.Morphs.Max(morph => lastFrameOf(morph.Frames)) : -1),
                    lastFrameOf(model.ConfigFrames)
                )
            ) : -1,
        }.AsParallel().Select(f => f.Invoke()).Max();
    }

    public void SetRenderOrder(PmmAccessory accessory, byte renderOrder)
    {
        if (!Accessories.Contains(accessory)) throw new ArgumentException("The accessory for which the Render Order setting was tried does not contain the PMM.");
        AccessoryRenderOrder.Move(renderOrder, accessory);
    }

    public byte? GetRenderOrder(PmmAccessory accessory)
    {
        int order = AccessoryRenderOrder.IndexOf(accessory);
        return order < 0 ? null : (byte)order;
    }

    public void SetRenderOrder(PmmModel model, byte renderOrder)
    {
        if (!Models.Contains(model)) throw new ArgumentException("The model for which the Render Order setting was tried does not contain the PMM.");
        ModelRenderOrder.Move(renderOrder, model);
    }

    public byte? GetRenderOrder(PmmModel model)
    {
        int order = ModelRenderOrder.IndexOf(model);
        return order < 0 ? null : (byte)order;
    }

    public void SetCalculateOrder(PmmModel model, byte calculateOrder)
    {
        if (!Models.Contains(model)) throw new ArgumentException("The model for which the Calculate Order setting was tried does not contain the PMM.");
        ModelCalculateOrder.Move(calculateOrder, model);
    }

    public byte? GetCalculateOrder(PmmModel model)
    {
        var order = ModelCalculateOrder.IndexOf(model);
        return order < 0 ? null : (byte)order;
    }

    static NotifyCollectionChangedEventHandler CreateOrderListSynchronizer<T>(IEnumerable<List<T>> orderLists) =>
        (sender, e) =>
        {
            foreach (var list in orderLists)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        AddAll(e.NewItems?.Cast<T>() ?? Array.Empty<T>());
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        RemoveAll(e.OldItems?.Cast<T>() ?? Array.Empty<T>());
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        ReplaceAll();
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        RemoveAll(list.ToArray());
                        AddAll(sender as IEnumerable<T> ?? Array.Empty<T>());
                        break;
                    case NotifyCollectionChangedAction.Move:
                    default:
                        break;
                }

                void ReplaceAll()
                {
                    foreach (var item in e.OldItems?.Cast<T>() ?? Array.Empty<T>())
                    {
                        list.Remove(item);
                    }
                    foreach (var item in e.NewItems?.Cast<T>().Select((Item, Id) => (Item, Id)) ?? Array.Empty<(T, int)>())
                    {
                        list.Insert(e.OldStartingIndex + item.Id, item.Item);
                    }
                }

                void RemoveAll(IEnumerable<T> items)
                {
                    foreach (T item in items)
                    {
                        list.Remove(item);
                    }
                }

                void AddAll(IEnumerable<T> items)
                {
                    foreach (T item in items)
                    {
                        list.Add(item);
                    }
                }
            }
        };

    public PolygonMovieMaker DeepCopy()
    {
        var cloneModels = Models.Select(model => (Origin: model, Clone: model.DeepCopy(out var bm), BoneMap: bm));
        var modelMap = cloneModels.ToDictionary(t => t.Origin, t => t.Clone);
        var boneMap = cloneModels.SelectMany(t => t.BoneMap).ToDictionary(t => t.Key, t => t.Value);
        var acsMap = Accessories.Select(acs => (Origin: acs, Clone: acs.DeepCopy(modelMap, boneMap))).ToDictionary(t => t.Origin, t => t.Clone);

        var clone = new PolygonMovieMaker()
        {
            BackGround = BackGround.DeepCopy(),
            Camera = Camera.DeepCopy(modelMap, boneMap),
            EditorState = EditorState.DeepCopy(modelMap, acsMap),
            Light = Light.DeepCopy(),
            OutputResolution = OutputResolution with { },
            PanelPane = PanelPane.DeepCopy(),
            Physics = Physics.DeepCopy(),
            PlayConfig = PlayConfig.DeepCopy(),
            RenderConfig = RenderConfig.DeepCopy(),
            SelfShadow = SelfShadow.DeepCopy(),
            Version = Version
        };

        foreach (var (o, c) in modelMap)
        {
            clone.Models.Add(c);
        }

        foreach (var (o, c) in acsMap)
        {
            clone.Accessories.Add(c);
        }

        return clone;
    }

    public object Clone() => DeepCopy();
}
