using MikuMikuMethods.PMM.ElementState;

namespace MikuMikuMethods.PMM.Frame
{
    public class PmmAccessoryFrame : PmmAccessoryState, IPmmFrame
    {
        public int Frame { get; set; }
        public bool IsSelected { get; set; }
        /// <summary>
        /// 所属アクセサリー
        /// </summary>
        public PmmAccessory Parent { get; set; }
    }
}