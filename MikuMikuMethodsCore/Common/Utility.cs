﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods
{
    /// <summary>
    /// 便利メソッド集
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// queryを新しい連番で整列する
        /// </summary>
        /// <param name="map">もとの連番と新しい連番の変換マップリスト</param>
        /// <param name="query">変換対象リスト</param>
        /// <returns>変換されたリスト</returns>
        public static List<T> OrderByMap<T>(List<int> map, List<T> query) where T : new()
        {
            List<T> r = new();

            for (int i = 0; i < map.Count; i++)
            {
                if (map[i] < 0)
                    continue;

                int id = map[i];
                while (r.Count <= id)
                {
                    r.Add(new T());
                }

                if (query.Count > i)
                    r[id] = query[i];
            }

            return r;
        }

        /// <summary>
        /// 引数の中身を入れ替える
        /// </summary>
        public static void Swap<T>(ref T first, ref T second)
        {
            T tmp = first;
            first = second;
            second = tmp;
        }
    }
}
