// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Buffers.Binary;

namespace IO.Compression.Primitives;

public record struct Int32Type: IPackable
{
    public Int32Type(Int32 value)
    {
        Value = value;
    }

    public Int32Type(byte[] bytes)
    {
        Value = BinaryPrimitives.ReadInt32BigEndian(bytes);
    }

    public Int32 Value { get; }

    public Int16Type SizeInBits => (Int16Type)32;

    public void Pack(Span<byte> buffer, int index, int bitIndex)
    {
        Span<byte> bytes = stackalloc byte[4];

        BinaryPrimitives.WriteInt32BigEndian(bytes, Value);

        BitVector8.Pack(bytes, 32, buffer, index, bitIndex);
    }

    public static implicit operator Int32(Int32Type type)
    {
        return type.Value;
    }

    public static explicit operator Int32Type(Int32 value)
    {
        return new Int32Type(value);
    }

}