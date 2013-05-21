namespace IBCS.Interfaces
{
    public interface PrivateKey
    {
        string GetAlgorithm();
        byte[] GetEncoded();
        string GetFormat();
    }
}
