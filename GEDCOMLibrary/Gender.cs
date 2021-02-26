namespace GEDCOMLibrary
{

    [System.Flags]
    public enum Gender : byte
    {
        Male = 0b_0000_0000,
        Female = 0b_0000_0001,
        Unspecified = 0b_0000_0010,
    }
}