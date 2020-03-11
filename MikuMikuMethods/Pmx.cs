using System;
using System.Collections.Generic;

namespace MikuMikuMethods
{
    namespace Pmx
    {
        public class Pmx
        {
            private static readonly byte[] MAGIC_BYTES = { 0x50, 0x4d, 0x58, 0x20 };// PMX

            /// <summary>ヘッダ get</summary>
            public Header Header { get; private set; }

            /// <summary>モデル情報 get</summary>
            public ModelInfo ModelInfo { get; private set; }

            /// <summary>頂点リスト get</summary>
            public List<Vertex> Vertex { get; private set; }

            /// <summary>材質リスト(各材質内に対応した面リストを格納) get</summary>
            public List<Material> Material { get; private set; }

            /// <summary>ボーンリスト get</summary>
            public List<Bone> Bone { get; private set; }

            /// <summary>モーフリスト get</summary>
            public List<Morph> Morph { get; private set; }

            /// <summary>ルート用表示枠(システム管理用) get</summary>
            public Node RootNode { get; private set; }

            /// <summary>表情枠(システム管理用) get</summary>
            public Node ExpressionNode { get; private set; }

            /// <summary>表示枠リスト get</summary>
            public List<Node> Node { get; private set; }

            /// <summary>剛体リスト get</summary>
            public List<Body> Body { get; private set; }

            /// <summary>Jointリスト get</summary>
            public List<Joint> Joint { get; private set; }

            /// <summary>SoftBodyリスト get</summary>
            public List<SoftBody> SoftBody { get; private set; }

            /// <summary>
            /// 正規化 - 未参照のオブジェクト関係を初期化します(Update時は内部で自動的に正規化します)
            ///
            /// 正規化手順
            /// 1.参照先頂点が存在しない場合 -&gt; 面:削除 頂点／UVモーフ:オフセット削除
            /// 2.参照先ボーンが存在しない場合 -&gt; 頂点:-1 ボーン親:-1 ボーン子:-1 IK_Target:非IK化 IK_Link:Link削除 ボーンモーフ:オフセット削除 ボーン枠:削除 剛体:-1
            /// 3.参照先モーフが存在しない場合 -&gt; グループモーフ:オフセット削除 モーフ枠:削除
            /// 4.参照先剛体が存在しない場合 -&gt; Joint:-1
            /// </summary>
            public void Normalize()
            {
                throw new NotImplementedException("Pmx.Normalizeメソッドは未実装です");
            }

            /// <summary>初期化</summary>
            void Clear()
            {
                throw new NotImplementedException("Pmx.Clearメソッドは未実装です");
            }

            /// <summary>ファイルパス(FromFile()で読み込む場合自動設定:各プレビュー処理に使用)</summary>
            public string FilePath { get; set; }

            public void Read(string path)
            {
                //throw new NotImplementedException("Pmx.Readメソッドは未実装です");
                FilePath = path;


            }
        }
        public class Body
        {
            public Body()
            {
                throw new NotImplementedException("Pmx.Bodyクラスは未実装です");
            }
        }
        public class Bone
        {
            public Bone()
            {
                throw new NotImplementedException("Pmx.Boneクラスは未実装です");
            }
        }
        public class Node
        {
            public Node()
            {
                throw new NotImplementedException("Pmx.Nodeクラスは未実装です");
            }
        }
        public class Header
        {
            public Header()
            {
                throw new NotImplementedException("Pmx.Headerクラスは未実装です");
            }
        }
        public class Joint
        {
            public Joint()
            {
                throw new NotImplementedException("Pmx.Jointクラスは未実装です");
            }
        }
        public class Material
        {
            public Material()
            {
                throw new NotImplementedException("Pmx.Materialクラスは未実装です");
            }
        }
        public class ModelInfo
        {
            public ModelInfo()
            {
                throw new NotImplementedException("Pmx.ModelInfoクラスは未実装です");
            }
        }
        public class Morph
        {
            public Morph()
            {
                throw new NotImplementedException("Pmx.Morphクラスは未実装です");
            }
        }
        public class SoftBody
        {
            public SoftBody()
            {
                throw new NotImplementedException("Pmx.SoftBodyクラスは未実装です");
            }
        }
        public class Vertex
        {
            public Vertex()
            {
                throw new NotImplementedException("Pmx.Vertexクラスは未実装です");
            }
        }
    }
}
