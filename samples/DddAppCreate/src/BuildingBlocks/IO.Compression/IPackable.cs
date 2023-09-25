// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using IO.Compression.Primitives;

namespace IO.Compression;

public interface IPackable
{
    void Pack(Span<byte> buffer, int index= 0, int bitIndex = 7);
    Int16Type SizeInBits { get; }
}