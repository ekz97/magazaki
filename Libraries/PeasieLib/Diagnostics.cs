using Peasie.Contracts.Interfaces;

namespace PeasieLib
{
    public class Diagnostics: IToHtmlTable
    {
        public int PID { get; }
        public long Allocated { get; }
        public long WorkingSet { get; }
        public long PrivateBytes { get; }
        public int Gen0 { get; }
        public int Gen1 { get; }
        public int Gen2 { get; }
        public double CPU { get; }
        public double RPS { get; }

        public Diagnostics(int pID, long allocated, long workingSet, long privateBytes, int gen0, int gen1, int gen2, double cPU, double rPS)
        {
            PID = pID;
            Allocated = allocated;
            WorkingSet = workingSet;
            PrivateBytes = privateBytes;
            Gen0 = gen0;
            Gen1 = gen1;
            Gen2 = gen2;
            CPU = cPU;
            RPS = rPS;
        }

        public override bool Equals(object? obj)
        {
            return obj is Diagnostics other &&
                   PID == other.PID &&
                   Allocated == other.Allocated &&
                   WorkingSet == other.WorkingSet &&
                   PrivateBytes == other.PrivateBytes &&
                   Gen0 == other.Gen0 &&
                   Gen1 == other.Gen1 &&
                   Gen2 == other.Gen2 &&
                   CPU == other.CPU &&
                   RPS == other.RPS;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(PID);
            hash.Add(Allocated);
            hash.Add(WorkingSet);
            hash.Add(PrivateBytes);
            hash.Add(Gen0);
            hash.Add(Gen1);
            hash.Add(Gen2);
            hash.Add(CPU);
            hash.Add(RPS);
            return hash.ToHashCode();
        }
    }
}