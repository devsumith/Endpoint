// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Buffers.Binary;

namespace IO.Compression.Primitives;

public record struct Int16Type: IPackable
{
    public Int16Type(Int16 value)
    {
        Value = value;
    }

    public Int16Type(byte[] bytes)
    {
        Value = BinaryPrimitives.ReadInt16BigEndian(bytes);
    }

    public Int16 Value { get; }

    public Int16Type SizeInBits => (Int16Type)16;

    public void Pack(Span<byte> buffer, int index, int bitIndex)
    {
        Span<byte> bytes = stackalloc byte[4];

        BinaryPrimitives.WriteInt16BigEndian(bytes, Value);

        BitVector8.Pack(bytes, 16, buffer, index, bitIndex);
    }

    public static implicit operator Int16(Int16Type type)
    {
        return type.Value;
    }

    public static explicit operator Int16Type(Int16 value)
    {
        return new Int16Type(value);
    }

}