using System.Collections.Generic;
using System.Drawing;

namespace MarkAble2
{
    public class PictureRecs:List<PictureRec>
    {
        public Image GetImageWithType(PictureRec.PictureTypes pictype)
        {
            foreach(var picrec in this)
            {
                if (picrec.PictureType == pictype)
                    return picrec.Picture;
            }

            return null;
        }
    }
}