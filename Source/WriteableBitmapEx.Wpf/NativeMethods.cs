using System;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;

namespace System.Windows.Media.Imaging
{
    internal static class NativeMethods
    {
        [TargetedPatchingOptOut("Internal method only, inlined across NGen boundaries for performance reasons")]
        internal static void CopyUnmanagedMemory(IntPtr srcPtr, int srcOffset, IntPtr dstPtr, int dstOffset, int count)
        {
			srcPtr = srcPtr.Add<byte>( srcOffset );
			dstPtr = dstPtr.Add<byte>( dstOffset );

			memcpy(dstPtr, srcPtr, (UInt32)count );
        }

        [TargetedPatchingOptOut("Internal method only, inlined across NGen boundaries for performance reasons")]
        internal static void SetUnmanagedMemory(IntPtr dst, int filler, int count)
        {
            memset(dst, filler, count);
        }

		// Win32 memory copy function
		/// <summary>
		/// Copies characters between buffers.
		/// </summary>
		/// <param name="dst">New buffer</param>
		/// <param name="src">Buffer to copy from</param>
		/// <param name="count">Number of characters to copy</param>
		/// <returns>returns the value of dest.</returns>
		[DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        private static extern IntPtr memcpy(
			[In] IntPtr dst,
			[In] IntPtr src,
			[In] UInt32 count );

		// Win32 memory set function
		/// <summary>
		/// Sets buffers to a specified character.
		/// </summary>
		/// <param name="dst">Pointer to destination</param>
		/// <param name="c">Character to set</param>
		/// <param name="count">Number of characters</param>
		[DllImport("msvcrt.dll", EntryPoint = "memset", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        private static extern void memset(
            IntPtr dst,
            int filler,
            int count);
    }
}
