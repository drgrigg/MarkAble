using MarkAble2.Properties;

namespace MarkAble2
{
    //this is just for convenience to fill combo box
    public class TrackOrFileInterval
    {
        public int Interval = 1;

        public TrackOrFileInterval(int interval)
        {
            Interval = interval;
        }

        public override string ToString()
        {
            if (Interval == 1)
                return Resources.Each;

            int rem100 = Interval%100;

            if ((rem100 >= 11) && (rem100 <= 13))
            {
                return Resources.Every + " " + Interval.ToString() + Resources.Ordinal_th;
            }

            int rem10 = Interval%10;

            switch (rem10)
            {
                case 1:
                    return Resources.Every + " " + Interval.ToString() + Resources.Ordinal_st;
                case 2:
                    return Resources.Every + " " + Interval.ToString() + Resources.Ordinal_nd;
                case 3:
                    return Resources.Every + " " + Interval.ToString() + Resources.Ordinal_rd;
                default:
                    return Resources.Every + " " + Interval.ToString() + Resources.Ordinal_th;
            }
        }
    }
}
