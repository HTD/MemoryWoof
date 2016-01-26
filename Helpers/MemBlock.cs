﻿using System;
using System.Runtime.InteropServices;

namespace CodeDog.System {

    public class MemBlock<T> {

        #region Properties

        public ulong Length { get; set; }

        public ulong Size { get; set; }

        #endregion

        #region Private Data

        const int PAGE_SIZE_LOG2 = 17; // PageSize 1MB

        T[][] Pages;
        int PageSize;
        int PageCount;

        #endregion

        public MemBlock(ulong size, ulong clearLsb = 0x01) {
            var elementSize = (ulong)Marshal.SizeOf<T>();
            Length = size / elementSize;
            Length++;
            Length >>= PAGE_SIZE_LOG2;
            Length <<= PAGE_SIZE_LOG2;
            if (clearLsb > 0) Length &= ~clearLsb;
            Size = Length * elementSize;
            PageSize = 1 << PAGE_SIZE_LOG2;
            PageCount = (int)(Length >> PAGE_SIZE_LOG2);
            Pages = new T[PageCount][];
            for (int i = 0; i < PageCount; i++) Pages[i] = new T[PageSize];
        }

        public T this[ulong i] {
            get {
                int page = (int)(i >> PAGE_SIZE_LOG2);
                int offset = (int)(i - ((ulong)page << PAGE_SIZE_LOG2));
                return Pages[page][offset];
            }
            set {
                int page = (int)(i >> PAGE_SIZE_LOG2);
                int offset = (int)(i - ((ulong)page << PAGE_SIZE_LOG2));
                Pages[page][offset] = value;
            }
        }

        public double ToGibi() => Math.Round(Size / (double)0x40000000, 3);

        public override string ToString() => $"[{ToGibi()}GB]";

    }

}