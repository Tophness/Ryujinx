using Ryujinx.Common;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Ryujinx.HLE.HOS.Services.SurfaceFlinger
{
    struct GraphicBuffer
    {
        public GraphicBufferHeader Header { get; private set; }
        public NvGraphicBuffer     Buffer { get; private set; }

        public int Width => Header.Width;
        public int Height => Header.Height;
        public int Format => Header.Format;
        public int Usage => Header.Usage;

        public int StructSize => Marshal.SizeOf<NvGraphicBuffer>() + Marshal.SizeOf<GraphicBufferHeader>();

        public GraphicBuffer(BinaryReader reader)
        {
            Header = reader.ReadStruct<GraphicBufferHeader>();

            // ignore fds
            // TODO: check if that is used in official implementation
            reader.BaseStream.Position += Header.FdsCount * 4;

            if (Header.IntsCount != 0x51)
            {
                throw new NotImplementedException($"Unexpected Graphic Buffer ints count (expected 0x51, found 0x{Header.IntsCount:x}");
            }

            Buffer = reader.ReadStruct<NvGraphicBuffer>();
        }

        public void Write(BinaryWriter writer)
        {
            writer.WriteStruct(Header);
            writer.WriteStruct(Buffer);
        }

        public Rect ToRect()
        {
            return new Rect(Width, Height);
        }
    }
}