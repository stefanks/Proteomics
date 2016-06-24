namespace Proteomics
{
    public class DigestionPoint
    {
        public int index { get; private set; }
        public int length { get; private set; }

        public DigestionPoint(int index, int length)
        {
            this.index = index;
            this.length = length;
        }
    }
}