namespace GEDCOMLibrary
{
    [System.Flags]
    public enum EventType : byte
    {
        Birth = 0b_0000_0000,
        Death = 0b_0000_0010,
        Residence = 0b_0000_0100,
        Marriage = 0b_0000_1000,
        Other = 0b_1000_0000,
    }
}