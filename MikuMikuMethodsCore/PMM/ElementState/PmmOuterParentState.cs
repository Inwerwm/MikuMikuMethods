﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMM.ElementState
{
    public record PmmOuterParentState
    {
        /// <summary>
        /// 外部親が設定されたフレーム
        /// </summary>
        public int? StartFrame { get; set; }
        /// <summary>
        /// 外部親が変化する直前のフレーム
        /// </summary>
        public int? EndFrame { get; set; }

        public PmmModel ParentModel { get; set; }
        public PmmBone ParentBone { get; set; }
    }
}