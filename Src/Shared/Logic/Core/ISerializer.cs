﻿using System.Text;

namespace TPUM.Shared.Logic.Core
{
    public interface ISerializer<T>
    {
        Encoding Encoding { get; }
        IFormatter<T> Formatter { get; }

        byte[] Serialize(T obj);
        T Deserialize(byte[] bytes);
    }
}