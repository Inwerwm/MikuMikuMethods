using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MikuMikuMethods
{
    /// <summary>
    /// ボーン名の比較クラス
    /// </summary>
    public class BoneNameComparer : IComparer<string>
    {
        static private int orderCount;

        static BoneNameComparer()
        {
            orderCount = 0;
        }

        /// <summary>
        /// 比較
        /// </summary>
        /// <returns>
        /// <para>0未満 - このインスタンスはotherよりソート順が前である</para>
        /// <para>0 - このインスタンスはotherとソート順が同じである</para>
        /// <para>0超 - このインスタンスはotherよりソート順が後である</para>
        /// </returns>
        public int Compare(string x, string y)
        {
            if (x == y)
                return 0;

            var xo = new Ordo(x);
            var yo = new Ordo(y);

            return xo.CompareTo(yo);
        }

        /// <summary>
        /// 順序構造体
        /// </summary>
        private struct Ordo : IComparable<Ordo>
        {
            //ボーン種類
            public BoneCategory Category;
            //左右などの方向
            public Direct Direction;
            //ボーン名の通し番号
            public int NameOrder;
            //多段化された親ボーンか
            public bool isParent;
            //添数字
            public int NumerusOrdinis;
            //ボーン名
            public string StemName;

            public Ordo(string name)
            {
                Category = BoneCategory.Other;
                NameOrder = -1;
                orderCount = 0;
                getBoneOrder(ref NameOrder, ref Category, name, "全ての親", BoneCategory.Center);
                getBoneOrder(ref NameOrder, ref Category, name, "センター", BoneCategory.Center);
                getBoneOrder(ref NameOrder, ref Category, name, "グルーブ", BoneCategory.Center);
                getBoneOrder(ref NameOrder, ref Category, name, "腰", BoneCategory.Center);
                getBoneOrder(ref NameOrder, ref Category, name, "頭", BoneCategory.Head);
                getBoneOrder(ref NameOrder, ref Category, name, "首", BoneCategory.Head);
                getBoneOrder(ref NameOrder, ref Category, name, "上半身2", BoneCategory.Trunk);
                if (Category == BoneCategory.Other)//上半身2であった場合カテゴリが上書きされるのを防ぐ
                    getBoneOrder(ref NameOrder, ref Category, name, "上半身", BoneCategory.Trunk);
                getBoneOrder(ref NameOrder, ref Category, name, "下半身", BoneCategory.Trunk);
                getBoneOrder(ref NameOrder, ref Category, name, "肩", BoneCategory.Arm);
                getBoneOrder(ref NameOrder, ref Category, name, "腕", BoneCategory.Arm);
                getBoneOrder(ref NameOrder, ref Category, name, "腕捩", BoneCategory.Arm);
                getBoneOrder(ref NameOrder, ref Category, name, "ひじ", BoneCategory.Arm);
                getBoneOrder(ref NameOrder, ref Category, name, "ヒジ", BoneCategory.Arm);
                getBoneOrder(ref NameOrder, ref Category, name, "手捩", BoneCategory.Arm);
                getBoneOrder(ref NameOrder, ref Category, name, "手首", BoneCategory.Arm);
                if (Category == BoneCategory.Other)//手首であった場合カテゴリが上書きされるのを防ぐ
                    getBoneOrder(ref NameOrder, ref Category, name, "手", BoneCategory.Arm);
                getBoneOrder(ref NameOrder, ref Category, name, "握り", BoneCategory.Finger);
                getBoneOrder(ref NameOrder, ref Category, name, "拡散", BoneCategory.Finger);
                getBoneOrder(ref NameOrder, ref Category, name, "親指", BoneCategory.Finger);
                getBoneOrder(ref NameOrder, ref Category, name, "人差指", BoneCategory.Finger);
                getBoneOrder(ref NameOrder, ref Category, name, "人指", BoneCategory.Finger);
                getBoneOrder(ref NameOrder, ref Category, name, "中指", BoneCategory.Finger);
                getBoneOrder(ref NameOrder, ref Category, name, "薬指", BoneCategory.Finger);
                getBoneOrder(ref NameOrder, ref Category, name, "小指", BoneCategory.Finger);
                getBoneOrder(ref NameOrder, ref Category, name, "足", BoneCategory.Leg);
                getBoneOrder(ref NameOrder, ref Category, name, "ひざ", BoneCategory.Leg);
                getBoneOrder(ref NameOrder, ref Category, name, "足首", BoneCategory.Leg);
                getBoneOrder(ref NameOrder, ref Category, name, "足先", BoneCategory.Leg);
                getBoneOrder(ref NameOrder, ref Category, name, "足IK", BoneCategory.Leg);
                getBoneOrder(ref NameOrder, ref Category, name, "つま先IK", BoneCategory.Leg);
                getBoneOrder(ref NameOrder, ref Category, name, "目", BoneCategory.Eye);
                getBoneOrder(ref NameOrder, ref Category, name, "髪", BoneCategory.Hair);

                if (NameOrder == -1)
                    NameOrder = orderCount;

                (Direction, StemName, isParent, NumerusOrdinis) = NameAnalysis(name, Category);
            }

            public int CompareTo(Ordo target)
            {
                if (Category != target.Category)
                    return Category.CompareTo(target.Category);
                //同カテゴリが残る

                //カテゴリ内で名前より方向を揃えることを優先するカテゴリであるか？
                if ((Category == BoneCategory.Other) || (Category == BoneCategory.Hair))
                {
                    if (StemName != target.StemName)
                        return StemName.CompareTo(target.StemName);
                    //同名ボーンが残る

                    if (Direction != target.Direction)
                        return Direction.CompareTo(target.Direction);
                    //同方向が残る
                }
                else
                {
                    if (Direction != target.Direction)
                        return Direction.CompareTo(target.Direction);
                    //同方向が残る

                    if (NameOrder != target.NameOrder)
                        return NameOrder.CompareTo(target.NameOrder);
                    //同名ボーンが残る

                }

                if (isParent)
                    return target.NumerusOrdinis.CompareTo(NumerusOrdinis);
                //親ボーンでないものが残る

                else if ((NumerusOrdinis != 0) && (target.NumerusOrdinis != 0))
                    return NumerusOrdinis.CompareTo(target.NumerusOrdinis);
                //添数字が無いものが残る

                return StemName.CompareTo(target.StemName);
            }
        }

        private enum BoneCategory
        {
            Center,
            Eye,
            Head,
            Trunk,
            Arm,
            Finger,
            Leg,
            Hair,
            Other
        }

        private enum Direct
        {
            None,
            Left,
            Right,
            Front,
            Back,
            Both,
            Cross
        }

        static private (Direct Direction, string name, bool isParent, int NumerusOidinis) NameAnalysis(string str, BoneCategory category)
        {
            (Direct Direction, string name, bool isParent, int NumerusOidinis) result;

            //方向
            result.Direction = Direct.None;
            if (0 <= str.IndexOf("左"))
                result.Direction = Direct.Left;
            if (0 <= str.IndexOf("右"))
                result.Direction = Direct.Right;

            //両目と寄目
            if (category == BoneCategory.Eye)
            {
                if (0 == str.IndexOf("両"))
                    result.Direction = Direct.Both;
                if (0 == str.IndexOf("寄"))
                    result.Direction = Direct.Cross;
            }

            //方向情報の文字を削除
            //寄目の場合表記ゆれ（寄目/寄り目）に対応
            switch (result.Direction)
            {
                case Direct.Left:
                case Direct.Right:
                case Direct.Both:
                    str = str.Remove(0, 1);
                    break;
                case Direct.Cross:
                    if (0 == str.IndexOf("寄り"))
                        str = str.Remove(0, 2);
                    else
                        str = str.Remove(0, 1);
                    break;
                default:
                    break;
            }

            //親ボーンの誤検知対策
            var cStr = str.Replace("全ての親", "全てのおや");
            cStr = cStr.Replace("親指", "おや指");
            //親ボーン判定
            int pPlace = cStr.IndexOf("親");
            result.isParent = 0 <= pPlace;

            //添数字読み取り
            cStr = str.Replace("上半身2", "上半身に");
            cStr = cStr.Replace("上半身２", "上半身に");
            //全角数字を半角に
            string num = ZenToHanNum(cStr);
            //半角数字のみ抽出
            num = Regex.Replace(num, @"[^0-9]", "");
            //文字がなくなったら0に
            if (num == "")
                num = "0";
            //数値化
            result.NumerusOidinis = int.Parse(num);

            //親ボーンの添字削除
            if (result.isParent)
                str = str.Remove(pPlace);
            //添数字削除
            str = Regex.Replace(str, @"[0-9]", "");

            result.name = str;
            return result;
        }

        static private string ZenToHanNum(string s)
        {
            return Regex.Replace(s, "[０-９]", p => ((char)(p.Value[0] - '０' + '0')).ToString());
        }

        static private void getBoneOrder(ref int order, ref BoneCategory category, string source, string query, BoneCategory setCategory = BoneCategory.Other)
        {
            System.Globalization.CompareInfo ci = System.Globalization.CultureInfo.CurrentCulture.CompareInfo;
            //source内にqueryが含まれていた場合、序数とカテゴリを代入
            if (0 <= ci.IndexOf(source, query, System.Globalization.CompareOptions.IgnoreWidth))
            {
                order = orderCount;
                category = setCategory;
            }
            orderCount++;
        }
    }
}
