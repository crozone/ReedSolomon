using System;
using Xunit;

using ReedSolomon;
using System.Text;
using Xunit.Abstractions;
using System.Diagnostics;

namespace ReedSolomonTests
{
    public class UnitTest1
    {
        private readonly Random random;
        private readonly ITestOutputHelper output;

        public UnitTest1(ITestOutputHelper output)
        {
            this.output = output;
            this.random = new Random(1234);
        }

        [Fact]
        public void TestEncodeFixed()
        {
            // A test block with data, but no parity.
            //
            Span<byte> testBlock = stackalloc byte[Rs8.BlockLength] {
                0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
                0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f,
                0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2a, 0x2b, 0x2c, 0x2d, 0x2e, 0x2f,
                0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f,
                0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f,
                0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5a, 0x5b, 0x5c, 0x5d, 0x5e, 0x5f,
                0x60, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f,
                0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7a, 0x7b, 0x7c, 0x7d, 0x7e, 0x7f,
                0x80, 0x81, 0x82, 0x83, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8a, 0x8b, 0x8c, 0x8d, 0x8e, 0x8f,
                0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x98, 0x99, 0x9a, 0x9b, 0x9c, 0x9d, 0x9e, 0x9f,
                0xa0, 0xa1, 0xa2, 0xa3, 0xa4, 0xa5, 0xa6, 0xa7, 0xa8, 0xa9, 0xaa, 0xab, 0xac, 0xad, 0xae, 0xaf,
                0xb0, 0xb1, 0xb2, 0xb3, 0xb4, 0xb5, 0xb6, 0xb7, 0xb8, 0xb9, 0xba, 0xbb, 0xbc, 0xbd, 0xbe, 0xbf,
                0xc0, 0xc1, 0xc2, 0xc3, 0xc4, 0xc5, 0xc6, 0xc7, 0xc8, 0xc9, 0xca, 0xcb, 0xcc, 0xcd, 0xce, 0xcf,
                0xd0, 0xd1, 0xd2, 0xd3, 0xd4, 0xd5, 0xd6, 0xd7, 0xd8, 0xd9, 0xda, 0xdb, 0xdc, 0xdd, 0xde, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };

            // Encode the test block to add parity
            //
            Rs8.Encode(testBlock.Slice(0, Rs8.DataLength), testBlock.Slice(Rs8.DataLength, Rs8.ParityLength));

            // The expected output from a known working encoder
            //
            Span<byte> correctBlock = stackalloc byte[Rs8.BlockLength] {
                0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
                0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f,
                0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2a, 0x2b, 0x2c, 0x2d, 0x2e, 0x2f,
                0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f,
                0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f,
                0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5a, 0x5b, 0x5c, 0x5d, 0x5e, 0x5f,
                0x60, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f,
                0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7a, 0x7b, 0x7c, 0x7d, 0x7e, 0x7f,
                0x80, 0x81, 0x82, 0x83, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8a, 0x8b, 0x8c, 0x8d, 0x8e, 0x8f,
                0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x98, 0x99, 0x9a, 0x9b, 0x9c, 0x9d, 0x9e, 0x9f,
                0xa0, 0xa1, 0xa2, 0xa3, 0xa4, 0xa5, 0xa6, 0xa7, 0xa8, 0xa9, 0xaa, 0xab, 0xac, 0xad, 0xae, 0xaf,
                0xb0, 0xb1, 0xb2, 0xb3, 0xb4, 0xb5, 0xb6, 0xb7, 0xb8, 0xb9, 0xba, 0xbb, 0xbc, 0xbd, 0xbe, 0xbf,
                0xc0, 0xc1, 0xc2, 0xc3, 0xc4, 0xc5, 0xc6, 0xc7, 0xc8, 0xc9, 0xca, 0xcb, 0xcc, 0xcd, 0xce, 0xcf,
                0xd0, 0xd1, 0xd2, 0xd3, 0xd4, 0xd5, 0xd6, 0xd7, 0xd8, 0xd9, 0xda, 0xdb, 0xdc, 0xdd, 0xde, 0x2f,
                0xbd, 0x4f, 0xb4, 0x74, 0x84, 0x94, 0xb9, 0xac, 0xd5, 0x54, 0x62, 0x72, 0x12, 0xee, 0xb3, 0xeb,
                0xed, 0x41, 0x19, 0x1d, 0xe1, 0xd3, 0x63, 0x20, 0xea, 0x49, 0x29, 0x0b, 0x25, 0xab, 0xcf
            };

            // Ensure the encoded output matches the expected output
            //
            Assert.True(correctBlock.SequenceEqual(testBlock));
        }

        [Fact]
        public void TestDecodeFixed()
        {
            // An encoded test block with 16 bad bytes
            //
            Span<byte> testBlock = stackalloc byte[Rs8.BlockLength] {
                0x58, 0x01, 0xa1, 0x03, 0x04, 0xc8, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x03, 0x0f,
                0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f,
                0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2a, 0x2b, 0x2c, 0x2d, 0x2e, 0x2f,
                0x30, 0x31, 0xf8, 0xa5, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f,
                0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f,
                0x4b, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0xfb, 0x5a, 0xf7, 0x5c, 0x5d, 0x5e, 0x5f,
                0x60, 0x61, 0x62, 0x63, 0xdd, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f,
                0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7a, 0x7b, 0x7c, 0x7d, 0x7e, 0x7f,
                0x80, 0x81, 0x82, 0x83, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8a, 0x8b, 0x8c, 0x8d, 0x8e, 0x8f,
                0x90, 0x74, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x98, 0x99, 0x9a, 0x9b, 0x9c, 0x9d, 0x9e, 0x9f,
                0xa0, 0xa1, 0xa2, 0xa3, 0xa4, 0xa5, 0xa6, 0xa7, 0xa8, 0xa9, 0xaa, 0xab, 0xac, 0xad, 0xae, 0xaf,
                0x24, 0xb1, 0xb2, 0xb3, 0xb4, 0xb5, 0xb6, 0xb7, 0xb8, 0x7a, 0xba, 0xbb, 0xbc, 0xbd, 0xbe, 0xbf,
                0xc0, 0xc1, 0xc2, 0xc3, 0xc4, 0xc5, 0xc6, 0xc7, 0xc8, 0xc9, 0xca, 0xcb, 0xcc, 0xcd, 0xce, 0xcf,
                0xd0, 0xd1, 0xd2, 0xd3, 0xd4, 0xd5, 0x41, 0xd7, 0xd8, 0xd9, 0xda, 0xdb, 0xdc, 0xdd, 0xde, 0x2f,
                0xbd, 0x4f, 0xb4, 0x74, 0x84, 0x94, 0xb9, 0xd6, 0xd5, 0x54, 0x62, 0x72, 0x12, 0xee, 0xb3, 0xeb,
                0xed, 0x41, 0x19, 0x1d, 0xe1, 0xd3, 0x63, 0x20, 0xea, 0x49, 0x00, 0x0b, 0x25, 0xab, 0xcf
            };

            // Decode the test block to correct errors
            //
            int correctedByteCount = Rs8.Decode(testBlock, Span<int>.Empty);

            Span<byte> correctBlock = stackalloc byte[Rs8.BlockLength] {
                0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
                0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f,
                0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2a, 0x2b, 0x2c, 0x2d, 0x2e, 0x2f,
                0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f,
                0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f,
                0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5a, 0x5b, 0x5c, 0x5d, 0x5e, 0x5f,
                0x60, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f,
                0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7a, 0x7b, 0x7c, 0x7d, 0x7e, 0x7f,
                0x80, 0x81, 0x82, 0x83, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8a, 0x8b, 0x8c, 0x8d, 0x8e, 0x8f,
                0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x98, 0x99, 0x9a, 0x9b, 0x9c, 0x9d, 0x9e, 0x9f,
                0xa0, 0xa1, 0xa2, 0xa3, 0xa4, 0xa5, 0xa6, 0xa7, 0xa8, 0xa9, 0xaa, 0xab, 0xac, 0xad, 0xae, 0xaf,
                0xb0, 0xb1, 0xb2, 0xb3, 0xb4, 0xb5, 0xb6, 0xb7, 0xb8, 0xb9, 0xba, 0xbb, 0xbc, 0xbd, 0xbe, 0xbf,
                0xc0, 0xc1, 0xc2, 0xc3, 0xc4, 0xc5, 0xc6, 0xc7, 0xc8, 0xc9, 0xca, 0xcb, 0xcc, 0xcd, 0xce, 0xcf,
                0xd0, 0xd1, 0xd2, 0xd3, 0xd4, 0xd5, 0xd6, 0xd7, 0xd8, 0xd9, 0xda, 0xdb, 0xdc, 0xdd, 0xde, 0x2f,
                0xbd, 0x4f, 0xb4, 0x74, 0x84, 0x94, 0xb9, 0xac, 0xd5, 0x54, 0x62, 0x72, 0x12, 0xee, 0xb3, 0xeb,
                0xed, 0x41, 0x19, 0x1d, 0xe1, 0xd3, 0x63, 0x20, 0xea, 0x49, 0x29, 0x0b, 0x25, 0xab, 0xcf
            };

            // Check that the corrected byte count actually matches the number of errors
            //
            Assert.Equal(16, correctedByteCount);

            // Check that the decoded test block matches the correct encoded block.
            //
            Assert.Equal(correctBlock.ToArray(), testBlock.ToArray());
        }


        [Fact]
        public void TestRandomFuzz()
        {
            Stopwatch watch = new Stopwatch();
            int iterations = 1;
            output.WriteLine($"Starting test from 0.0 -> 1.0 chance with {iterations} iterations");
            for (double i = 0; i <= 1; i += 0.01)
            {
                output.WriteLine($"{i},{iterations}");

                for (int j = 0; j < iterations; j++)
                {
                    DoTest(i);
                }
            }
        }

        private void DoTest(double corruptChance)
        {
            Span<byte> testPayload = stackalloc byte[Rs8.BlockLength];
            Span<byte> corruptPayload = stackalloc byte[Rs8.BlockLength];

            // Generate random data using the corrupt method at 100%
            //
            CorruptSpan(testPayload.Slice(0, Rs8.DataLength), 1);

            output.WriteLine("Test data:");
            output.WriteLine(FormatSpan(testPayload));

            // Calculate parity with encode
            //
            Rs8.Encode(testPayload.Slice(0, Rs8.DataLength), testPayload.Slice(Rs8.DataLength, Rs8.ParityLength));

            output.WriteLine("Encoded block:");
            output.WriteLine(FormatSpan(testPayload));

            // Copy the test payload
            //
            testPayload.CopyTo(corruptPayload);

            // Corrupt the corrupt payload
            //
            CorruptSpan(corruptPayload, corruptChance);

            // Count how many bytes were actually corrupted
            //
            int corruptedByteCount = CompareSpans(testPayload, corruptPayload);

            output.WriteLine($"Corrupted block ({corruptedByteCount} bytes corrupted):");
            output.WriteLine(FormatSpan(corruptPayload));

            // Correct the corrupted array with decode
            //
            int correctedByteCount = Rs8.Decode(corruptPayload, null);

            // Check if there were any differences between the original
            // block and the corrected block
            //
            int corruptedByteCountAfterDecode = CompareSpans(testPayload, corruptPayload);

            output.WriteLine($"Corrected block ({correctedByteCount} bytes corrected, {corruptedByteCountAfterDecode} errors remain):");
            output.WriteLine(FormatSpan(corruptPayload));

            if (correctedByteCount < 0)
            {
                // Decode failed. This is expected if there were too many errors.
                // Check that errors were excessive.
                //
                Assert.True(corruptedByteCount > Rs8.ParityLength / 2);
            }
            else
            {
                // The decoder appears to have succeeded.
                // Ensure that the reported corrected bytes count
                // matches the number actually corrupted.
                //
                Assert.Equal(corruptedByteCount, correctedByteCount);

                // Verify that the block now matches the uncorrupted block
                //
                Assert.Equal(0, corruptedByteCountAfterDecode);
            }
        }

        private string FormatSpan(Span<byte> span)
        {
            StringBuilder stringBuilder = new StringBuilder(span.Length * 4);

            for (int i = 0; i < span.Length; i++)
            {
                if (i % 16 == 0) {
                    stringBuilder.AppendLine();
                }

                stringBuilder.AppendFormat("0x{0:X2}, ", span[i]);
            }
            stringBuilder.AppendLine();

            return stringBuilder.ToString();
        }

        private int CompareSpans(Span<byte> span1, Span<byte> Span2)
        {
            if (span1.Length != Span2.Length) throw new ArgumentException("Span lengths must be equal");

            int differenceCount = 0;
            for (int i = 0; i < span1.Length; i++)
            {
                if (span1[i] != Span2[i])
                {
                    differenceCount++;
                }
            }

            return differenceCount;
        }

        private void CorruptSpan(Span<byte> span, double chance)
        {
            for (int i = 0; i < span.Length; i++)
            {
                if (chance > 0 && (chance >= 1 || random.NextDouble() < chance))
                {
                    span[i] ^= (byte)random.Next(1, 255);
                }
            }
        }
    }
}
