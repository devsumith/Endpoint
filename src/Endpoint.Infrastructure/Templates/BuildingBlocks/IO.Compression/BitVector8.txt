// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace IO.Compression;


public static class BitVector8
{
    public static void Pack(ReadOnlySpan<byte> input, int sizeInBits, Span<byte> buffer, int index = 0, int bitIndex = 7)
    {
        for (int j = 0; j < input.Length; j++)
        {
            int numberOfBits = sizeInBits % 8 > 0 ? sizeInBits % 8 : 8;

            sizeInBits -= numberOfBits;

            int value = input[j];

            while (numberOfBits > 0)
            {
                if (index >= buffer.Length) return;

                var numberOfBitsThatCanBePacked = bitIndex + 1;

                if (numberOfBits <= numberOfBitsThatCanBePacked)
                {
                    var mask = (1 << numberOfBits) - 1;

                    buffer[index] |= (byte)((value & mask) << (numberOfBitsThatCanBePacked - numberOfBits));

                    bitIndex -= numberOfBits;

                    if (bitIndex == -1)
                    {
                        bitIndex = 7;
                        index++;
                    }

                    numberOfBits = 0;
                }
                else
                {
                    var mask = ((1 << numberOfBitsThatCanBePacked) - 1) << (numberOfBits - numberOfBitsThatCanBePacked);
                    buffer[index] |= (byte)((value & mask) >>> (numberOfBits - numberOfBitsThatCanBePacked));
                    bitIndex = 7;
                    index++;
                    numberOfBits -= +numberOfBitsThatCanBePacked;
                }
            }
        }
    }

    public static byte[] Unpack(byte[] buffer, int take, int index = 0, int offset = 0)
    {
        int size = (take + offset + 7) / 8;

        ReadOnlySpan<byte> source = Slice(buffer, index, size, offset);

        byte[] destination = new byte[(take + 7) / 8];

        int sourceIndex = 0;

        int destinationIndex = 0;

        int unpackedBits = 0;

        while (sourceIndex < source.Length && destinationIndex < destination.Length && take - unpackedBits > 0)
        {
            if (take - unpackedBits > 8)
            {
                destination[destinationIndex] = source[sourceIndex];
                unpackedBits += 8;
            }
            else
            {
                var mask = (1 << 8) - 1 << (8 - (take - unpackedBits)) & 0xFF;

                destination[destinationIndex] = (byte)(source[sourceIndex] & mask);

                unpackedBits += take;
            }

            destinationIndex++;

            sourceIndex++;
        }

        return destination;
    }

    public static Span<byte> Slice(byte[] buffer, int index, int size, int offset)
    {
        if (offset == 0) return new Span<byte>(buffer, index, size);

        if (size == 1)
        {
            return new Span<byte>(new byte[1]
            {
                (byte)(buffer[index] << offset)
            }, 0, 1);
        }

        Span<byte> destinantion = new byte[size];

        for (int i = index; i < size + index - 1; i++)
        {
            destinantion[i - index] = (byte)(buffer[i] << offset);

            int mask = ((1 << 8) - 1) << 8 - offset & 0xFF;

            destinantion[i - index] |= (byte)((buffer[i + 1] & mask) >> 8 - offset);
        }

        destinantion[size - 1] = (byte)(buffer[index + size - 1] << offset);

        return destinantion;
    }
}