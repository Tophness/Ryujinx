﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ARMeilleure.Decoders
{
    class OpCode32SimdS : OpCode32, IOpCode32Simd
    {
        public int Vd { get; private set; }
        public int Vm { get; private set; }
        public int Opc { get; protected set; }
        public int Size { get; protected set; }
        public bool Q { get; private set; }
        public int Elems => 1;

        public OpCode32SimdS(InstDescriptor inst, ulong address, int opCode) : base(inst, address, opCode)
        {
            Opc = (opCode >> 15) & 0x3;
            Size = (opCode >> 8) & 0x3;

            var single = Size != 0b11;

            RegisterSize = single ? RegisterSize.Simd32 : RegisterSize.Simd64;

            if (single)
            {
                Vm = ((opCode >> 5) & 0x1) | ((opCode << 1) & 0x1e);
                Vd = ((opCode >> 22) & 0x1) | ((opCode >> 11) & 0x1e);
            }
            else
            {
                Vm = ((opCode >> 1) & 0x10) | ((opCode >> 0) & 0xf);
                Vd = ((opCode >> 18) & 0x10) | ((opCode >> 12) & 0xf);
            }
        }
    }
}
