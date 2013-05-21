namespace IBCS.Interfaces
{
    public interface PublicKey
    {
        string GetAlgorithm();
        byte[] GetEncoded();
        string GetFormat();
    }
}
