using System;

namespace IBCS.Interfaces
{
    public interface FieldElement
    {
        sbyte[] ToByteArray();
        byte[] ToUByteArray();
        String ToString(int radix);
    }
}

